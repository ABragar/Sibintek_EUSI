(function () {
    'use strict';

    var gridTemplates = window.gridTemplates = {};

    gridTemplates.enum = function (typeEnum, valEnum, text) {
        var init = function (el, $enum) {
            el.append($('<i>').addClass($enum.Icon).css('color', $enum.Color)).append($('<span>').text(text || $enum.Title));
        };

        var getKey = function (v) {
            return typeEnum + '_' + v;
        };

        var attr = 'data-tmp_enum_val';

        //отложенный вызов через минимальный «тик» таймера
        //чтобы функция сработала после того, как текущий скрипт завершится
        setTimeout(function () {
            application.UiEnums.get(typeEnum,
                function (result) {
                    for (var v in result.Values) {
                        if (result.Values.hasOwnProperty(v)) {
                            var key = getKey(v);

                            $('[' + attr + '="' + key + '"]').removeAttr(attr).each(function (indx, element) {
                                var $enum = result.Values[v];
                                init($(element), $enum);
                            });
                        }
                    }
                });
        }, 0);

        return '<div ' + attr + '="' + getKey(valEnum) + '"></div>';
    };

    gridTemplates.vmConfig = function (type) {
        var init = function (el, config) {
            el.append($('<i>').addClass(config.Icon.Value).css('color', config.Icon.Color)).append($('<span>').text(config.DetailView.Title));
        };

        var attr = 'data-tmp_vmconfig';

        //отложенный вызов через минимальный «тик» таймера
        //чтобы функция сработала после того, как текущий скрипт завершится
        setTimeout(function () {
            application.viewModelConfigs.get(type).done(
                function (config) {
                    $('[' + attr + '="' + type + '"]').removeAttr(attr).each(function (indx, element) {
                        init($(element), config);
                    });
                });
        }, 0);

        return '<div ' + attr + '="' + type + '"></div>';
    };

    gridTemplates.linkBaseObject = function (gridUid, linkBo) {
        if (linkBo.ID !== 0) {
            var attr = 'data-tmp_linkBaseObject';
            var attrKey = linkBo.Mnemonic + '_' + linkBo.ID;

            var init = function (el, config, pr) {
                el.addClass('base-object-one-cell');

                if (!pr)
                    return;

                if (config.Icon && config.Icon.Value && config.Icon.Color) {
                    var $span = $('<span>').addClass(config.Icon.Value).css('color', config.Icon.Color);
                    el = el.append($span);
                }

                var uid = gridUid + '_' + config.Mnemonic;

                var $a = $(
                    '<a href="javascript: void(0)"' +
                        'onclick="pbaAPI.openDetailView(\'' + config.Mnemonic + '\', { wid: \'' + uid + '\', title: \'' + config.DetailView.Title + '\', id: ' + pr.ID + '});">' +
                    '>')
                    .text(config.DetailView.Title);

                el.append($a);
            };

            //отложенный вызов через минимальный «тик» таймера
            //чтобы функция сработала после того, как текущий скрипт завершится
            setTimeout(function () {
                application.viewModelConfigs.get(linkBo.Mnemonic || linkBo.TypeName).done(
                    function (config) {
                        $('div[' + attr + '="' + attrKey + '"]').removeAttr(attr).each(function (indx, element) {
                            init($(element), config, linkBo);
                        });
                    });
            }, 0);

            return '<div ' + attr + '="' + attrKey + '"></div>';
        }

        return '';
    };

    gridTemplates.period = function (start, end, mnemonic) {
        // Задача 10440
        // "Period" в классе "AccountingMoving" (регистра движений (РСБУ, МСФО, НУ)).
        // Формат значения поля должен быть в виде "Месяц ГГГГ".
        var mnemonicsForMonthFormat = ['AccMovingRSBU', 'AccMovingMSFO', 'AccMovingNU']
        var isMonthFormat = mnemonicsForMonthFormat.indexOf(mnemonic) != -1
        var dateFormat = isMonthFormat
            ? application.MONTH_FORMAT
            : application.DATE_FORMATE;

        var format = function (date) {
            return kendo.toString(kendo.parseDate(date, application.DATE_TIME_FORMATE), dateFormat);
        };

        var res = '<span class="glyphicon glyphicon-calendar"></span>&nbsp;';

        if (start) {
            res = res + format(start);
        }

        if (end && !isMonthFormat) {
            res = res + ' ~ ' + format(end);
        }

        return res;
    };

    gridTemplates.location = function(location) {
        var res = '<span class="halfling halfling-map-marker">&nbsp;</span>';

        if (location && location.Address) {
            res = res + location.Address;
        } else {
            res = res + 'Отсутсвует';
        }

        return res + '</span>';
    };

    gridTemplates.baseObject = function (gridUid, mnemonic, property) {
        var attr = 'data-tmp_baseObject';
        var attrKey = mnemonic;

        if (property)
            attrKey += '_' + property.ID;

        var init = function (el, config, pr) {
            el.addClass('base-object-one-cell');

            if (!pr)
                return;

            if (config.LookupProperty && config.LookupProperty.Image) {
                var $img = $('<img>').addClass('img-circle').attr('src', pbaAPI.imageHelpers.getImageThumbnailSrc(pr[config.LookupProperty.Image], 'XXS')).attr('alt', '""');
                $img.append('&nbsp;');
                el = el.append($img);
            }

            if (config.Icon && config.Icon.Value && config.Icon.Color) {
                var $span = $('<span>').addClass(config.Icon.Value).css('color', config.Icon.Color).addClass('base-object-one-cell__icon');
                el = el.append($span);
            }

            var uid = gridUid + '_' + config.Mnemonic;

            var $a = $(
                '<a href="javascript: void(0)"' +
                    'onclick="pbaAPI.openDetailView(\'' + config.Mnemonic + '\', { wid: \'' + uid + '\', title: \'' + config.DetailView.Title + '\', id: ' + pr.ID + '});">' +
                '>')
                .text(pr[config.LookupProperty.Text]);

            if (config.Preview === true) {
                $a.addClass('valign').attr('data-mnemonic', config.Mnemonic).attr('data-id', pr.ID || '');
            }

            el.append($a);
        };

        //отложенный вызов через минимальный «тик» таймера
        //чтобы функция сработала после того, как текущий скрипт завершится
        setTimeout(function () {
            application.viewModelConfigs.get(mnemonic).done(
                function (config) {
                    $('div[' + attr + '="' + attrKey + '"]').removeAttr(attr).each(function (indx, element) {
                        init($(element), config, property);
                    });
                });
        }, 0);

        return '<div ' + attr + '="' + attrKey + '"></div>';
    };
}());