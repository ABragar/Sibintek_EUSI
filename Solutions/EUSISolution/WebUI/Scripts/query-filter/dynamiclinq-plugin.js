$(function () {
    $.fn.queryBuilder.defaults({
        dynamiclinqOperators: {
            equal: { op: '@ = ?' },
            not_equal: { op: '@ != ?' },
            in: { op: '(@ in(?))', sep: ',' },
            //not_in: { op: 'NOT IN(?)', sep: ', ' },
            less: { op: '@ < ?' },
            less_or_equal: { op: '@ <= ?' },
            greater: { op: '@ > ?' },
            greater_or_equal: { op: '@ >= ?' },
            between: { op: '@ BETWEEN(?)', sep: ' AND ' },
            //between_exclude: { op: '@ BETWEEN_EXCLUDE(?)', sep: ' AND ', optgroup: 'custom'},
            not_between: { op: '@ NOT_BETWEEN(?)', sep: ' AND ' },
            //not_between_exclude: { op: '@ NOT_BETWEEN_EXCLUDE(?)', sep: ' AND ', optgroup: 'custom' },
            //begins_with: { op: 'LIKE(?)', mod: '{0}%' },
            //not_begins_with: { op: 'NOT LIKE(?)', mod: '{0}%' },
            contains: { op: '@.Contains(?)' },
            not_contains: { op: '!@.Contains(?)', mod: '%{0}%' },
            //ends_with: { op: 'LIKE(?)', mod: '%{0}' },
            //not_ends_with: { op: 'NOT LIKE(?)', mod: '%{0}' },
            //is_empty: { op: '= \'\'' },
            //is_not_empty: { op: '!= \'\'' },
            is_null: { op: '@ = null' },
            is_not_null: { op: '@ != null' },
            any: { op: '@.Any(it.ObjectID in (?))', sep: ',' }
        }
    });

    // jquery-builder.ru.js does not contain not_between translation
    $.fn.queryBuilder.regional["ru"]["operators"]["not_between"] = "не между";

    $.fn.queryBuilder.extend({
        getDynamiclinq: function (data) {
            var separator = ' ';
            data = (data === undefined) ? this.getRules() : data;
            var self = this;
            return (function parse(group) {
                if (!group.condition) {
                    group.condition = self.settings.default_condition;
                }
                if (['AND', 'OR'].indexOf(group.condition.toUpperCase()) === -1) {
                    alert('error');
                }
                if (!group.rules) {
                    return '';
                }
                var parts = [];
                var partsLabel = [];
                group.rules.forEach(function (rule) {
                    if (rule.rules && rule.rules.length > 0) {
                        var tt = parse(rule);
                        if (tt.query) {
                            parts.push('(' + separator + tt.query + separator + ')' + separator);
                            partsLabel.push('(' + separator + tt.text + separator + ')' + separator);
                        }
                        else {
                            parts.push('(' + separator + tt + separator + ')' + separator);
                            partsLabel.push('(' + separator + tt + separator + ')' + separator);
                        }

                    }
                    else {
                        var sql = self.settings.dynamiclinqOperators[rule.operator];
                        var ope = self.getOperatorByType(rule.operator);
                        var value = '';

                        if (sql === undefined) {
                            alert('Unknown Dynamic LINQ operation ' + rule.operator);
                        }

                        if (ope.nb_inputs !== 0) {
                            if (!(rule.value instanceof Array)) {
                                if (rule.operator === 'in') {
                                    rule.value = rule.value.split(',');
                                }
                                else {
                                    rule.value = [rule.value];
                                }
                            }
                            var builder = self;
                            rule.value.forEach(function (v, i) {
                                if (i > 0) {
                                    value += sql.sep;
                                }

                                if (rule.data.system_type === 'Enum') {
                                    rule.field = 'Int32(' + rule.field + ')';
                                    rule.id = 'Int32(' + rule.id + ')';

                                } else if (rule.data.system_type === 'BaseObjectOne') {
                                    if (v != null) {
                                        rule.field = rule.field + '.ID';
                                        rule.id = rule.id + '.ID';
                                    }
                                } else {
                                    if (rule.type === 'string') {
                                        v = '"' + v + '"';
                                    }
                                }
                                value += v;
                            });
                        }
                        var sqlFn = function (v, field) {
                            var res = sql.op.replace(/\?/g, v).replace(/\@/g, field);
                            return res;
                        };
                        var field = self.change('getSQLField', rule.field, rule);
                        var fcopy = field;
                        if (fcopy.indexOf('.') > 0) {
                            var ff = fcopy.split('.');
                            fcopy = ff[0];
                        }
                        var arr = self.filters.filter(function (f) { return f.field === fcopy; });
                        var label = '[' + fcopy + ']';
                        if (arr) {
                            if (arr.length > 0) {
                                if (arr[0].label) {
                                    label = '[' + arr[0].label + ']';
                                }
                            }
                        }
                        var ruleExpression = sqlFn(value, field);
                        var ruleText = sqlFn(value, label);
                        parts.push(self.change('ruleToSQL', ruleExpression, rule, value, sqlFn));
                        partsLabel.push(self.change('ruleToSQL', ruleText, rule, value, sqlFn));
                    }
                });
                var groupExpression = parts.join(separator + group.condition + separator);
                var groupText = partsLabel.join(separator + group.condition + separator);
                var res = {};
                res.query = self.change('groupToSQL', groupExpression, group);
                res.query = res.query.replace(/ AND /g, ' and ').replace(/ OR /g, ' or ');
                res.text = self.change('groupToSQL', groupText, group);
                res.text = res.text.replace(/ AND /g, ' И ').replace(/ OR /g, ' ИЛИ ')
                    .replace(/ BETWEEN/g, ' МЕЖДУ').replace(/ NOT_BETWEEN/g, ' НЕ МЕЖДУ');
                return res;
            }(data));
        },
        getRulesFromDynamiclinq: function (query) {
            var result = query;
            var notContainsRegEx = /(!)\w+(\.Contains\(").*?("\))/g;
            var match = notContainsRegEx.exec(result);
            while (match != null) {
                var founded = match[0];
                var final = founded.replace(match[1], '')
                    .replace(match[2], ' NOT LIKE(\'%')
                    .replace(match[3], '%\')');
                result = result.replace(founded, final);
                match = notContainsRegEx.exec(result);
            }
            var containsRegEx = /\w+(\.Contains\(").*?("\))/g;
            var match = containsRegEx.exec(result);
            while (match != null) {
                var founded = match[0];
                var final = founded
                    .replace(match[1], ' LIKE(\'%')
                    .replace(match[2], '%\')');
                result = result.replace(founded, final);
                match = containsRegEx.exec(result);
            }

            var isNullRegEx = /\w+( = null)/g;
            var match = isNullRegEx.exec(result);
            while (match != null) {
                var founded = match[0];
                var final = founded.replace(match[1], ' IS NULL ');
                result = result.replace(founded, final);
                match = isNullRegEx.exec(result);
            }

            var isNotNullRegEx = /\w+( != null)/g;
            var match = isNotNullRegEx.exec(result);
            while (match != null) {
                var founded = match[0];
                var final = founded.replace(match[1], ' IS NOT NULL ');
                result = result.replace(founded, final);
                match = isNotNullRegEx.exec(result);
            }
            var dateTimeRegEx = /(?!(\'|\,))DateTime\(.*?\)/g;
            var match = dateTimeRegEx.exec(result);
            if (match != null) {
                result = result.replace(dateTimeRegEx, "\'$&\'");
            }

            var enumRegEx = /Int32\((.*?)\)/g;
            var match = enumRegEx.exec(result);
            while (match != null) {
                var founded = match[0];
                result = result.replace(founded, match[1]);
                match = enumRegEx.exec(result);
            }

            var anyRegex = /(\.Any)\(it.ObjectID in \((\d+(,\d+)*)\)\)/g;
            var anyMatch = anyRegex.exec(result);
            while (anyMatch != null) {
                var founded = anyMatch[0];
                var numbers = anyMatch[2];
                result = result.replace(founded, " ANY(" + numbers + ")");
                anyMatch = anyRegex.exec(result);
            }

            var notBetweenRegex = /(.*)(NOT_BETWEEN)(\()(\'DateTime\(.*?\)\' and \'DateTime\(.*?\)\')(\))/g;
            var notBetweenMatch = notBetweenRegex.exec(result);
            while (notBetweenMatch != null) {
                var found = notBetweenMatch[2];
                result = result.replace(found, "NOT BETWEEN");
                notBetweenMatch = notBetweenRegex.exec(result);
            }

            var betweenRegex = /(.*)(BETWEEN)(\()(\'DateTime\(.*?\)\' and \'DateTime\(.*?\)\')(\))/g;
            var betweenMatch = betweenRegex.exec(result);
            while (betweenMatch != null) {
                result = result.replace(betweenRegex, "$1$2 $4");
                betweenMatch = betweenRegex.exec(result);
            }

            //for base object one
            result = result.replace(/\.ID /g, ' ');

            //or -> OR, and -> AND
            result = result.replace(/ and /g, ' AND ').replace(/ or /g, ' OR ');

            return this.getRulesFromSQL(result);
        }
    });
});