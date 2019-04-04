/* globals console, $, pbaAPI */
(function () {
    'use strict';

    // #######
    // PRIVATE
    // #######

    var kendoNotification = null;
    var kendoConfirmWindow = null;
    var kendoAlertWindow = null;

    /**
     * Выводит сообщение указанного типа.
     * @param  {string} text                    Текст сообщения.
     * @param  {string} type                    Тип сообщения (проще использовать шорткат-методы ниже).
     * @param  {number} [autoHideAfter=2000]    Время в миллисекундах, после которого сообщение исчезнет.
     */
    pbaAPI.msg = function (text, type, autoHideAfter) {
        if (type === 'error') {
            kendoNotification.options.autoHideAfter = autoHideAfter || 6000;
            console.error('error: ' + text);
            if (console.trace) {
                console.trace('call stack');
            }
        } else {
            kendoNotification.options.autoHideAfter = autoHideAfter || 2000;
        }

        kendoNotification.show({ message: text }, type);
    };

    /**
     * Выводит сообщение об ошибке.
     * @param  {string} text          Текст сообщения.
     * @param  {number} autoHideAfter Время в миллисекундах, после которого сообщение исчезнет.
     */
    pbaAPI.errorMsg = function (text, autoHideAfter) {
        //sib
        //все ошибки показываем в модальном окне.
        //pbaAPI.msg(text, 'error', autoHideAfter);
        pbaAPI.alertError(text);
        //end sib
    };

    /**
     * Если debug выводит сообщение об ошибке.
     * @param  {string} text          Текст сообщения.
     * @param  {number} autoHideAfter Время в миллисекундах, после которого сообщение исчезнет.
     */
    pbaAPI.debugError = function (text, autoHideAfter) {
        if (application.isDebug)
            pbaAPI.msg(text, 'error', autoHideAfter);
    };


    /**
     * Выводит уведомляющее сообщение.
     * @param  {string} text          Текст сообщения.
     * @param  {number} autoHideAfter Время в миллисекундах, после которого сообщение исчезнет.
     */
    pbaAPI.infoMsg = function (text, autoHideAfter) {
        pbaAPI.msg(text, 'info', autoHideAfter);
    };

    /**
     * Выводит сообщение об успешной загрузке.
     * @param  {string} text          Текст сообщения.
     * @param  {number} autoHideAfter Время в миллисекундах, после которого сообщение исчезнет.
     */
    pbaAPI.uploadMsg = function (text, autoHideAfter) {
        pbaAPI.msg(text, 'upload-success', autoHideAfter);
    };

    /**
     * Выводит сообщение о новом сообщении в чат.
     * @param  {string} text          Текст сообщения.
     * @param  {number} autoHideAfter Время в миллисекундах, после которого сообщение исчезнет.
     */
    pbaAPI.chatMsg = function (text, autoHideAfter) {
        pbaAPI.msg(text, 'chat', autoHideAfter);
    };

    /**
     * Выводит диалоговое модальное окно с вопросом и двумя кнопками: да/нет.
     * @param  {string}     title           Заголовок окна.
     * @param  {string}     text            Сообщение/вопрос пользователю.
     * @param  {function}   callbackYes     Колбэк отработает, если пользователь нажмет "ДА".
     * @param  {function}   callbackNo      Колбэк отработает, если пользователь нажмет "НЕТ".
     */
    pbaAPI.confirm = function (title, text, callbackYes, callbackNo) {
        pbaAPI.confirmEx({
            title: title,
            text: text,
            callbackYes: callbackYes,
            callbackNo: callbackNo
        });
    };

    /**
     * Выводит диалоговое модальное окно с сообщением об ошибке и кнопкой: ОК.
     * @param  {string}     text            Сообщение пользователю.
     * @param  {function}   callbackYes     Колбэк отработает, если пользователь нажмет "ОК".
     */
    pbaAPI.alertError = function (text, callbackYes) {
        pbaAPI.alertEx({
            title: "Ошибка",
            text: text,
            type: "error",
            callbackYes: callbackYes,
        });
    };

    /**
     * Выводит диалоговое модальное окно с сообщением об успешном завершении операции и кнопкой: ОК.
     * @param  {string}     text            Сообщение пользователю.
     * @param  {function}   callbackYes     Колбэк отработает, если пользователь нажмет "ОК".
     */
    pbaAPI.alertSuccess = function (text, callbackYes) {
        pbaAPI.alertEx({
            title: "Завершено",
            text: text,
            type: "success",
            callbackYes: callbackYes,
        });
    };

    /**
     * Выводит диалоговое модальное окно с сообщением о завершении операции c предупреждением и кнопкой: ОК.
     * @param  {string}     text            Сообщение пользователю.
     * @param  {function}   callbackYes     Колбэк отработает, если пользователь нажмет "ОК".
     */
    pbaAPI.alertWarning = function (text, callbackYes) {
        pbaAPI.alertEx({
            title: "Внимание",
            text: text,
            type: "warning",
            callbackYes: callbackYes,
        });
    };

    /**
     * Выводит диалоговое модальное окно с сообщением и кнопкой: ОК.
     * @param  {string}     title           Заголовок окна.
     * @param  {string}     text            Сообщение пользователю.
     * @param  {string}     type            Тип сообщения (проще использовать шорткат-методы выше).
     * @param  {function}   callbackYes     Колбэк отработает, если пользователь нажмет "ОК".
     */
    pbaAPI.alert = function (title, text, type, callbackYes) {
        pbaAPI.alertEx({
            title: title,
            text: text,
            type: type,
            callbackYes: callbackYes,
        });
    };

    pbaAPI.confirmEx = function (options) {
        var params = $.extend({
            title: "Подтверждение",
            text: "Неопределенное подтверждение",
            titleYes: "Да",
            titleNo: "Нет",
            callbackYes: function () { },
            callbackNo: function () { }
        }, options);

        //если получаем неопределенный запрос на подтверждение - логгируем колл-стек
        if (params.text === "Неопределенное подтверждение") {
            console.error('error: ' + params.text);
            if (console.trace) {
                console.trace('call stack');
            }
            //логгируем колл-стек когда ошибка не определена

            try {
                throw new Error(params.text);
            }
            catch (e) {
                var callstack = e.stack.toString();
                pbaAPI.proxyclient.EUSI.logUndefinedError(null, callstack);
            }

        }

        kendoConfirmWindow.title(pbaAPI.truncate(params.title, 25));
        kendoConfirmWindow.titleYes(params.titleYes || "Да");
        kendoConfirmWindow.titleNo(params.titleNo || "Нет");
        kendoConfirmWindow.text(params.text);
        kendoConfirmWindow.yes(params.callbackYes);
        kendoConfirmWindow.no(params.callbackNo);
        kendoConfirmWindow.center();
        kendoConfirmWindow.open();
    };

    pbaAPI.alertEx = function (options) {
        var params = $.extend({
            title: "Ошибка",
            text: "Неизвестная ошибка",
            titleYes: "Ок",
            type: "error",
            callbackYes: function () { }
        }, options);

        if (params.type === "error") {
            kendoAlertWindow.element.parent().find(".k-window-titlebar").css("background-color", "#d32f2f");

            console.error('error: ' + params.text);
            if (console.trace) {
                console.trace('call stack');
            }
            //логгируем колл-стек когда ошибка не определена
            if (params.text === "Неизвестная ошибка") {
                try {
                    throw new Error(params.text);
                }
                catch (e) {
                    var callstack = e.stack.toString();
                    pbaAPI.proxyclient.EUSI.logUndefinedError(null, callstack);
                }
            }
        }
        else if (params.type === "success")
            kendoAlertWindow.element.parent().find(".k-window-titlebar").css("background-color", "#4caf50");
        else if (params.type === "warning")
            kendoAlertWindow.element.parent().find(".k-window-titlebar").css("background-color", "#dcb112");
        else
            kendoAlertWindow.element.parent().find(".k-window-titlebar").css("background-color", "");

        kendoAlertWindow.title(pbaAPI.truncate(params.title, 25));
        kendoAlertWindow.titleYes(params.titleYes || "Ok");
        kendoAlertWindow.text(params.text);
        kendoAlertWindow.yes(params.callbackYes);
        kendoAlertWindow.center();
        kendoAlertWindow.open();
    };

    function getKendoNotification() {
        var id = pbaAPI.guid('notification');

        var $notification = $('<div id="' + id + '">').appendTo('body');

        return $notification.kendoNotification({
            stacking: 'down',
            position: {
                pinned: true,
                top: 5,
                right: 5,
            },
            templates: [
                {
                    type: 'info',
                    template: '<div class="notification info">' +
                    '<p>#= message #</p>' +
                    '</div>'
                },
                {
                    type: 'error',
                    template: '<div class="notification error">' +
                    '<p>#= message #</p>' +
                    '</div>'
                },
                {
                    type: 'upload-success',
                    template: '<div class="notification upload-success">' +
                    '<p>#= message #</p>' +
                    '</div>'
                },
                {
                    type: 'chat',
                    template: '<div class="notification new-chat-msg">' +
                    '<p>#= message #</p>' +
                    '</div>'
                },
            ],
            show: function (e) {
                e.element.closest('.k-animation-container').css('z-index', '999999');
            }
        }).data('kendoNotification');
    }

    function getKendoConfirmWindow() {
        var id = pbaAPI.guid('confirm');

        var html = '<div id="' + id + '" class="k-popup-edit-form k-window-content k-content">' +
            '<div class="k-edit-form-container">' +
            '<p class="k-popup-message"></p>' +
            '<div class="k-edit-buttons">' +
            '<a class="k-button btn-yes success" href="#"></a>' +
            '<a class="k-button btn-no secondary" href="#"></a>' +
            '</div>' +
            '</div>' +
            '</div>';

        var $wnd = $(html).appendTo('body');

        var wnd = $wnd.kendoWindow({
            width: 400,
            modal: true,
            visible: false,
        }).data('kendoWindow');

        wnd._callback_yes = null;
        wnd._callback_no = null;

        wnd.yes = function (callback) {
            wnd._callback_yes = callback;
        };

        wnd.no = function (callback) {
            wnd._callback_no = callback;
        };

        wnd.text = function (text) {
            wnd.element.find('p.k-popup-message').html(text || "");
        };

        var btnYes = $wnd.find(".btn-yes");

        wnd.titleYes = function (title) {
            btnYes.html(title);
        };

        btnYes.on("click", function () {
            wnd.close();
            if (wnd._callback_yes)
                wnd._callback_yes();
        });

        var btnNo = $wnd.find(".btn-no");

        wnd.titleNo = function (title) {
            btnNo.html(title);
        };

        btnNo.on("click", function () {
            wnd.close();
            if (wnd._callback_no)
                wnd._callback_no();
        });

        return wnd;
    }

    function getKendoAlertWindow() {
        var id = pbaAPI.guid('alert');

        var html = '<div id="' + id + '" class="k-popup-edit-form k-window-content k-content">' +
            '<div class="k-edit-form-container">' +
            '<p class="k-popup-message"></p>' +
            '<div class="k-edit-buttons">' +
            '<a class="k-button btn-yes success" href="#"></a>' +
            '</div>' +
            '</div>' +
            '</div>';

        var $wnd = $(html).appendTo('body');

        var wnd = $wnd.kendoWindow({
            width: 420,
            maxHeight: 500,
            modal: true,
            visible: false,
        }).data('kendoWindow');

        wnd._callback_yes = null;
        wnd._callback_no = null;

        wnd.yes = function (callback) {
            wnd._callback_yes = callback;
        };

        wnd.no = function (callback) {
            wnd._callback_no = callback;
        };

        wnd.text = function (text) {
            wnd.element.find('p.k-popup-message').html(text || "");
        };

        var btnYes = $wnd.find(".btn-yes");

        wnd.titleYes = function (title) {
            btnYes.html(title);
        };

        btnYes.on("click", function () {
            wnd.close();
            if (wnd._callback_yes)
                wnd._callback_yes();
        });

        return wnd;
    }

    $(function () {
        kendoNotification = getKendoNotification();
        kendoConfirmWindow = getKendoConfirmWindow();
        kendoAlertWindow = getKendoAlertWindow();
    });

}());
