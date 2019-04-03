/* globals $, globalapp */
(function() {
    'use strict';

    // ############
    // DEPENDENCIES
    // ############

    var userStateService = globalapp.userStateService;

    // #######
    // HELPERS
    // #######

    var CustomStatus = {
        ready: {
            icon: 'mdi mdi-checkbox-marked-circle-outline',
            desc: 'Готов к контакту'
        },
        away: {
            icon: 'mdi mdi-alarm',
            desc: 'Отсутствует'
        },
        dontDisturb: {
            icon: 'mdi mdi-alert-circle-outline',
            desc: 'Не беспокоить'
        },
        disconnected: {
            icon: 'mdi mdi-power',
            desc: 'Отключен'
        },

        // system statuses
        inConversation: {
            icon: 'mdi mdi-phone-in-talk',
            desc: 'Ведет разговор'
        }
    };

    var updateIcon = function(element, userStatus) {
        if (!element) return;

        // PARSE BASE CLASSES
        var classes = element.className.match(/user-state-icon(--\w+)?/g) || [];

        // FALLBACK TO DEFAULT CLASSNAME
        if (!classes.length) {
            classes.push('user-state-icon');
        }

        // ADD ICON-SPECIFIC CLASS
        classes.push(!userStatus.isOnline ? CustomStatus.disconnected.icon : CustomStatus[userStatus.status].icon);

        // APPLY THAT CLASS TO THE ICON ELEMENT
        element.className = classes.join(' ');
    };

    var updateDescription = function(element, userStatus) {
        if (!element) return;

        // REPLACE TEXT CONTENT TO RELEVANT ONE
        element.textContent = CustomStatus[userStatus.status].desc;
    };

    var applyStatus = function(element, userStatus) {
        if (!element) return;

        // UPDATE ATTRIBUTE VALUES
        element.setAttribute('data-user-state-online', userStatus.isOnline === true);
        element.setAttribute('data-user-state-status', userStatus.isOnline ? userStatus.status : 'disconnected');

        // UPDATE INNER ICON ELEMENT
        updateIcon(element.querySelector('.user-state-icon'), userStatus);

        // UPDATE INNER STATUS DESCRIPTION
        updateDescription(element.querySelector('.user-state-desc'), userStatus);
    };

    //var updateTooltips = function() {
    //    $('.user-image [data-user-state-status]').each(function() {
    //        var status = this.getAttribute('data-user-state-status');
    //        if (!CustomStatus[status]) return;

    //        $(this).attr('title', CustomStatus[status].desc);
    //    });
    //};

    // ###########
    // MAIN MODULE
    // ###########

    var binder = {
        getIds: function() {
            var elements = document.querySelectorAll('[data-user-state-id]') || [];

            return Array.prototype.map.call(elements, function(element) {
                return element.getAttribute('data-user-state-id');
            });
        },
        applyState: function(userStatuses) {
            userStatuses.forEach(function(userStatus) {
                var elements = document.querySelectorAll('[data-user-state-id="' + userStatus.id + '"]') || [];

                Array.prototype.forEach.call(elements, function(element) {
                    applyStatus(element, userStatus);
                    //updateTooltips();
                });
            });
        },
    };

    // #######
    // BINDING
    // #######

    userStateService.addBinder(binder);

}());
