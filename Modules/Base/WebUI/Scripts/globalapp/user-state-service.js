/* globals console, $, pbaAPI, globalapp, application */
(function() {
    'use strict';

    // ############
    // DEPENDENCIES
    // ############

    var socketService = globalapp.socketService;
    var events = globalapp.events;

    // ###################
    // MAIN PRIVATE MODULE
    // ###################

    var UserState = {
        binders: [],

        cache: {}, // key: number (userId), value: userStatus

        isLoading: {}, // key: number (userId), value: boolean (isLoading)
        socketPool: {}, // key: number (userId), value: userStatus
    };

    // ###############
    // PRIVATE METHODS
    // ###############

    var _parseCustomStatus = function(customStatus) {
        if (customStatus && customStatus.toLowerCase) {
            customStatus = customStatus.toLowerCase();
        }

        switch (customStatus) {
            case 0:
            case '0':
            case 'ready':
                return 'ready';
            case 1:
            case '1':
            case 'away':
                return 'away';
            case 2:
            case '2':
            case 'dontdisturb':
                return 'dontDisturb';
            case 3:
            case '3':
            case 'disconnected':
                return 'disconnected';
            case 10:
            case '10':
            case 'inconversation':
                return 'inConversation';
        }

        console.error('Cannot parse custom status', customStatus);
        return null;
    };

    var _parseUserStatus = function(statusData) {
        return {
            id: statusData.UserId,
            isOnline: statusData.Online,
            status: _parseCustomStatus(statusData.CustomStatus),
        };
    };

    var _parseResolveOnline = function(resolvedData) {
        return {
            id: resolvedData.ID,
            isOnline: true,
            status: _parseCustomStatus(resolvedData.Status),
        };
    };

    var _askBinders = function() {
        var idHash = {};

        UserState.binders.forEach(function(binder) {
            binder.getIds().filter(function(id) {
                return !isNaN(parseInt(id));
            }).forEach(function(id) {
                idHash[id] = true;
            });
        });

        return Object.keys(idHash).map(function(strId) {
            return parseInt(strId);
        });
    };

    var _execBinders = function(userStatuses) {
        UserState.binders.forEach(function(binder) {
            binder.applyState(userStatuses);
        });
    };

    var _onSocketSignal = function(userStatus) {

        userStatus = _parseUserStatus(userStatus);

        if (!userStatus) {
            return;
        }

        // DISMISS IF CURRENT USER STATUS IS LOADING NOW
        if (UserState.isLoading[userStatus.id]) {
            UserState.socketPool[userStatus.id] = userStatus;
            return;
        }

        // PUT INTO/UPDATE CACHE
        UserState.cache[userStatus.id] = userStatus;

        // EXECUTE BINDERS
        _execBinders([ userStatus ]);
    };

    var _load = function(userIds, callback) {
        $.get('/users/resolveonline', $.param({ userIds: userIds }, true), function(data) {
            if (data && Array.isArray(data)) {
                callback(null, userIds, data.map(_parseResolveOnline));
            } else {
                callback(new Error());
            }
        });
    };

    var _onLoad = function(error, requestIds, response) {

        // remove ids from loading list
        requestIds.forEach(function(id) {
            UserState.isLoading[id] = false;
        });

        var users = [];

        // if ajax request was not successful,
        // emit changes from 'socket pool' for
        // the requested ids (and only for them)
        if (error) {
            users = requestIds.filter(function(id) {
                return UserState.socketPool[id];
            }).map(function(id) {
                return {
                    id: id,
                    isOnline: UserState.socketPool[id].isOnline,
                    status: UserState.socketPool[id].status
                };
            });
        } else {
            users = requestIds.map(function(id) {
                var socketVersion = UserState.socketPool[id];

                if (socketVersion) {

                    // clear 'socket pool' record
                    delete UserState.socketPool[id];

                    return socketVersion;
                }

                // get item from server response
                var item = response.filter(function(item) {
                    return item.id === id;
                })[0];

                return {
                    id: id,
                    isOnline: !!item,
                    status: item ? item.status : 'disconnected',
                };
            });
        }

        // cache/update cache with responsed data
        users.forEach(function(status) {
            UserState.cache[status.id] = status;
        });

        _execBinders(users);
    };

    // UPDATE STATE FROM CACHE (50 ms debounce)
    var _updateStateFromCache = pbaAPI.debounce(function() {

        // get non-repeating list of id from all binders
        var ids = _askBinders();

        // remove those, which is "marked" as "in-loading"
        ids = ids.filter(function(id) {
            return !UserState.isLoading[id];
        });

        // do not handle empty array
        if (!ids.length) return;

        // apply cached ones to a DOM
        _execBinders(ids.filter(function(id) {
            return !!UserState.cache[id];
        }).map(function(id) {
            return UserState.cache[id];
        }));
    }, { time: 50 });

    // UPDATE STATE FROM SERVER (1 second debounce)
    var _updateStateFromServer = pbaAPI.debounce(function() {

        // get non-repeating list of id from all binders
        var ids = _askBinders();

        // remove those, which is "marked" as "in-loading"
        ids = ids.filter(function(id) {
            return !UserState.isLoading[id];
        });

        // do not handle empty array
        if (!ids.length) return;

        // load user statuses from server
        _load(ids, _onLoad);
    }, { time: 1000 });

    // ###############
    // SERVICE METHODS
    // ###############

    var changeStatus = function(status) {
        socketService.invoke(events.user.changeStatus, status);
    };

    var addBinder = function(binder) {
        if (!binder || typeof binder.getIds !== 'function' || typeof binder.applyState !== 'function') {
            console.error('binder has bad interface, and would not be used!');
        }

        // copy methods to prevent outside effects
        var statefulBinder = {};
        statefulBinder.getIds = binder.getIds.bind(statefulBinder);
        statefulBinder.applyState = binder.applyState.bind(statefulBinder);

        UserState.binders.push(statefulBinder);
    };

    var updateState = function() {
        _updateStateFromCache();
        _updateStateFromServer();
    };

    var setCallStatus = function() {

        // GET CURRENT USER STATUS
        var userStatus = UserState.cache[application.currentUser.id];

        // IF THERE IS A CACHED USER STATE DATA
        if (userStatus) {

            // REMEMBER LAST USED CUSTOM STATUS BEFORE CHANGE IT TO THE 'InConversation'
            userStatus.lastStatus = userStatus.status;

            // CHANGE STATUS
            socketService.invoke(events.user.changeStatus, 'InConversation');
        }
    };

    var clearCallStatus = function() {

        // GET CURRENT USER STATUS
        var userStatus = UserState.cache[application.currentUser.id];

        // IF THERE IS A CACHED USER STATE DATA
        if (userStatus) {

            // CHANGE STATUS
            socketService.invoke(events.user.changeStatus, userStatus.lastStatus || 'Ready');
        }
    };

    // #################
    // LISTEN TO SOCKETS
    // #################

    socketService.on(events.user.onConnect, _onSocketSignal);
    socketService.on(events.user.onDisconnect, _onSocketSignal);
    socketService.on(events.user.onChangeStatus, _onSocketSignal);

    // ######
    // EXPORT
    // ######
    globalapp.userStateService = {
        addBinder: addBinder,
        updateState: updateState,
        changeStatus: changeStatus,

        setCallStatus: setCallStatus,
        clearCallStatus: clearCallStatus,
    };

}());
