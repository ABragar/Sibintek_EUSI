(function ($, pbaApi, window) {
    'use strict';

    var operatorsRu = {
        equal: { op: '= ?' },
        not_equal: { op: '!= ?' },
        in: { op: 'ИЗ УКАЗАННЫХ(?)', sep: ', ' },
        not_in: { op: 'НЕ ИЗ УКАЗАННЫХ(?)', sep: ', ' },
        less: { op: '< ?' },
        less_or_equal: { op: '<= ?' },
        greater: { op: '> ?' },
        greater_or_equal: { op: '>= ?' },
        between: { op: 'МЕЖДУ ?', sep: ' И ' },
        not_between: { op: 'НЕ МЕЖДУ ?', sep: ' И ' },
        begins_with: { op: 'НАЧИНАЕТСЯ С(?)', mod: '{0}%' },
        not_begins_with: { op: 'НЕ НАЧИНАЕТСЯ С(?)', mod: '{0}%' },
        contains: { op: 'СОДЕРЖИТ(?)', mod: '%{0}%' },
        not_contains: { op: 'НЕ СОДЕРЖИТ(?)', mod: '%{0}%' },
        ends_with: { op: 'ОКАНЧИВАЕТСЯ НА(?)', mod: '%{0}' },
        not_ends_with: { op: 'НЕ ОКАНЧИВАЕТСЯ НА(?)', mod: '%{0}' },
        is_empty: { op: '= \'\'' },
        is_not_empty: { op: '!= \'\'' },
        is_null: { op: 'ПУСТО' },
        is_not_null: { op: 'НЕ ПУСТО' },
        quantity_1: { op: 'КОЛИЧЕСТВО РАВНО ОДНОМУ' },
        quantity_less: { op: 'КОЛИЧЕСТВО МЕНЬШЕ ?' },
        quantity_equal: { op: 'КОЛИЧЕСТВО РАВНО ?' },
        quantity_greater: { op: 'КОЛИЧЕСТВО БОЛЬШЕ ?' },
        quantity_from_to: { op: 'КОЛИЧЕСТВО ОТ ?', sep: ' ДО ' },
        collection_in: { op: 'В КОТОРЫХ ЕСТЬ ?', sep: ', ' },
        collection_not_in: { op: 'В КОТОРЫХ НЕТ ?', sep: ', ' },
        aggregate_sum: { op: 'СУММА ПО ПОЛЮ ?' },
        aggregate_average: { op: 'СРЕДНЕЕ ЗНАЧЕНИЕ ПО ПОЛЮ ?' },
        aggregate_max: { op: 'МАКСИМАЛЬНОЕ ЗНАЧЕНИЕ ПО ПОЛЮ ?' },
        aggregate_min: { op: 'МИНИМАЛЬНОЕ ЗНАЧЕНИЕ ПО ПОЛЮ ?'}
    };

    var getFriendly = function (data) {
        data = (data === undefined) ? this.getRules() : data;

        if (!data) {
            return null;
        }

        var self = this;

        var boolean_as_integer = self.getPluginOptions('sql-support', 'boolean_as_integer');

        var separator = ' ';
        var result = (function parse(group) {
            if (!group.condition) {
                group.condition = self.settings.default_condition;
            }
            if (['AND', 'OR'].indexOf(group.condition.toUpperCase()) === -1) {
                Utils.error('UndefinedSQLCondition', 'Unable to build SQL query with condition "{0}"', group.condition);
            }

            if (!group.rules) {
                return '';
            }

            var parts = [];

            group.rules.forEach(function (rule) {
                if (rule.rules && rule.rules.length > 0) {
                    var tt = parse(rule);
                    if (tt) {
                        parts.push('(' + separator + tt + separator + ')' + separator);
                    }
                    else {
                        parts.push('(' + separator + tt + separator + ')' + separator);
                    }
                }
                else {
                    var sql = self.operatorsRu[rule.operator];

                    var ope = self.getOperatorByType(rule.operator);
                    var value = '';

                    var sqlFn = function (v) {
                        return sql.op.replace(/\?/, v);
                    };

                    if (sql === undefined) {
                        Utils.error('UndefinedSQLOperator', 'Unknown SQL operation for operator "{0}"', rule.operator);
                    }
                    if (rule.operator.indexOf('aggregate') !== -1) {
                        value = rule.value.label + ' ' + self.operatorsRu[rule.value.operator].op.replace(/\?/, rule.value.value);
                    } else
                    if (ope.nb_inputs !== 0) {
                        var values = rule.value instanceof Array ? rule.value : [rule.value];

                        values.forEach(function (v, i) {
                            if (i > 0) {
                                value += sql.sep;
                            }

                            if (rule.type == 'integer' || rule.type == 'double' || rule.type == 'boolean') {
                                v = Utils.changeType(v, rule.type, boolean_as_integer);
                            }

                            if (sql.mod) {
                                v = Utils.fmt(sql.mod, v);
                            }

                            if (typeof v == 'string') {
                                v = '\'' + v + '\'';
                            }

                            if (v.condition) {
                                v = parse(v);
                            }

                            value += v;
                        });
                    }
                  
                    var label = "[" + self.change('getSQLField', rule.label, rule) + "]";
                    var ruleExpression = label + ' ' + sqlFn(value);

                    parts.push(self.change('ruleToSQL', ruleExpression, rule, value, sqlFn));
                }
            });

            var groupExpression = parts.join(' ' + group.condition + ' ');

            var res = self.change('groupToSQL', groupExpression, group);

            return res.replace(/ AND /g, ' И ').replace(/ OR /g, ' ИЛИ ');
        }(data));

        return result;
    }

    var getRules = function (options) {
        options = $.extend({
            get_flags: false,
            allow_invalid: false,
            skip_empty: false
        },
            options);

        var valid = this.validate(options);
        if (!valid && !options.allow_invalid) {
            return null;
        }

        var self = this;

        var out = (function parse(group) {
            var groupData = {
                condition: group.condition,
                rules: []
            };

            if (group.data) {
                groupData.data = $.extendext(true, 'replace', {}, group.data);
            }

            if (options.get_flags) {
                var flags = self.getGroupFlags(group.flags, options.get_flags === 'all');
                if (!$.isEmptyObject(flags)) {
                    groupData.flags = flags;
                }
            }

            group.each(function (rule) {
                if (!rule.filter && options.skip_empty) {
                    return;
                }

                var value = null;
                if (!rule.operator || rule.operator.nb_inputs !== 0) {
                    value = rule.value;
                }

                var ruleData = {
                    id: rule.filter ? rule.filter.id : null,
                    field: rule.filter ? rule.filter.field : null,
                    type: rule.filter ? rule.filter.type : null,
                    input: rule.filter ? rule.filter.input : null,
                    operator: rule.operator ? rule.operator.type : null,
                    label: rule.filter ? rule.filter.label : null,
                    value: value
                };

                if (rule.filter && rule.filter.data || rule.data) {
                    ruleData.data = $.extendext(true, 'replace', {}, rule.filter.data, rule.data);
                }

                if (options.get_flags) {
                    var flags = self.getRuleFlags(rule.flags, options.get_flags === 'all');
                    if (!$.isEmptyObject(flags)) {
                        ruleData.flags = flags;
                    }
                }

                /**
                 * Modifies the JSON generated from a Rule object
                 * @event changer:ruleToJson
                 * @memberof QueryBuilder
                 * @param {object} json
                 * @param {Rule} rule
                 * @returns {object}
                 */
                groupData.rules.push(self.change('ruleToJson', ruleData, rule));

            },
                function (model) {
                    var data = parse(model);
                    if (data.rules.length !== 0 || !options.skip_empty) {
                        groupData.rules.push(data);
                    }
                },
                this);

            /**
             * Modifies the JSON generated from a Group object
             * @event changer:groupToJson
             * @memberof QueryBuilder
             * @param {object} json
             * @param {Group} group
             * @returns {object}
             */
            return self.change('groupToJson', groupData, group);

        }(this.model.root));

        out.valid = valid;

        /**
         * Modifies the result of the {@link QueryBuilder#getRules} method
         * @event changer:getRules
         * @memberof QueryBuilder
         * @param {object} json
         * @returns {object}
         */
        return this.change('getRules', out);
    }

    var $builder = $('<div/>');
    $builder.queryBuilder.extend({
        operatorsRu: operatorsRu,
        getFriendly: getFriendly,
        getRules: getRules
    });
    // Friendly names end

    /**
 * Allowed types and their internal representation
 * @type {object.<string, string>}
 * @readonly
 * @private
 */
    var QueryBuilder = {};
    QueryBuilder.types = {
        'string': 'string',
        'integer': 'number',
        'double': 'number',
        'date': 'datetime',
        'time': 'datetime',
        'datetime': 'datetime',
        'boolean': 'boolean'
    };

    $.extend($.fn.queryBuilder.constructor.OPERATORS,
        {
            quantity_1: {
                type: 'quantity_1',
                nb_inputs: 0,
                multiple: false,
                apply_to: ['string', 'number', 'datetime', 'boolean']
            },
            quantity_less: {
                type: 'quantity_less',
                nb_inputs: 1,
                multiple: false,
                apply_to: ['string', 'number', 'datetime', 'boolean']
            },
            quantity_equal: {
                type: 'quantity_equal',
                nb_inputs: 1,
                multiple: false,
                apply_to: ['string', 'number', 'datetime', 'boolean']
            },
            quantity_greater: {
                type: 'quantity_greater',
                nb_inputs: 1,
                multiple: false,
                apply_to: ['string', 'number', 'datetime', 'boolean']
            },
            quantity_from_to: {
                type: 'quantity_from_to',
                nb_inputs: 2,
                multiple: false,
                apply_to: ['string', 'number', 'datetime', 'boolean']
            },
            aggregate_sum: {
                type: 'aggregate_sum',
                nb_inputs: 1
            },
            collection_in: {
                type: 'collection_in',
                nb_inputs: 1
            },
            collection_not_in: {
                type: 'collection_not_in',
                nb_inputs: 1
            },
            aggregate_average: {
                type: 'aggregate_average',
                nb_inputs: 1
            },
            aggregate_max: {
                type: 'aggregate_max',
                nb_inputs: 1
            },
            aggregate_min: {
                type: 'aggregate_min',
                nb_inputs: 1
            }
        });

    $.fn.queryBuilder.constructor.DEFAULTS.operators.push(
        'quantity_1',
        'quantity_less',
        'quantity_equal',
        'quantity_greater',
        'quantity_from_to',
        'aggregate_sum',
        'collection_in',
        'collection_not_in',
        'aggregate_average',
        'aggregate_max',
        'aggregate_min'
    );

    // jquery-builder.ru.js does not contain not_between translation
    $.fn.queryBuilder.regional["ru"]["operators"]["not_between"] = "не между";
    $.fn.queryBuilder.regional["ru"]["operators"]["quantity_1"] = "количество равно одному";
    $.fn.queryBuilder.regional["ru"]["operators"]["quantity_less"] = "количество меньше";
    $.fn.queryBuilder.regional["ru"]["operators"]["quantity_equal"] = "количество равно";
    $.fn.queryBuilder.regional["ru"]["operators"]["quantity_greater"] = "количество больше";
    $.fn.queryBuilder.regional["ru"]["operators"]["quantity_from_to"] = "количество от до";
    $.fn.queryBuilder.regional["ru"]["operators"]["aggregate_sum"] = "сумма";
    $.fn.queryBuilder.regional["ru"]["operators"]["collection_in"] = "в которых есть";
    $.fn.queryBuilder.regional["ru"]["operators"]["collection_not_in"] = "в которых нет";
    $.fn.queryBuilder.regional["ru"]["operators"]["aggregate_average"] = "среднее";
    $.fn.queryBuilder.regional["ru"]["operators"]["aggregate_max"] = "максимальное";
    $.fn.queryBuilder.regional["ru"]["operators"]["aggregate_min"] = "минимальное";



    $.fn.queryBuilder.constructor.prototype._validateValue = function (rule, value) {
        var filter = rule.filter;
        var operator = rule.operator;

        var validation = filter.validation || {};
        // does not allow empty values for default validation
        validation.allow_empty_value = false;

        var result = true;
        var tmp, tempValue;

        if (rule.operator.nb_inputs === 1) {
            value = [value];
        }

        for (var i = 0; i < operator.nb_inputs; i++) {
            if (!operator.multiple && $.isArray(value[i]) && value[i].length > 1) {
                result = ['operator_not_multiple', operator.type, this.translate('operators', operator.type)];
                break;
            }

            switch (filter.input) {
                case 'radio':
                    if (value[i] === undefined || value[i].length === 0) {
                        if (!validation.allow_empty_value) {
                            result = ['radio_empty'];
                        }
                        break;
                    }
                    break;

                case 'checkbox':
                    if (value[i] === undefined || value[i].length === 0) {
                        if (!validation.allow_empty_value) {
                            result = ['checkbox_empty'];
                        }
                        break;
                    }
                    break;

                case 'select':
                    if (value[i] === undefined || value[i].length === 0 || (filter.placeholder && value[i] == filter.placeholder_value)) {
                        if (!validation.allow_empty_value) {
                            result = ['select_empty'];
                        }
                        break;
                    }
                    break;

                default:
                    tempValue = $.isArray(value[i]) ? value[i] : [value[i]];

                    for (var j = 0; j < tempValue.length; j++) {
                        switch (QueryBuilder.types[filter.type]) {
                            case 'string':
                                if (rule.operator.type === 'in' || rule.operator.type === 'not_in') {
                                    validation.format = /^(\w+)(;\s*\w+)*$/;
                                }
                                if (tempValue[j] === undefined || tempValue[j].length === 0) {
                                    if (!validation.allow_empty_value) {
                                        result = ['string_empty'];
                                    }
                                    break;
                                }
                                if (validation.min !== undefined) {
                                    if (tempValue[j].length < parseInt(validation.min)) {
                                        result = [this.getValidationMessage(validation, 'min', 'string_exceed_min_length'), validation.min];
                                        break;
                                    }
                                }
                                if (validation.max !== undefined) {
                                    if (tempValue[j].length > parseInt(validation.max)) {
                                        result = [this.getValidationMessage(validation, 'max', 'string_exceed_max_length'), validation.max];
                                        break;
                                    }
                                }
                                if (validation.format) {
                                    if (typeof validation.format == 'string') {
                                        validation.format = new RegExp(validation.format);
                                    }
                                    if (!validation.format.test(tempValue[j])) {
                                        result = [this.getValidationMessage(validation, 'format', 'string_invalid_format'), validation.format];
                                        break;
                                    }
                                }
                                break;

                            case 'number':
                                if (rule.operator.type === 'in' || rule.operator.type === 'not_in' || !rule.filter.isQuantity) {
                                    if (rule.filter.type === 'integer') {
                                        if (!/^([+-]?\d+)+([\;][+-]?\d+)*$/.test(tempValue[j])) {
                                            result = [this.getValidationMessage(validation, 'format', 'string_invalid_format'), validation.format];
                                            break;
                                        }
                                    }
                                    else {
                                        if (!/^([+-]?\d+([\.]\d+)?)+([\;][+-]?\d+([\.]\d+)?)*$/.test(tempValue[j])) {
                                            result = [this.getValidationMessage(validation, 'format', 'string_invalid_format'), validation.format];
                                            break;
                                        }
                                    }
                                }
                                else {
                                    if (tempValue[j] === undefined || tempValue[j].length === 0) {
                                        if (!validation.allow_empty_value) {
                                            result = ['number_nan'];
                                        }
                                        break;
                                    }
                                    if (isNaN(tempValue[j])) {
                                        result = ['number_nan'];
                                        break;
                                    }
                                    if (filter.type == 'integer') {
                                        if (parseInt(tempValue[j]) != tempValue[j]) {
                                            result = ['number_not_integer'];
                                            break;
                                        }
                                    }
                                    else {
                                        if (parseFloat(tempValue[j]) != tempValue[j]) {
                                            result = ['number_not_double'];
                                            break;
                                        }
                                    }
                                    if (validation.min !== undefined) {
                                        if (tempValue[j] < parseFloat(validation.min)) {
                                            result = [this.getValidationMessage(validation, 'min', 'number_exceed_min'), validation.min];
                                            break;
                                        }
                                    }
                                    if (validation.max !== undefined) {
                                        if (tempValue[j] > parseFloat(validation.max)) {
                                            result = [this.getValidationMessage(validation, 'max', 'number_exceed_max'), validation.max];
                                            break;
                                        }
                                    }
                                    if (validation.step !== undefined && validation.step !== 'any') {
                                        var v = (tempValue[j] / validation.step).toPrecision(14);
                                        if (parseInt(v) != v) {
                                            result = [this.getValidationMessage(validation, 'step', 'number_wrong_step'), validation.step];
                                            break;
                                        }
                                    }
                                }
                                break;

                            case 'datetime':
                                if (tempValue[j] === undefined || tempValue[j].length === 0) {
                                    if (!validation.allow_empty_value) {
                                        result = ['datetime_empty'];
                                    }
                                    break;
                                }

                                // we need MomentJS
                                if (validation.format) {
                                    if (!('moment' in window)) {
                                        Utils.error('MissingLibrary', 'MomentJS is required for Date/Time validation. Get it here http://momentjs.com');
                                    }

                                    var datetime = moment(tempValue[j], validation.format);
                                    if (!datetime.isValid()) {
                                        result = [this.getValidationMessage(validation, 'format', 'datetime_invalid'), validation.format];
                                        break;
                                    }
                                    else {
                                        if (validation.min) {
                                            if (datetime < moment(validation.min, validation.format)) {
                                                result = [this.getValidationMessage(validation, 'min', 'datetime_exceed_min'), validation.min];
                                                break;
                                            }
                                        }
                                        if (validation.max) {
                                            if (datetime > moment(validation.max, validation.format)) {
                                                result = [this.getValidationMessage(validation, 'max', 'datetime_exceed_max'), validation.max];
                                                break;
                                            }
                                        }
                                    }
                                }
                                break;

                            case 'boolean':
                                if (tempValue[j] === undefined || tempValue[j].length === 0) {
                                    if (!validation.allow_empty_value) {
                                        result = ['boolean_not_valid'];
                                    }
                                    break;
                                }
                                tmp = ('' + tempValue[j]).trim().toLowerCase();
                                if (tmp !== 'true' && tmp !== 'false' && tmp !== '1' && tmp !== '0' && tempValue[j] !== 1 && tempValue[j] !== 0) {
                                    result = ['boolean_not_valid'];
                                    break;
                                }
                        }

                        if (result !== true) {
                            break;
                        }
                    }
            }

            if (result !== true) {
                break;
            }
        }

        if ((rule.operator.type === 'between' || rule.operator.type === 'not_between') && value.length === 2) {
            switch (QueryBuilder.types[filter.type]) {
                case 'number':
                    if (value[0] > value[1]) {
                        result = ['number_between_invalid', value[0], value[1]];
                    }
                    break;

                case 'datetime':
                    // we need MomentJS
                    if (validation.format) {
                        if (!('moment' in window)) {
                            Utils.error('MissingLibrary', 'MomentJS is required for Date/Time validation. Get it here http://momentjs.com');
                        }

                        if (moment(value[0], validation.format).isAfter(moment(value[1], validation.format))) {
                            result = ['datetime_between_invalid', value[0], value[1]];
                        }
                    }
                    break;
            }
        }

        return result;
    };

    $.fn.queryBuilder.constructor.prototype.getRuleInput = function (rule, value_id) {
        var filter = rule.filter;
        var validation = rule.filter.validation || {};
        var name = rule.id + '_value_' + value_id;
        var c = filter.vertical ? ' class=block' : '';
        var h = '';
        var operator = rule.operator.type;

        if (operator !== "in" && operator !== "not_in" && filter.plugin === "queryTreeInAndNotIn") {
            filter = filter.oldFilter;
        }
        /*if (operator.substring('quantity' !== -1)) {
            filter.input = 'number';
            validation.min = 0;
        }*/
        if (typeof filter.input === 'function') {
            h = filter.input.call(this, rule, name);
        }
        else {
            switch (filter.input) {
                case 'radio':
                case 'checkbox':
                    Utils.iterateOptions(filter.values, function (key, val) {
                        h += '<label' + c + '><input type="' + filter.input + '" name="' + name + '" value="' + key + '"> ' + val + '</label> ';
                    });
                    break;

                case 'select':
                    h += '<select class="form-control" name="' + name + '"' + (filter.multiple ? ' multiple' : '') + '>';
                    if (filter.placeholder) {
                        h += '<option value="' + filter.placeholder_value + '" disabled selected>' + filter.placeholder + '</option>';
                    }
                    Utils.iterateOptions(filter.values, function (key, val) {
                        h += '<option value="' + key + '">' + val + '</option> ';
                    });
                    h += '</select>';
                    break;

                case 'textarea':
                    h += '<textarea name="' + name + '"';
                    if (filter.size) h += ' cols="' + filter.size + '"';
                    if (filter.rows) h += ' rows="' + filter.rows + '"';
                    if (validation.min !== undefined) h += ' minlength="' + validation.min + '"';
                    if (validation.max !== undefined) h += ' maxlength="' + validation.max + '"';
                    if (filter.placeholder) h += ' placeholder="' + filter.placeholder + '"';
                    h += ' style="border: 1px solid rgb(68, 68, 68); max-height:500px"></textarea>';
                    break;

                case 'number':
                    h += '<input class="form-control" type="number"' + ' name="' + name + '"';
                    if (validation.step !== undefined) h += ' step="' + validation.step + '"';
                    if (validation.min !== undefined) h += ' min="' + validation.min + '"';
                    if (validation.max !== undefined) h += ' max="' + validation.max + '"';
                    if (filter.placeholder) h += ' placeholder="' + filter.placeholder + '"';
                    if (filter.size) h += ' size="' + filter.size + '"';
                    h += '>';
                    break;

                default:
                    h += '<input class="form-control" type="text" name="' + name + '"';
                    if (filter.placeholder) h += ' placeholder="' + filter.placeholder + '"';
                    if (filter.type === 'string' && validation.min !== undefined) h += ' minlength="' + validation.min + '"';
                    if (filter.type === 'string' && validation.max !== undefined) h += ' maxlength="' + validation.max + '"';
                    if (filter.size) h += ' size="' + filter.size + '"';
                    h += '>';
            }
        }

        /**
         * Modifies the raw HTML of the rule's input
         * @event changer:getRuleInput
         * @memberof QueryBuilder
         * @param {string} html
         * @param {Rule} rule
         * @param {string} name - the name that the input must have
         * @returns {string}
         */
        return this.change('getRuleInput', h, rule, name);
    };

    $.fn.queryBuilder.constructor.prototype.updateRuleOperator = function (rule, previousOperator) {
        var $valueContainer = rule.$el.find($.fn.queryBuilder.constructor.selectors.value_container);
        var ruleValue = rule.value;
        var operatorType = rule.operator.type;

        var filters = rule.filter.filters;

        if (operatorType === "collection_in" || operatorType === "collection_not_in") {
            rule.__.filter = filters.QueryTree;
        } else if (operatorType.indexOf("quantity") !== -1) {
            rule.__.filter = {
                id: rule.filter.id,
                field: rule.filter.field,
                operators: rule.filter.operators,
                label: rule.filter.label,
                input: 'number',
                type: 'integer',
                validation: {
                    min: 0
                }
            };
        } else if (operatorType.indexOf("aggregate") !== -1) {
            rule.__.filter = filters.AggregateFuncs;
        } else if (operatorType === "in" || operatorType === "not_in") {
            rule.__.filter = filters.InAndNotIn;
        } else if (filters.Default) {
            rule.__.filter = filters.Default;
        }     

        rule.__.filter.filters = filters;

        /*if (rule.operator.type.indexOf("aggregate") !== -1) {
            rule.filter.oldFilter = rule.filter.oldFilter || Object.assign({}, rule.filter);
            $.extend(rule.filter, window.querytreeFilters.AggregateFuncs(rule.filter));
            rule.filter.isAggregate = true;
        } else if (rule.filter.isAggregate) {
            rule.filter = rule.filter.oldFilter || rule.filter;
        }*/

        if (!rule.operator || rule.operator.nb_inputs === 0) {
            $valueContainer.hide();
            rule.__.value = undefined;
        }
        else {
            $valueContainer.show();
            if ($valueContainer.is(':empty') || !previousOperator ||
                rule.operator !== previousOperator) {
                this.createRuleInput(rule);
            }
        }

        if (rule.operator) {
            rule.$el.find($.fn.queryBuilder.constructor.selectors.rule_operator).val(rule.operator.type);
        }

        /**
         *  After the operator has been updated and the input optionally re-created
         * @event afterUpdateRuleOperator
         * @memberof QueryBuilder
         * @param {Rule} rule
         * @param {object} previousOperator
         */
        this.trigger('afterUpdateRuleOperator', rule, previousOperator);

        this.trigger('rulesChanged');

        // FIXME is it necessary ?
        this.updateRuleValue(rule, ruleValue);
    };

    // define Utils locally begin
    var Utils = {};

    /**
     * @member {object}
     * @memberof QueryBuilder
     * @see Utils
     */
    QueryBuilder.utils = Utils;

    /**
     * @callback Utils#OptionsIteratee
     * @param {string} key
     * @param {string} value
     */

    /**
     * Iterates over radio/checkbox/selection options, it accept three formats
     *
     * @example
     * // array of values
     * options = ['one', 'two', 'three']
     * @example
     * // simple key-value map
     * options = {1: 'one', 2: 'two', 3: 'three'}
     * @example
     * // array of 1-element maps
     * options = [{1: 'one'}, {2: 'two'}, {3: 'three'}]
     *
     * @param {object|array} options
     * @param {Utils#OptionsIteratee} tpl
     */
    Utils.iterateOptions = function (options, tpl) {
        if (options) {
            if ($.isArray(options)) {
                options.forEach(function (entry) {
                    // array of one-element maps
                    if ($.isPlainObject(entry)) {
                        $.each(entry, function (key, val) {
                            tpl(key, val);
                            return false; // break after first entry
                        });
                    }
                    // array of values
                    else {
                        tpl(entry, entry);
                    }
                });
            }
            // unordered map
            else {
                $.each(options, function (key, val) {
                    tpl(key, val);
                });
            }
        }
    };

    /**
     * Replaces {0}, {1}, ... in a string
     * @param {string} str
     * @param {...*} args
     * @returns {string}
     */
    Utils.fmt = function (str, args) {
        if (!Array.isArray(args)) {
            args = Array.prototype.slice.call(arguments, 1);
        }

        return str.replace(/{([0-9]+)}/g, function (m, i) {
            return args[parseInt(i)];
        });
    };

    /**
     * Throws an Error object with custom name or logs an error
     * @param {boolean} [doThrow=true]
     * @param {string} type
     * @param {string} message
     * @param {...*} args
     */
    Utils.error = function () {
        var i = 0;
        var doThrow = typeof arguments[i] === 'boolean' ? arguments[i++] : true;
        var type = arguments[i++];
        var message = arguments[i++];
        var args = Array.isArray(arguments[i]) ? arguments[i] : Array.prototype.slice.call(arguments, i);

        if (doThrow) {
            var err = new Error(Utils.fmt(message, args));
            err.name = type + 'Error';
            err.args = args;
            throw err;
        }
        else {
            console.error(type + 'Error: ' + Utils.fmt(message, args));
        }
    };

    /**
     * Changes the type of a value to int, float or bool
     * @param {*} value
     * @param {string} type - 'integer', 'double', 'boolean' or anything else (passthrough)
     * @param {boolean} [boolAsInt=false] - return 0 or 1 for booleans
     * @returns {*}
     */
    Utils.changeType = function (value, type, boolAsInt) {
        switch (type) {
            // @formatter:off
            case 'integer': return parseInt(value);
            case 'double': return parseFloat(value);
            case 'boolean':
                var bool = value.trim().toLowerCase() === 'true' || value.trim() === '1' || value === 1;
                return boolAsInt ? (bool ? 1 : 0) : bool;
            default: return value;
            // @formatter:on
        }
    };

    /**
     * Escapes a string like PHP's mysql_real_escape_string does
     * @param {string} value
     * @returns {string}
     */
    Utils.escapeString = function (value) {
        if (typeof value != 'string') {
            return value;
        }

        return value
            .replace(/[\0\n\r\b\\\'\"]/g, function (s) {
                switch (s) {
                    // @formatter:off
                    case '\0': return '\\0';
                    case '\n': return '\\n';
                    case '\r': return '\\r';
                    case '\b': return '\\b';
                    default: return '\\' + s;
                    // @formatter:off
                }
            })
            // uglify compliant
            .replace(/\t/g, '\\t')
            .replace(/\x1a/g, '\\Z');
    };

    /**
     * Escapes a string for use in regex
     * @param {string} str
     * @returns {string}
     */
    Utils.escapeRegExp = function (str) {
        return str.replace(/[\-\[\]\/\{\}\(\)\*\+\?\.\\\^\$\|]/g, '\\$&');
    };

    /**
     * Escapes a string for use in HTML element id
     * @param {string} str
     * @returns {string}
     */
    Utils.escapeElementId = function (str) {
        // Regex based on that suggested by:
        // https://learn.jquery.com/using-jquery-core/faq/how-do-i-select-an-element-by-an-id-that-has-characters-used-in-css-notation/
        // - escapes : . [ ] ,
        // - avoids escaping already escaped values
        return (str) ? str.replace(/(\\)?([:.\[\],])/g,
            function ($0, $1, $2) { return $1 ? $0 : '\\' + $2; }) : str;
    };

    /**
     * Sorts objects by grouping them by `key`, preserving initial order when possible
     * @param {object[]} items
     * @param {string} key
     * @returns {object[]}
     */
    Utils.groupSort = function (items, key) {
        var optgroups = [];
        var newItems = [];

        items.forEach(function (item) {
            var idx;

            if (item[key]) {
                idx = optgroups.lastIndexOf(item[key]);

                if (idx == -1) {
                    idx = optgroups.length;
                }
                else {
                    idx++;
                }
            }
            else {
                idx = optgroups.length;
            }

            optgroups.splice(idx, 0, item[key]);
            newItems.splice(idx, 0, item);
        });

        return newItems;
    };

    /**
     * Defines properties on an Node prototype with getter and setter.<br>
     *     Update events are emitted in the setter through root Model (if any).<br>
     *     The object must have a `__` object, non enumerable property to store values.
     * @param {function} obj
     * @param {string[]} fields
     */
    Utils.defineModelProperties = function (obj, fields) {
        fields.forEach(function (field) {
            Object.defineProperty(obj.prototype, field, {
                enumerable: true,
                get: function () {
                    return this.__[field];
                },
                set: function (value) {
                    var previousValue = (this.__[field] !== null && typeof this.__[field] == 'object') ?
                        $.extend({}, this.__[field]) :
                        this.__[field];

                    this.__[field] = value;

                    if (this.model !== null) {
                        /**
                         * After a value of the model changed
                         * @event model:update
                         * @memberof Model
                         * @param {Node} node
                         * @param {string} field
                         * @param {*} value
                         * @param {*} previousValue
                         */
                        this.model.trigger('update', this, field, value, previousValue);
                    }
                }
            });
        });
    };

    // define Utils locally end

}(window.jQuery, window.pbaAPI, window));