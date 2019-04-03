pbaAPI.registerEditor('Address', pbaAPI.Editor.extend({
    address: {},
    original: {},
    country: {},

    init: function ($wrap, propertyName) {
        'use strict';

        pbaAPI.Editor.fn.init.call(this, $wrap, propertyName);

        this.wrap.find('[data-remove-modal]').click(this.removeModal.bind(this));
        this.wrap.find('[data-open-modal]').click(this.openModal.bind(this));

        //console.log('kladr', this);
    },

    /**
     * @override
     */
    onAfterBind: function () {
        this.original = this.readProperty();
        this.address = $.extend({}, this.original);
        this.country = {
            Title: this.address.CountryTitle,
            NumericCode: this.CountryNumericCode
        };

        this.wrap.find('input').val(this.address.Title);
    },

    /**
     * @override
     */
    onSave: function () {
        'use strict';
        this.writeProperty(this.address);
    },

    // CUSTOM LOGIC
    removeModal: function () {
        'use strict';

        this.wrap.find('input').val('');

        this.country.Title = null;

        this.address = $.extend({}, this.original);

        this.address.Title = '';
        this.address.Region.Name = '';
        this.address.District.Name = '';
        this.address.City.Name = '';
        this.address.Street.Name = '';
        this.address.Building.Name = '';
        this.address.Apartment = '';


        this.address.CountryNumericCode = 643;
        this.address.CountryTitle = 'Россия';

        var editor = this;

        $.kladr.getAddress($('#kladr_' + this.wrapId), function (objs) {
            var setValues = function (prop, Prop) {
                if (objs[prop]) {
                    editor.address[Prop] = {
                        ID: objs[prop].id,
                        ContentType: objs[prop].contentType,
                        Name: objs[prop].name,
                        TypeShort: objs[prop].typeShort
                    };
                } else {
                    editor.address[Prop] = {
                        ID: null,
                        ContentType: null,
                        Name: null,
                        TypeShort: null
                    };
                }
            };

            setValues('region', 'Region');
            setValues('district', 'District');
            setValues('city', 'City');
            setValues('street', 'Street');
            setValues('building', 'Building');
            setValues('apartment', 'Apartment');
        });

        $('#validation_'+ this.wrapId).attr('value', '');
    },

    openModal: function () {
        'use strict';
        var editor = this;

        var wnd = $('<div>', { id: 'wnd_' + editor.wrapId }).addClass('view-model-window').kendoWindow({
            width: 1200,
            height: 800,
            maxHeight: 900,
            content: {
                template: $('#kladr-template_' + editor.wrapId).html()
            },
            title: 'Карта',
            actions: ['Maximize', 'Close'],
            modal: true,
            deactivate: function () {
                this.destroy();
            },
            refresh: function () {
                var map = null;
                var mapCreated = false;

                var $kladr = $('#kladr_' + editor.wrapId),
                    $region = $kladr.find('[name="region"]'),
                    $district = $kladr.find('[name="district"]'),
                    $city = $kladr.find('[name="city"]'),
                    $street = $kladr.find('[name="street"]'),
                    $building = $kladr.find('[name="building"]'),
                    $apartment = $kladr.find('[name="apartment"]');

                function log(obj) {
                    var $log, i;

                    $('.js-log li').hide();

                    for (i in obj) {
                        if (obj.hasOwnProperty(i)) {
                            $log = $('#' + i);

                            if ($log.length) {
                                $log.find('.value').text(obj[i]);
                                $log.show();
                            }
                        }
                    }
                }

                function createMap() {
                    if (mapCreated) return;

                    if (editor.country.Title === 'Россия' || editor.country.NumericCode === 643) {
                        mapCreated = true;

                        var $map = $('#map_' + editor.wrapId);

                        $map.height($map.closest('.view-model').height());

                        map = new window.ymaps.Map('map_' + editor.wrapId, {
                            center: [55.76, 37.64],
                            zoom: 12,
                            controls: []
                        });

                        map.controls.add('zoomControl', {
                            position: {
                                right: 10,
                                top: 10
                            }
                        });
                    }
                }

                function addressUpdate() {
                    var address = $.kladr.getAddress($kladr.find('.js-form-address'));
                    $kladr.find('#address_' + editor.wrapId).text(address);
                }

                function setLabel($input, text) {
                    text = text.charAt(0).toUpperCase() + text.substr(1).toLowerCase();
                    $input.parent().find('label').text(text);
                }

                function mapUpdate() {
                    var zoom = 4;

                    var address = $.kladr.getAddress($kladr.find('.js-form-address'), function (objs) {
                        var result = '',
                            name = '',
                            type = '';
                        for (var i in objs) {
                            if (objs.hasOwnProperty(i)) {
                                if ($.type(objs[i]) === 'object') {
                                    name = objs[i].name;
                                    type = ' ' + objs[i].typeShort;
                                }
                                else {
                                    name = objs[i];
                                    type = '';
                                }

                                if (result) result += ', ';
                                result += type + name;

                                if (objs[i].contentType) {
                                    switch (objs[i].contentType) {
                                    case $.kladr.type.region:
                                        zoom = 4;
                                        break;

                                    case $.kladr.type.district:
                                        zoom = 7;
                                        break;

                                    case $.kladr.type.city:
                                        zoom = 10;
                                        break;

                                    case $.kladr.type.street:
                                        zoom = 13;
                                        break;

                                    case $.kladr.type.building:
                                        zoom = 16;
                                        break;
                                    }
                                }
                            }
                        }

                        return result;
                    });

                    if (address && mapCreated) {
                        var geocode = window.ymaps.geocode(address);

                        geocode.then(function (res) {
                            if (res && res.geoObjects.get(0) && res.geoObjects.get(0).geometry) {
                                map.geoObjects.each(function(geoObject) {
                                    map.geoObjects.remove(geoObject);
                                });
                                var position = res.geoObjects.get(0).geometry.getCoordinates(),
                                    placemark = new window.ymaps.Placemark(position, {}, {});

                                map.geoObjects.add(placemark);
                                map.setCenter(position, zoom);
                            } else {
                                //clear center ??
                                //map.setCenter(position, zoom);
                            }

                        });
                    }
                }

                if (editor.country.Title == null) {
                    editor.country.Title = 'Россия';
                    editor.country.NumericCode = 643;
                }

                if (editor.country.Title === 'Россия' || editor.country.NumericCode === 643) {
                    $kladr.find('.js-form-address').show();
                } else {
                    $kladr.find('.full-address').show();
                    $kladr.find('.full-address input').val(editor.address.Title);
                }

                $kladr.find('[data-select-country]').click(function () {
                    pbaAPI.openModalDialog('Country', function (data) {
                        if (data[0]) {
                            editor.country.Title = data[0].Title;
                            editor.country.NumericCode = data[0].NumericCode;

                            $kladr.find('[data-country]').val(data[0].Title);

                            if (editor.country.Title === 'Россия' || editor.country.NumericCode === 643) {
                                createMap();
                                $kladr.find('.js-form-address').show();
                                $kladr.find('.full-address').hide();
                                $kladr.find('.full-address input').val('');
                            } else {
                                $kladr.find('.js-form-address').hide();
                                $kladr.find('.full-address').show();
                            }
                        }
                    });
                });

                $kladr.find('[data-clear-country]').click(function () {
                    editor.country = {};
                    $kladr.find('[data-country]').val('');
                    $kladr.find('.js-form-address').hide();
                    $kladr.find('.full-address').show();

                    $kladr.find('.js-form-address input').val('');
                    
                    addressUpdate();

                    $('#' + editor.wrapId + '_address').text('');

                    $('#map_' + editor.wrapId).empty();

                    map = null;
                    mapCreated = false;

                    editor.country.Title = 'Россия';
                    editor.country.NumericCode = 643;
                    $kladr.find('[data-country]').val('Россия');
                    createMap();
                    $kladr.find('.js-form-address').show();
                    $kladr.find('.full-address').hide();
                    $kladr.find('.full-address input').val('');

                });

                $kladr.find('[data-country]').val(editor.country.Title);

                $kladr.find('[data-save]').click(function () {

                    editor.address.Region.Name = $region.val();
                    editor.address.District.Name = $district.val();
                    editor.address.City.Name = $city.val();
                    editor.address.Street.Name = $street.val();
                    editor.address.Building.Name = $building.val();
                    editor.address.Apartment = $apartment.val();

                    editor.address.CountryNumericCode = editor.country.NumericCode;
                    editor.address.CountryTitle = editor.country.Title;

                    var withoutCountry = $kladr.find('.full-address input').val();

                    if (withoutCountry) {
                        editor.address.Title = withoutCountry;
                    } else {
                        editor.address.Title = $kladr.find('#address_' + editor.wrapId).html();
                    }

                    editor.wrap.find('input').val(editor.address.Title);

                    $.kladr.getAddress($kladr, function (objs) {
                        var setValues = function (prop, Prop) {

                            if (objs[prop]) {
                                editor.address[Prop] = {
                                    ID: objs[prop].id,
                                    ContentType: objs[prop].contentType,
                                    Name: objs[prop].name,
                                    TypeShort: objs[prop].typeShort
                                };
                            } else {
                                editor.address[Prop] = {
                                    ID: null,
                                    ContentType: null,
                                    Name: null,
                                    TypeShort: null
                                };
                            }
                        };

                        setValues('region', 'Region');
                        setValues('district', 'District');
                        setValues('city', 'City');
                        setValues('street', 'Street');
                        setValues('building', 'Building');
                    });

                    wnd.close();
                });

                $kladr.find('[data-close]').click(function () {
                    wnd.close();
                });

                $.kladr.setDefault({
                    parentInput: $kladr.find('.js-form-address'),
                    verify: true,
                    labelFormat: function (obj, query) {
                        var label = '';

                        var name = obj.name.toLowerCase();
                        query = query.name.toLowerCase();

                        var start = name.indexOf(query);
                        start = start > 0 ? start : 0;

                        if (obj.typeShort) {
                            label += obj.typeShort + '. ';
                        }
                        if (query.length < obj.name.length) {
                            label += obj.name.substr(0, start);
                            label += '<strong>' + obj.name.substr(start, query.length) + '</strong>';
                            label += obj.name.substr(start + query.length, obj.name.length - query.length - start);
                        } else {
                            label += '<strong>' + obj.name + '</strong>';
                        }
                        if (obj.parents) {
                            for (var k = obj.parents.length - 1; k > -1; k--) {
                                var parent = obj.parents[k];
                                if (parent.name) {
                                    if (label) label += '<small>, </small>';
                                    label += '<small>' + parent.name + ' ' + parent.typeShort + '.</small>';
                                }
                            }
                        }

                        return label;
                    },
                    select: function (obj) {
                        setLabel($('#' + editor.wrapId), obj.type);
                        log(obj);
                        addressUpdate();
                        mapUpdate();
                    },
                    check: function (obj) {
                        if (obj) {
                            setLabel($('#' + editor.wrapId), obj.type);
                        }

                        log(obj);
                        addressUpdate();
                        mapUpdate();
                    }
                });

                $region.kladr('withParents', true);
                $district.kladr('withParents', true);
                $city.kladr('withParents', true);
                $street.kladr('withParents', true);
                $building.kladr('withParents', true);

                $building.kladr('verify', false);

                if (editor.address.Region) {
                    $region.kladr('type', editor.address.Region.contentType || $.kladr.type.region).val(editor.address.Region.Name);

                    $district.kladr('parentType', editor.address.Region.contentType || $.kladr.type.region);
                    $district.kladr('type', editor.address.District.contentType || $.kladr.type.district).val(editor.address.District.Name);

                    $city.kladr('parentType', editor.address.District.contentType || $.kladr.type.$district);
                    $city.kladr('type', editor.address.City.contentType || $.kladr.type.city).val(editor.address.City.Name);

                    $street.kladr('parentType', editor.address.City.contentType || $.kladr.type.city);
                    $street.kladr('type', editor.address.Street.contentType || $.kladr.type.street).val(editor.address.Street.Name);

                    $district.kladr('parentType', editor.address.Street.contentType || $.kladr.type.street);
                    $building.kladr('type', editor.address.Building.contentType || $.kladr.type.building).val(editor.address.Building.Name);
                }

                function setValue(selector, prop) {
                    selector.kladr('controller').setValueByObject({
                        id: prop.ID,
                        typeShort: prop.TypeShort,
                        name: prop.Name,
                        contentType: prop.ContentType
                    });
                }


                window.ymaps.ready(function () {
                    createMap();
                    if (editor.address.Title) {
                        setValue($region, editor.address.Region);
                        setValue($district, editor.address.District);
                        setValue($city, editor.address.City);
                        setValue($street, editor.address.Street);
                        setValue($building, editor.address.Building);
                        $apartment.val(editor.address.Apartment);
                        $kladr.find('#address_' + editor.wrapId).html(editor.address.Title);
                        mapUpdate();
                    }
                });
            }
        }).data('kendoWindow');

        //var wnd = $('#wnd_' + this.wrapId).data('kendoWindow');

        wnd.center().open();

    }
}));
