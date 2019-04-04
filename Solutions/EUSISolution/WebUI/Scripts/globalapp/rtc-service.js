/* globals console, $, kendo, pbaAPI, globalapp, Favico, RTCMultiConnection, RecordRTC */
(function() {
    'use strict';

    // ############
    // DEPENDENCIES
    // ############

    var service = function () {
        var self = this;

        var callWindows = {};
        var callRequests = {};
        var requestTimers = {};

        var videoTypes = {
            0: 'capture',
            1: 'conference'
        };

        var config = {
            channel: null,
            userid: null,
            sessionid: null,
            videoSession: {
                audio: true,
                video: true
            },
            audioSession: {
                audio: true,
                video: false
            },
            sessionScreen: {
                screen: true,
                oneway: true
            },
            dontTransmit: true,
            extra: {},
            streams: {},
            peerConfigs: {},
            direction: 'many-to-many',
            iceServers: [
                         {
                             "url": "stun:85.31.177.238:3478",
                             "urls": "stun:85.31.177.238:3478"
                         },
                         {
                             "url": "turn:85.31.177.238:3478",
                             "urls": "turn:85.31.177.238:3478",
                             "credential": "pba",
                             "username": "pba"
                         }
            ],

            keepStreamsOpened: false,
            dontCaptureUserMedia: false,
            onstart: null,
            onstop: null,
            onconnect: null,
            ondisconnect: null,
            onopen: null,
            onstoprecord: null
        };

        var chosenSession = config.videoSession;

        var templateOtcomingPrivate = kendo.template('<div class="conf-call-request-user"><img src="/Files/GetImage?id=#= ToImage #&width=80&height=80&defImage" alt="" class="img-circle"><p>#= ToTitle #</p></div><div class="btn-group pull-right"><button type="button" class="k-button error btn-cancel-call"><i class="mdi mdi-power"></i> Отменить</button></div>');
        var templateIncomingPrivate = kendo.template('<div class="conf-call-request-user"><img src="/Files/GetImage?id=#= FromImage #&width=80&height=80&defImage" alt="" class="img-circle"><p>#= FromTitle #</p></div><div class="btn-group btn-group-justified"><button type="button" class="k-button error btn-cancel-call pull-left"><i class="mdi mdi-power"></i> Отменить</button><button type="button" class="k-button success btn-success-call-oneway pull-right"><i class="mdi mdi-eye-off"></i></button><button type="button" class="k-button success btn-success-call-audio pull-right"><i class="mdi mdi-microphone"></i></button><button type="button" class="k-button success btn-success-call pull-right"><i class="mdi mdi-video"></i></button></div>');
        var templateOtcomingPublic = kendo.template('<div class="conf-call-request-user"><img src="/Content/images/callus80.png" alt="" class="img-circle"><p>#= ConferenceTitle #</p></div><div class="btn-group pull-right"><button type="button" class="k-button error btn-cancel-call"><i class="mdi mdi-power"></i> Отменить</button></div>');
        var templateIncomingPublic = kendo.template('<div class="conf-call-request-user"><img src="/Files/GetImage?id=#= FromImage #&width=80&height=80&defImage" alt="" class="img-circle"><p>#= FromTitle #</p></div><div class="btn-group btn-group-justified"><button type="button" class="k-button error btn-cancel-call pull-left"><i class="mdi mdi-power"></i> Отменить</button><button type="button" class="k-button success btn-success-call-oneway pull-right"><i class="mdi mdi-eye-off"></i></button><button type="button" class="k-button success btn-success-call-audio pull-right"><i class="mdi mdi-microphone"></i></button><button type="button" class="k-button success btn-success-call pull-right"><i class="mdi mdi-video"></i></button></div>');

        var hasCall = function (request) {
            return callWindows[request.Key] && callRequests[request.Key];
        };


        var favTimer;

        var favicon = new Favico({
            animation: 'none'
        });

        var faviconAnimateStart = function () {
            var prevVal = ' ';
            favicon.badge(prevVal);

            favTimer = setInterval(function () {
                if (prevVal === ' ')
                    prevVal = '';
                else
                    prevVal = ' ';

                favicon.badge(prevVal);
            }, 500);
        };

        var faviconAnimateStop = function() {
            clearInterval(favTimer);
            favicon.badge('');
        };

        var showVideoRequest = function (request, soundUrls) {
            var modalTemplate, modalTitle;

            var stopCallingUrl = '/Content/sounds/skype_end.mp3';
            var callingUrl = '/Content/sounds/in.mp3';

            if (soundUrls && soundUrls.length) {
                if (soundUrls.length > 1) {
                    callingUrl = soundUrls[0];
                    stopCallingUrl = soundUrls[1];
                } else {
                    callingUrl = soundUrls[0];
                }
            }

            switch (request.DialogType) {
                case "PrivateMessage":
                    if (request.Self) {
                        modalTemplate = templateOtcomingPrivate;
                        modalTitle = 'Исходящий звонок';
                    } else {
                        modalTemplate = templateIncomingPrivate;
                        modalTitle = 'Входящий звонок';
                    }
                    break;
                case "PublicMessage":
                    if (request.Self) {
                        modalTemplate = templateOtcomingPublic;
                        modalTitle = 'Исходящий групповой звонок';
                    } else {
                        modalTemplate = templateIncomingPublic;
                        modalTitle = 'Входящий групповой звонок';
                    }
                    break;
            }

            callWindows[request.Key] = $('<div id="' + request.Key + '" />').kendoWindow({
                actions: [],
                draggable: false,
                height: "150px",
                modal: true,
                pinned: false,
                position: {
                    top: 100,
                    left: 100
                },
                resizable: false,
                title: modalTitle,
                width: "500px",
                deactivate: function () {
                    this.destroy();
                },
                open: function () {
                    callWindows[request.Key].wrapper.closest(".k-window").css('z-index', 9999999999);
                    $.soundPlay(callingUrl, request.Key, true);
                    faviconAnimateStart();
                },
                close: function() {
                    $.soundStop(request.Key);
                    faviconAnimateStop();

                    if (this.options.canceled && stopCallingUrl)
                            $.soundPlay(stopCallingUrl, null, false);

                }
            }).data("kendoWindow");

            callWindows[request.Key].content(modalTemplate(request));

            callWindows[request.Key].wrapper.find(".btn-cancel-call").click(function (e) {
                clearTimeout(requestTimers[request.Key]);

                callWindows[request.Key].options.canceled = true;
                callWindows[request.Key].close();

                if (request.DialogType === 'PrivateMessage' || request.Self)
                    globalapp.socketService.invoke('CancelCall', request);

                delete callRequests[request.Key];
            });

            callWindows[request.Key].wrapper.find(".btn-success-call, .btn-success-call-oneway, .btn-success-call-audio").click(function (e) {

                config.dontCaptureUserMedia = $(this).hasClass('btn-success-call-oneway');

                chosenSession = $(this).hasClass('btn-success-call-audio') ? config.audioSession : config.videoSession;

                //Stream type switching
                if (config.dontCaptureUserMedia) {
                    $('#video-switch-dialog').removeClass('active checked');
                    $('#audio-switch-dialog').removeClass('active checked');
                } else {
                    $('#video-switch-dialog').addClass('active checked');
                    $('#audio-switch-dialog').addClass('active checked');
                }

                clearTimeout(requestTimers[request.Key]);
                callWindows[request.Key].close();
                delete callRequests[request.Key];

                if (self.rtc && self.rtc.started) {
                    //Поставить на удержание текущий

                }

                globalapp.socketService.invoke('SuccessCall', request);
            });

            callWindows[request.Key].center().open();

            if (request.Self)
                requestTimers[request.Key] = setTimeout(function () {
                    globalapp.socketService.invoke('MissedCall', request);
                    callWindows[request.Key].wrapper.find(".btn-cancel-call").click();
                }, 20000);
        };

        this.sendCallRequest = function (id, type, key, sessionType) {

            config.dontCaptureUserMedia = false;

            switch (sessionType) {
                case 'audio':
                    chosenSession = config.audioSession;
                    break;
                default:
                    chosenSession = config.videoSession;
                    break;
            }

            //$('#video-switch-dialog').addClass('active checked');
            //$('#audio-switch-dialog').addClass('active checked');

            globalapp.socketService.invoke('SendVideoRequest', id, type, key);
        };

        this.addMembersDialog = function() {
            pbaAPI.openModalDialog('User', function (selectedUsers) {
                if (!selectedUsers.length) return;

                if (self.rtc) {
                    var userIds = selectedUsers.map(function (selectedUser) {
                        return selectedUser.ID;
                    });

                    userIds.forEach(function (userId) {
                        self.sendCallRequest(userId, 'PrivateMessage', self.key);
                    });
                }
            }, {
                title: 'Добавить в конференцию',
                multiselect: false,
                zIndex: 99999999999999
            });
        };

        //Not working/using
        //globalapp.socketService.on('OnJoinVideoConference', function (key, request) {
        //    if (currentRequest && isCurrentCall(request)) {
        //        clearTimeout(requestTimers);
        //        callWnd.close();
        //        currentRequest = null;
        //    }

        //    //?
        //    var dialogId = request.DialogType == 'PrivateMessage' ? request.ToId : request.ConferenceId;

        //    self.joinChannel({
        //        key: request.Key,
        //        dialogId: dialogId,
        //        dialogType: request.DialogType,
        //        onstart: function (data) { },
        //        onstop: function (data) { },
        //        onconnect: function (data) { },
        //        ondisconnect: function (data) { }
        //    });
        //});

        globalapp.socketService.on('OnSendVideoRequest', function (request) {
            if (callRequests[request.Key]) {
                return;
            }

            callRequests[request.Key] = request;

            var callSound = request.Self ? '/Content/sounds/out.mp3' : '/Content/sounds/in.mp3';
            var callStopSound = '/Content/sounds/skype_end.mp3';

            showVideoRequest(request, [callSound, callStopSound]);
        });

        globalapp.socketService.on('OnCallSuccess', function (request) {
            if (hasCall(request)) {
                clearTimeout(requestTimers[request.Key]);
                callWindows[request.Key].close();
                delete callRequests[request.Key];
            }

            globalapp.rtcService.openChannel({
                key: request.Key,
                session: chosenSession,
                onstart: function (data) { },
                onstop: function (data) { },
                onconnect: function (data) { },
                ondisconnect: function (data) { },
                onopen: function (data) {
                    globalapp.socketService.invoke('StartVideoConference', request);
                }
            });
        });

        globalapp.socketService.on('OnCallCancel', function (request) {
            if (hasCall(request)) {
                clearTimeout(requestTimers[request.Key]);

                callWindows[request.Key].options.canceled = true;
                callWindows[request.Key].close();

                //$.soundPlay('/Content/sounds/skype_end.mp3', null, false);

                delete callRequests[request.Key];
            }
        });

        globalapp.socketService.on('OnStartVideoConference', function (request) {
            if (hasCall(request)) {
                clearTimeout(requestTimers[request.Key]);
                callWindows[request.Key].close();
                delete callRequests[request.Key];
            }

            globalapp.rtcService.joinChannel({
                key: request.Key,
                session: chosenSession,
                onstart: function (data) { },
                onstop: function (data) { },
                onconnect: function (data) { },
                ondisconnect: function (data) { },
                onopen: function (data) {

                }
            });
        });

        this.rtc = null;
        this.started = false;

        this.keys = {};
        this.key = null;
        this.videoType = videoTypes[0];

        var recorderMain = null;
        var recorderSecondary = null;

        var recordType = 'video';

        this.startRecording = function (type, settings) {
            recordType = type;
            recorderMain = null;
            recorderSecondary = null;

            $('#video-add-dialog').hide();

            var session;

            switch (type) {
                case 'audio':
                    session = config.audioSession;
                    break;
                default:
                    session = config.videoSession;
                    break;
            }

            self.rtc = new RTCMultiConnection(); //RTC multiconnection не нужен

            self.rtc.dontAttachStream = true;

            self.videoType = videoTypes[0];

            config.onstart = settings.onstart;
            config.onstop = settings.onstop;

            self.rtc.onstream = function (data) {
                self.started = true;

                $('#video-wrapper').show();
                $('#video-wrapper').addClass('local-record');

                data.mediaElement.className = "local-video";

                updateLayout(data);

                if (config.onstart)
                    config.onstart(data);
            };

            self.rtc.onstreamended = function (event) {
                self.started = false;

                $('#' + event.streamid).closest('div').remove();

                self.rtc.disconnect();
                self.rtc = null;
            };

            //self.rtc.ondisconnected = function (event) {
            //    console.log('On before clear layout');
            //    self.started = false;
            //    self.rtc = null;
            //};


            self.rtc.captureUserMedia(function (stream) {
                var isFirefox = self.rtc.UA.isFirefox;

                if (isFirefox) {
                    recorderMain = RecordRTC(stream);
                } else {
                    recorderMain = RecordRTC(stream, {
                        type: 'audio'
                    });

                    if (recordType === 'video') {
                        recorderSecondary = RecordRTC(stream, {
                            type: 'video',
                            video: {
                                width: 1024,
                                height: 768
                            },
                            canvas: {
                                width: 1024,
                                height: 768
                            }
                        });
                    }
                }

                recorderMain.initRecorder(function () {
                    if (isFirefox || recordType === 'audio') {
                        recorderMain.dontFireOnDataAvailableEvent = false;
                        recorderMain.startRecording();
                    } else {
                        recorderSecondary.initRecorder(function () {
                            recorderMain.dontFireOnDataAvailableEvent = false;
                            recorderMain.startRecording();
                            setTimeout(function() {
                                recorderSecondary.dontFireOnDataAvailableEvent = false;
                                recorderSecondary.startRecording();
                            }, 100);
                        });
                    }
                });

            }, session);

        };

        var prepareBlobs = function (blobs) {
            recorderMain = null;
            recorderSecondary = null;

            var data = self.rtc.streams.selectFirst({ local: true });

            if (data)
                data.stop();

            if (config.onstop)
                config.onstop(blobs);
        };

        this.stopRecording = function () {
            $('#video-wrapper').hide();
            $('#video-wrapper').removeClass('local-record');


            var isFirefox = self.rtc.UA.isFirefox;

            recorderMain.stopRecording(function () {
                var mainBlob = recorderMain.getBlob();

                if (recordType === 'video' && !isFirefox && recorderSecondary)
                    recorderSecondary.stopRecording(function () {
                        var secondaryBlob = recorderSecondary.getBlob();
                        prepareBlobs([mainBlob, secondaryBlob]);
                    });
                else
                    prepareBlobs([mainBlob]);
            });
        };

        this.stopRTC = function () {
            self.started = false;

            if (self.rtc) {

                self.rtc.dontCaptureUserMedia = true;
                self.rtc.dontAttachStream = true;

                if (self.videoType === videoTypes[0]) {
                    self.stopRecording();
                } else {
                    self.rtc.leave(null, function () {
                        clearLayout(self.rtc, true);

                        self.rtc.disconnect();
                    });
                }
            }
        };

        var init = function(settings) {
            config.streams = {};
            config.peerConfigs = {};

            self.key = settings.key;
            self.videoType = videoTypes[1];

            config.channel = 'channel_' + settings.key;
            config.sessionid = 'session_' + settings.key;
            config.userid = 'user_' + settings.key;

            config.onstart = settings.onstart;
            config.onstop = settings.onstop;

            config.onconnect = settings.onconnect;
            config.ondisconnect = settings.ondisconnect;

            config.onopen = settings.onopen;

            var onMessageCallbacks = {};

            globalapp.socketService.on('onMessageReceived', function(message) {
                message = pbaAPI.json.parse(message);

                if (message && onMessageCallbacks[message.channel]) {
                    onMessageCallbacks[message.channel](message.message);
                }
            });

            self.rtc = new RTCMultiConnection(config.channel);
            self.rtc.log = false;
            self.rtc.firebase = false;

            self.rtc.dontCaptureUserMedia = config.dontCaptureUserMedia;
            self.rtc.dontAttachStream = false;


            self.rtc.sdpConstraints.mandatory = {
                OfferToReceiveAudio: true,
                OfferToReceiveVideo: true
            };

            if (self.rtc.UA.isFirefox) {
                console.log("Firefox mandatory...");
                self.rtc.sdpConstraints.mandatory = {
                    offerToReceiveAudio: true,
                    offerToReceiveVideo: true
                };
            }

            self.rtc.iceProtocols = {
                tcp: true, // prefer using TCP-candidates
                udp: true  // prefer using UDP-candidates
            };

            //self.rtc.mediaConstraints.mandatory = {
            //    minWidth: 1280,
            //    maxWidth: 1280,
            //    minHeight: 720,
            //    maxHeight: 720,
            //    minFrameRate: 30
            //};

            self.rtc.mediaConstraints = {
                "audio": true,
                "video": {
                    "width": {
                        "min": "200",
                        "max": "200"
                    },
                    "height": {
                        "min": "150",
                        "max": "150"
                    },
                    "frameRate": {
                        "min": "15"
                    }
                }
            };


            self.rtc.getExternalIceServers = false;
            self.rtc.iceServers = config.iceServers;

            self.rtc.onMediaError = function(event) {
                //console.log('Media error:', event);
            };

            self.rtc.openSignalingChannel = function (conf) {
                console.log('Try to open signal...');
                if (!self.rtc || !self.started) return false;

                var channel = conf.channel || this.channel;
                onMessageCallbacks[channel] = conf.onmessage;

                if (conf.onopen) setTimeout(conf.onopen, 1000);

                return {
                    send: function (message) {
                        message = pbaAPI.json.stringify({
                            message: message,
                            channel: channel,
                            chosenSession: chosenSession
                        });

                        if (message) {
                            globalapp.socketService.invoke('WebRtcSend', message);
                        }
                    }
                };
            };

            self.rtc.sendMessage = function(message) {
                if (self.rtc) {
                    message.userid = self.rtc.userid;
                    message.chosenSession = self.rtc.chosenSession;
                    message.extra = self.rtc.extra;
                    self.rtc.sendCustomMessage(message);
                }
            };

            self.rtc.onCustomMessage = function (message) {
                if (message.switchstream && message.userid !== self.rtc.userid) {
                    if (config.peerConfigs[message.userid]) {
                        config.peerConfigs[message.userid].video = !config.peerConfigs[message.userid].video;
                        switchMediaType(config.peerConfigs[message.userid].video, message.userid);
                    }
                }

                if (message.refreshstream) {
                    if (self.rtc) {
                        if (!self.rtc.streams[message.streamid]) {

                            console.log('Some user (' + message.userid + ') have stream i dont\'t, ask him to renegotiate');

                            if (self.rtc.askToShareParticipants) {
                                console.log('One more asking to share...');
                                self.rtc.askToShareParticipants();
                            } else {
                                self.rtc.sendMessage({
                                    renegotiateStream: true,
                                    requestorId: message.userid,
                                    streamid: message.streamid
                                });
                            }

                        }
                    }
                }

                if (message.removeStream) {
                    if (self.rtc.streams[message.streamid]) {
                        self.rtc.streams[message.streamid].stop();

                        delete self.rtc.streams[message.streamid];

                        if (config.streams[message.streamid])
                            delete config.streams[message.streamid];

                    }

                    clearLayout(self.rtc, true);
                }

                if (message.renegotiateStream && message.requestorId === self.rtc.userid) {
                    console.log('Try renegotiate stream to user:', message.userid);
                    if (self.rtc.streams[message.streamid]) {
                        console.log('I have this stream');
                        if (self.rtc.peers[message.userid]) {
                            console.log('I have this user, renegotiate...');
                            self.rtc.peers[message.userid].renegotiate(self.rtc.streams[message.streamid].stream, self.rtc.streams[message.streamid].session);
                        } else {
                            console.log('I haven\'t this user', message.userid);
                        }
                    }
                }
            };

            self.rtc.keepStreamsOpened = config.keepStreamsOpened;

            //Стрим начался
            self.rtc.onstream = function (event) {
                console.info('On stream:', event);

                //When user in firefox doesn't access web cam (this is not help)
                if (event.streamid === undefined) {
                    return false;
                }

                var fakeAudio = (event.type === 'local' && !chosenSession.video) || (event.type === 'remote' && config.peerConfigs[event.userid] && !config.peerConfigs[event.userid].video);

                event.mediaElement.className = event.type + '-video' + (fakeAudio ? ' fakeaudio': '');
                event.mediaElement.setAttribute("data-media-userid", event.userid);


                if (event.type === 'local') {
                    self.rtc.sendMessage({
                        refreshstream: true,
                        streamid: event.streamid
                    });

                    updateLayout(event);
                    self.rtc.dontCaptureUserMedia = true;
                    config.streams[event.streamid] = event;
                } else {
                    if (config.streams[event.streamid]) {
                        console.log('Remote stream already exists...');
                    } else {
                        config.streams[event.streamid] = event;
                        updateLayout(event);
                    }
                }

                $('#video-wrapper').show();

                if (config.onstart)
                    config.onstart({
                        rtc: self.rtc,
                        event: event
                    });
            };

            //Стрим был завершен
            self.rtc.onstreamended = function (event) {
                delete config.streams[event.streamid];

                $('#' + event.streamid).closest('div').remove();
                updateLayout();

                if (config.onstop)
                    config.onstop({
                        rtc: self.rtc,
                        event: event
                    });
            };

            //Когда я вызвал disconnect
            self.rtc.ondisconnected = function (event) {
                //console.log('On disconnect');

                // CHANGE USER STATUS
                globalapp.userStateService.clearCallStatus();

                clearLayout(self.rtc, true);

                self.started = false;
                self.rtc = null;

                $.soundPlay('/Content/sounds/skype_end.mp3', null, false);

                if (config.ondisconnect)
                    config.ondisconnect(event);
            };


            //Когда кто-то зашел
            self.rtc.onconnected = function (event) {

                if (config.onconnect)
                    config.onconnect({
                        rtc: self.rtc,
                        event: event
                    });
            };

            self.rtc.onNewSession = function (session) {
                console.log('New session:', session);
            };


            //Костыль, когда пользователь слишком долго не дает доступ к устройствам, то в следствии его не слышно и не видно и ему тоже
            var establish = setInterval(function () {
                if (self.rtc && self.rtc.askToShareParticipants) {
                    console.warn('Ask to share partipiants...');

                    self.rtc.askToShareParticipants();
                    clearInterval(establish);
                    establish = 0;
                }
            }, 500);


            //Срабатывает когда кто-то вышел
            self.rtc.onleave = function (event) {
                updateSession(event.userid);
            };

            self.rtc.onclose = function (event) {
                //console.log('Default close event:', event);
            };

            //Срабатывает непонятно вобще когда
            self.rtc.onopen = function (event) {
                //console.log('Default open event:', event);
            };

            //TODO: Need test
            self.rtc.onstatechange = function (event) {

                if (!self.rtc || !self.started) return;

                if (self.rtc.isInitiator) {
                    if (event.name === 'usermedia-fetched' && self.needsignal) {
                        self.needsignal = false;
                        if (config.onopen)
                            config.onopen({
                                rtc: self.rtc,
                                event: event
                            });
                    }
                } else {
                    if (event.name === 'connecting-with-initiator') {

                        if (config.onopen)
                            config.onopen({
                                rtc: self.rtc,
                                event: event
                            });
                    }

                    if (event.name === 'connected-with-initiator') {
                        self.rtc.localStreamids.forEach(function (streamid) {
                            self.rtc.sendMessage({
                                refreshstream: true,
                                streamid: streamid
                            });
                        });

                    }
                }

            };

            self.rtc.customPeerAdd = function (cfg) {
                console.log('New peer connected:', cfg);
                config.peerConfigs[cfg.userid] = cfg.chosenSession;
            };


            self.rtc.processSdp = function(sdp) {
                //console.log('SDP:', sdp);
                return sdp;
            };

        };

        var updateSession = function (userid) {
            if (self.rtc) {
                var remoteKey = updatePeers(userid);

                clearLayout(self.rtc, true);

                if (!remoteKey) {
                    globalapp.socketService.invoke('EndVideoConference', self.key);
                    self.rtc.disconnect();
                }
            }
        };

        var updatePeers = function(userid) {
            var remoteKey = false;
            if (self.rtc) {

                self.rtc.remove(userid);

                for (var peerid in self.rtc.peers) {
                    //console.log('On update peer:', self.rtc.peers[peer]);

                    var peer = self.rtc.peers[peerid];

                    if (peer && peer.userid && peer.userid !== userid) {
                        remoteKey = true;
                        break;
                    }
                }

            }
            return remoteKey;
        };

        var clearLayout = function (rtc, withupdate) {
            if (rtc) {
                if ($('.conf-video-player-wrapper video, .conf-video-player-wrapper audio, .conf-video-player-wrapper object').length) {
                    $('.conf-video-player-wrapper video, .conf-video-player-wrapper audio, .conf-video-player-wrapper object').each(function() {
                        //console.log('clear: ', this);
                        var $el = $(this);
                        if (!self.rtc.streams || !self.rtc.streams[$el.attr('id')]) {
                            $el.closest('div').remove();
                        }
                    });
                }
            } else {
                //$('.conf-video-player-wrapper video, .conf-video-player-wrapper audio, .conf-video-player-wrapper object').remove();
                $('.local-media-wrapper, .remote-media-wrapper').remove();
            }

            if (withupdate)
                updateLayout();

        };



        var updateLayout = function(event) {
            if (event && event.mediaElement) {

                event.mediaElement.removeAttribute('controls');

                var $wrap = $('<div/>');

                if(self.videoType === videoTypes[1]) {
                    $wrap.append('<div class="media-controls"></div>');

                    var session = event.rtcMultiConnection.chosenSession;

                    var eye = !session.video ? 'mdi mdi-eye' : 'mdi mdi-eye-off';

                    if (event.type === 'local')
                        $wrap.find('.media-controls').append('<button data-btn="switch-media" data-value="' + session.video + '" class="k-button"><i class="' + eye + '"></i></button>');
                }

                $wrap.addClass(event.type + '-media-wrapper');

                if (event.mediaElement.className.indexOf('fakeaudio') !== -1)
                    $wrap.addClass('fakeaudio-media-wrapper');

                $wrap.append(event.mediaElement);

                $('.conf-video-player-wrapper').append($wrap);

                var interval = setInterval(function() {
                    var $media = $('#' + event.streamid);
                    if ($media.length) {
                        $media[0].play();
                        clearInterval(interval);
                    }
                }, 100);
            }

            setTimeout(function () {
                if (!$('.conf-video-player-wrapper video, .conf-video-player-wrapper audio, .conf-video-player-wrapper object').length) {
                    if (!self.rtc) {
                        $('.conf-video-player-wrapper').hide();
                    }
                    return;
                }

                var attr = 'data-video-layout';

                var videoWrapper = $('.conf-video-player-wrapper');
                var locals = videoWrapper.children('.local-media-wrapper');
                var remotes = videoWrapper.children('.remote-media-wrapper');
                var hasLocal = locals.length > 0;
                var hasRemote = remotes.length > 0;

                var currentLayout = videoWrapper.attr(attr);
                var nextLayout = null;

                if (locals.length + remotes.length === 0) {
                    nextLayout = 'no-video';
                } else if (locals.length + remotes.length === 1) {
                    nextLayout = 'one-video';
                } else if (remotes.length === 1) {
                    nextLayout = 'private-call';
                } else {
                    nextLayout = 'conference-call';
                }

                if (currentLayout !== nextLayout) {
                    videoWrapper.attr(attr, nextLayout);
                }
            }, 0);
        };

        var start = function (session, callback) {
            self.needsignal = true;

            self.rtc.chosenSession = session ? session : config.videoSession;

            self.rtc.direction = config.direction;
            self.rtc.session = {
                audio: true,
                video: true
            };
            self.rtc.userid = config.userid;
            self.rtc.extra = config.extra;

            self.rtc.open({
                dontTransmit: config.dontTransmit,
                sessionid: config.sessionid
            });

            if (callback)
                callback();
        };

        var join = function (session, callback) {
            self.needsignal = false;

            self.rtc.chosenSession = session ? session : config.videoSession;

            self.rtc.direction = config.direction;

            self.rtc.join({
                sessionid: config.sessionid,
                userid: config.userid,
                extra: config.extra,
                session: {
                    audio: true,
                    video: true
                }
            });

            if (callback)
                callback();
        };

        this.openChannel = function (settings) {

            // CHANGE USER STATUS
            globalapp.userStateService.setCallStatus();

            $('#video-add-dialog').show(); //only for initiator

            if (!self.started) {
                self.started = true;

                init(settings);

                start(settings.session, function () {

                });
            } else {
                if (settings.onopen)
                    settings.onopen();
            }
        };

        this.joinChannel = function (settings) {

            // CHANGE USER STATUS
            globalapp.userStateService.setCallStatus();

            $('#video-add-dialog').hide(); //only for initiator

            if (!self.started) {
                self.started = true;

                init(settings);

                join(settings.session, function () {

                });
            } else {
                if (settings.onopen)
                    settings.onopen();
            }

        };

        this.switchVideo = function(state) {
            if (self.rtc && self.started) {
                var streams = self.rtc.streams.selectAll({ local: true, video: true });
                if (streams && streams.length && !state) {
                    streams.forEach(function(stream) {
                        //console.log('stream:', stream);

                        self.rtc.streams[stream.streamid].stop();
                        //self.rtc.removeStream(stream.streamid, true);

                        self.rtc.sendMessage({
                            removeStream: true,
                            streamid: stream.streamid
                        });

                        delete self.rtc.streams[stream.streamid];

                        if (config.streams[stream.streamid])
                            delete config.streams[stream.streamid];

                        self.rtc.dontCaptureUserMedia = config.dontCaptureUserMedia = !self.rtc.streams.selectAll({ local: true }).length;
                        clearLayout(self.rtc, true);
                    });
                } else {
                    self.rtc.dontCaptureUserMedia = config.dontCaptureUserMedia = false;
                    self.currentSession = config.videoSession;
                    self.rtc.addStream(config.videoSession);
                    if (self.rtc.askToShareParticipants)
                        self.rtc.askToShareParticipants();
                }
            }
        };

        // !!!!!!!!!!!!!!!!!!!!!!
        // id   - codec
        //
        // 0    - PCMU/8000 work
        // 8    - PCMA/8000 work
        // 9    - G722/8000 not work
        // 103  - ISAC/16000 not work
        // 104  - ISAC/32000 not work
        // 111  - opus/48000 work
        // 126  - telephone-event/8000 not work
        var SDP = {
            setAudioCodec: function(sdp, codecId) {
                var rMAudio = /m=audio (\d+) ([a-zA-Z0-9\/]+)/;

                var lines = sdp.split('\r\n');

                var audioSectionIndex = -1;
                var videoSectionIndex = -1;

                for (var i = 0; i < lines.length; i++) {
                    if (audioSectionIndex >= 0 && videoSectionIndex >= 0) {
                        break;
                    }

                    if (audioSectionIndex < 0 && lines[i][0] === 'm' && lines[i].indexOf('m=audio') === 0) {
                        audioSectionIndex = i;
                        continue;
                    }

                    if (videoSectionIndex < 0 && lines[i][0] === 'm' && lines[i].indexOf('m=video') === 0) {
                        videoSectionIndex = i;
                        continue;
                    }
                }

                var audioSection = lines.slice(audioSectionIndex, videoSectionIndex);

                var reMAudio = audioSection[0].match(rMAudio);

                var port = reMAudio[1];
                var proto = reMAudio[2];

                var toRemove = audioSection[0]
                    .slice(reMAudio[0].length)
                    .trim()
                    .split(' ')
                    .filter(function(def) {
                        return def != codecId;
                    });

                toRemove.forEach(function(def) {
                    audioSection = audioSection.filter(function(line) {
                        return line.indexOf(':' + def + ' ') < 0;
                    });
                });

                audioSection[0] = 'm=audio ' + port + ' ' + proto + ' ' + codecId;

                if (audioSection.every(function(line) {
                    return line.indexOf('PCMU') === -1;
                })) {
                    audioSection.push('a=rtpmap:0 PCMU/8000');
                }

                return lines
                    .slice(0, audioSectionIndex)
                    .concat(audioSection)
                    .concat(lines.slice(videoSectionIndex))
                    .join('\r\n');
            },
            log: function(sdp) {
                console.log({ sdp: sdp.split('\r\n') });
            }
        };

        this.fixSdp = function(sdpString) {
            console.info('BEFORE SDP FIX');
            SDP.log(sdpString);

            var fixed = SDP.setAudioCodec(sdpString, 0);

            console.info('AFTER SDP FIX');
            SDP.log(fixed);

            return fixed;
        };
        // !!!!!!!!!!!!!!!!!!!!!!

        $('#video-wrapper').on('click', '.btn[data-btn="switch-media"]', function () {
            var $btn = $(this);

            if (!self.rtc) return;

            self.rtc.chosenSession.video = !self.rtc.chosenSession.video;

            if (self.rtc.chosenSession.video)
                $btn.find('i').switchClass('mdi-eye', 'mdi-eye-off');
            else
                $btn.find('i').switchClass('mdi-eye-off', 'mdi-eye');

            $btn.attr('data-value', self.rtc.chosenSession.video);

            switchMediaType(self.rtc.chosenSession.video, self.rtc.userid);

            self.rtc.sendMessage({
                switchstream: true
            });
        });

        var switchMediaType = function (value, userid) {
            console.log('Try to switch:', value, userid);

            var $media = $('[data-media-userid="' + userid + '"]');

            if ($media.length) {
                var $wrap = $media.closest('div');

                if (value) {
                    $wrap.removeClass('fakeaudio-media-wrapper');
                    $wrap.find('video').removeClass('fakeaudio');
                } else {
                    $wrap.addClass('fakeaudio-media-wrapper');
                    $wrap.find('video').addClass('fakeaudio');
                }
            }

        };

    };

    globalapp.rtcService = new service();

}());
