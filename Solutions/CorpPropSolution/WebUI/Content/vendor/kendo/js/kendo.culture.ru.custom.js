/*
* Kendo UI Complete v2012.3.1315 (http://kendoui.com)
* Copyright 2013 Telerik AD. All rights reserved.
*
* Kendo UI Complete commercial licenses may be obtained at
* https://www.kendoui.com/purchase/license-agreement/kendo-ui-complete-commercial.aspx
* If you do not own a commercial license, this file shall be governed by the trial license terms.
*/
(function () {
    kendo.cultures.ru = {
        name: "ru",
        numberFormat: { pattern: ["-n"], decimals: 3, ",": " ", ".": ",", groupSize: [3], percent: { pattern: ["-n%", "n%"], decimals: 3, ",": " ", ".": ",", groupSize: [3], symbol: "%" }, currency: { pattern: ["-n$", "n$"], decimals: 2, ",": " ", ".": ",", groupSize: [3], symbol: "р." } }, calendars: { standard: { days: { names: ["воскресенье", "понедельник", "вторник", "среда", "четверг", "пятница", "суббота"], namesAbbr: ["Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб"], namesShort: ["Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб"] }, months: { names: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь", ""], namesAbbr: ["янв", "фев", "мар", "апр", "май", "июн", "июл", "авг", "сен", "окт", "ноя", "дек", ""] }, AM: [""], PM: [""], patterns: { d: "dd.MM.yyyy", D: "d MMMM yyyy 'г.'", F: "d MMMM yyyy 'г.' H:mm:ss", g: "dd.MM.yyyy H:mm", G: "dd.MM.yyyy H:mm:ss", m: "MMMM dd", M: "MMMM dd", s: "yyyy'-'MM'-'dd'T'HH':'mm':'ss", t: "H:mm", T: "H:mm:ss", u: "yyyy'-'MM'-'dd HH':'mm':'ss'Z'", y: "MMMM yyyy", Y: "MMMM yyyy" }, "/": ".", ":": ":", firstDay: 1 } }
    }
})(this);

kendo.culture("ru");

kendo.ui.ColumnMenu.prototype.options.messages =
    $.extend(kendo.ui.ColumnMenu.prototype.options.messages, {
        sortAscending: "По возрастанию",
        sortDescending: "По убыванию",
        filter: "Фильтр",
        columns: "Колонки"
    });

kendo.ui.Groupable.prototype.options.messages =
    $.extend(kendo.ui.Groupable.prototype.options.messages, {
        empty: "Перетащите заголовок столбца для группировки по нему"
    });

kendo.ui.FilterMenu.prototype.options.messages =
    $.extend(kendo.ui.FilterMenu.prototype.options.messages, {

        info: "Фильтр:",
        filter: "Применить",
        clear: "Отменить",
        search: "Поиск",
        isTrue: "Да",
        isFalse: "Нет",
        and: "И",
        or: "ИЛИ",
        selectValue: "-выберите-"
    });

kendo.ui.FilterMenu.prototype.options.operators =
    $.extend(kendo.ui.FilterMenu.prototype.options.operators, {
        string: {
            eq: "Равно",
            neq: "Не равно",
            startswith: "Начинается с",
            contains: "Содержит",
            doesnotcontain: "Не содержит",
            endswith: "Оканчивается на"
        },
        number: {
            eq: "Равно",
            neq: "Не равно",
            gte: "Больше или равно",
            gt: "Больше",
            lte: "Меньше или равно",
            lt: "Меньше"
        },
        date: {
            eq: "Равно",
            neq: "Не равно",
            gte: "Позже или равно",
            gt: "Позже",
            lte: "Раньше или равно",
            lt: "Раньше"
        },
        enums: {
            eq: "Равно",
            neq: "Не равно"
        }

    });

kendo.ui.Pager.prototype.options.messages =
    $.extend(kendo.ui.Pager.prototype.options.messages, {
        display: "{0} - {1} из {2} записей",
        empty: "Нет данных",
        page: "Страница",
        of: "из {0}",
        itemsPerPage: "записей на странице",
        first: "Первая страница",
        previous: "Предыдущая",
        next: "Следующая",
        last: "Последняя страница",
        refresh: "Обновить"
    });

kendo.ui.Validator.prototype.options.messages =
    $.extend(kendo.ui.Validator.prototype.options.messages, {
        required: "{0} обязателен",
        pattern: "{0} не верен",
        min: "{0} должен быть больше или равен {1}",
        max: "{0} должен быть меньше или равен {1}",
        step: "{0} не верен",
        email: "{0} некорректный email",
        url: "{0} некорректный URL",
        date: "{0} некорректная дата"
    });

kendo.ui.ImageBrowser.prototype.options.messages =
    $.extend(kendo.ui.ImageBrowser.prototype.options.messages, {
        uploadFile: "Загрузить",
        orderBy: "Сортировать по",
        orderByName: "Имя",
        orderBySize: "Размер",
        directoryNotFound: "Каталог с указанным именем не существует",
        emptyFolder: "Каталог пуст",
        deleteFile: 'Вы действительно хотите удалить "{0}"?',
        invalidFileType: "Выбранный файл \"{0}\" не поддерживается. Доступные типы {1}.",
        overwriteFile: "Файл \"{0}\" уже существует. Заменить?",
        dropFilesHere: "Перетащите сюда файлы для загрузки"
    });

kendo.ui.Editor.prototype.options.messages =
    $.extend(kendo.ui.Editor.prototype.options.messages, {
        bold: "Жирный",
        italic: "Курсив",
        underline: "Подчеркнутый",
        strikethrough: "Зачеркнутый",
        superscript: "Верхний индекс",
        subscript: "Нижний индекс",
        justifyCenter: "По центру",
        justifyLeft: "По левому краю",
        justifyRight: "По правому краю",
        justifyFull: "По середине",
        insertUnorderedList: "Вставить маркированный список",
        insertOrderedList: "Вставить нумерованный список",
        indent: "Уменьшить отступ",
        outdent: "Увеличить отступ",
        createLink: "Вставить ссылку",
        unlink: "Убрать ссылку",
        insertImage: "Вставить изображение",
        insertHtml: "Вставить HTML",
        fontName: "Шрифт",
        fontNameInherit: "шрифт",
        fontSize: "Размер шрифта",
        fontSizeInherit: "размер",
        formatBlock: "Форматирование",
        foreColor: "Цвет шрифта",
        backColor: "Цвет фона",
        style: "Стиль",
        emptyFolder: "Пустой каталог",
        uploadFile: "Загрузить файл",
        orderBy: "Сортировать по:",
        orderBySize: "Размер",
        orderByName: "Имя",
        invalidFileType: "Выбранный файл \"{0}\" не поддерживается. Доступные типы {1}.",
        overwriteFile: "Файл \"{0}\" уже существует. Заменить?",
        deleteFile: 'Вы уверены что хотите удалить "{0}"?',
        directoryNotFound: "Каталог не найден.",
        imageWebAddress: "Web адрес",
        imageAltText: "Альтернативный текст",
        dialogInsert: "Вставить",
        dialogButtonSeparator: "или",
        dialogCancel: "Отменить",
        formatting: "Формат"
    });

kendo.ui.Editor.prototype.options.messages =
$.extend(kendo.ui.Editor.prototype.options.messages, {

    /* EDITOR MESSAGES
     ****************************************************************************/
    bold: "Полужирный",
    italic: "Курсив",
    underline: "Подчеркнутый",
    strikethrough: "Зачеркнутый",
    superscript: "Верхний индекс",
    subscript: "Нижний индекс",
    justifyCenter: "По центру",
    justifyLeft: "По левому краю",
    justifyRight: "По правому краю",
    justifyFull: "По ширине",
    insertUnorderedList: "Вставить маркированный список",
    insertOrderedList: "Вставить нумерованный список",
    indent: "Увеличить отступ",
    outdent: "Уменьшить отступ",
    createLink: "Вставить гиперссылку",
    unlink: "Удалить гиперссылку",
    insertImage: "Вставить изображение",
    createTable: "Вставить таблицу",
    addRowAbove: "Вставить строку сверху",
    addRowBelow: "Вставить строку снизу",
    addColumnLeft: "Вставить столбец слева",
    addColumnRight: "Вставить столбец справа",
    deleteRow: "Удалить строку",
    deleteColumn: "Удалить столбец",
    viewHtml: "Просмотр HTML",
    insertHtml: "Вставить HTML",
    fontName: "Шрифт",
    fontNameInherit: "(наследовать шрифт)",
    fontSize: "Размер шрифта",
    fontSizeInherit: "(наследовать размер)",
    formatting: "Форматирование",
    foreColor: "Цвет шрифта",
    backColor: "Цвет фона",
    style: "Стиль",
    emptyFolder: "Пустой каталог",
    uploadFile: "Загрузить файл",
    orderBy: "Сортировать по:",
    orderBySize: "Размер",
    orderByName: "Имя",
    invalidFileType: "Выбранный файл \"{0}\" не поддерживается. Доступные типы {1}.",
    overwriteFile: "Файл \"{0}\" уже существует. Заменить?",
    deleteFile: 'Вы действительно хотите удалить "{0}"?',
    directoryNotFound: "Каталог с указанным именем не существует",
    imageWebAddress: "Веб-адрес",
    imageAltText: "Альтернативный текст",
    dialogInsert: "Вставить",
    dialogUpdate: "Обновить",
    dialogButtonSeparator: "или",
    dialogCancel: "Отменить",
    linkWebAddress: "Веб-адрес",
    linkText: "Текст",
    linkToolTip: "Всплывающая подсказка",
    linkOpenInNewWindow: "Открыть ссылку в новом окне"
    /***************************************************************************/
});

kendo.ui.Upload.prototype.options.localization =
    $.extend(kendo.ui.Upload.prototype.options.localization, {
        select: "Выберите файл",
        cancel: "Отмена загрузки",
        clearSelectedFiles: "Очистить загруженные файлы",
        dropFilesHere: "Перетащите сюда файлы для загрузки",
        headerStatusUploaded: "Файл загружен",
        headerStatusUploading: "Файл загружается",
        invalidFileExtension: "Данный формат не поддерживается",
        invalidMaxFileSize: "Превышен максимальный размер",
        invalidMinFileSize: "Слишком маленький размер файла",
        remove: "Удалить файл",
        retry: "Повторить загрузку",
        statusFailed: "Ошибка загрузки",
        statusUploaded: "Файл загружен",
        statusUploading: "Файл загружается",
        uploadSelectedFiles: "Загрузить выбранные файлы",
    });

kendo.ui.AutoComplete.prototype.options =
    $.extend(kendo.ui.AutoComplete.prototype.options, {
        noDataTemplate: "Данные отсутствуют"
    });

(function (trv, $) {
    "use strict";

    var sr = {
        controllerNotInitialized: 'Controller is not initialized.',
        noReportInstance: 'No report instance.',
        missingTemplate: 'ReportViewer template is missing. Please specify the templateUrl in the options.',
        noReport: 'Отсутствует шаблон отчета.',
        noReportDocument: 'Отчет отсутствует.',
        invalidParameter: 'Please input a valid value.',
        invalidDateTimeValue: 'Please input a valid date.',
        parameterIsEmpty: 'Parameter value cannot be empty.',
        cannotValidateType: 'Cannot validate parameter of type {type}.',
        loadingFormats: 'Загрузка форматов...',
        loadingReport: 'Загрузка отчета...',
        preparingDownload: 'Подготовка документа к загрузке. Ожидайте...',
        preparingPrint: 'Подготовка документа к печати. Ожидайте...',
        errorLoadingTemplates: 'Error loading the report viewer\'s templates.',
        loadingReportPagesInProgress: 'Загрузка...',
        loadedReportPagesComplete: 'Всего загружено {0} стр.',
        noPageToDisplay: "Нет данных для отображения.",
        errorDeletingReportInstance: 'Error deleting report instance: {0}',
        errorRegisteringViewer: 'Error registering the viewer with the service.',
        noServiceClient: 'No serviceClient has been specified for this controller.',
        errorRegisteringClientInstance: 'Error registering client instance',
        errorCreatingReportInstance: 'Error creating report instance (Report = {0})',
        errorCreatingReportDocument: 'Error creating report document (Report = {0}; Format = {1})',
        unableToGetReportParameters: "Ошибка получения параметров отчета"
    };

    trv.sr = $.extend(trv.sr, sr);

}(window.telerikReportViewer = window.telerikReportViewer || {}, jQuery));