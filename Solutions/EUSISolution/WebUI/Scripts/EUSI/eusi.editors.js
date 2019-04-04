window.eusi = window.eusi || {};
(function () {
    'use strict';
    //--------------------------------------------------------------------------------------
    eusi.dv = eusi.dv || {};
    eusi.dv.editors = eusi.dv.editors || {};
    eusi.dv.editors.onChange = eusi.dv.editors.onChange || {};

    eusi.dv.editors.onChange.ER_PositionConsolidation = function (form, isChange) {

        if (form.getPr('PositionConsolidation')) {
            var pos = form.getPr('PositionConsolidation');
            if (pos.ID && isChange) {
                pbaAPI.proxyclient.EUSI.getGroupPosition({
                    id: pos.ID
                }).done(
                    function (group) {
                        if (group)
                            if (group) {
                                if (group.Code.startsWith("G")) {
                                    form.setPr('GroupConsolidationMSFO', group);
                                }
                                else {
                                    form.setPr('GroupConsolidationRSBU', group);
                                }
                            }
                    });
            }
        }
        else {
            form.setPr('GroupConsolidationMSFO', null);
            form.setPr('GroupConsolidationRSBU', null);
        }

    }

    //получение и установка страны
    var SetCountry = function(obj, form) {
        {
            if (obj.model) {
                if (obj.model.CountryID &&
                    (!form.getPr('SibCountry') || form.getPr('SibCountry').ID != obj.model.CountryID)) {
                    pbaAPI.proxyclient.crud.get({
                        mnemonic: 'SibCountry',
                        id: obj.model.CountryID
                    }).done(
                        function(country) {
                            if (country) {
                                form.setPr('SibCountry', country.model);
                            };
                        });
                }
            }
        }

    };

    //получение и установка ФО
    var SetFederalDistrict = function (obj, form) {
            if (obj.model.FederalDistrictID &&
            (!form.getPr('SibFederalDistrict') ||
                form.getPr('SibFederalDistrict').ID != obj.model.FederalDistrictID)) {
                pbaAPI.proxyclient.crud.get({
                    mnemonic: 'SibFederalDistrict',
                    id: obj.model.FederalDistrictID
                }).done(
                    function(district) {
                        if (district) {
                            form.setPr('SibFederalDistrict', district.model);
                        }
                    });
            }
    };

    //получение и установка региона
    var SetRegion = function (obj, form) {
            if (obj.model.SibRegionID &&
                (!form.getPr('SibRegion') || form.getPr('SibRegion').ID != obj.model.SibRegionID)) {
                pbaAPI.proxyclient.crud.get({
                    mnemonic: 'SibRegion',
                    id: obj.model.SibRegionID
                }).done(
                    function(region) {
                        if (region) {
                            form.setPr('SibRegion', region.model);
                        }
                    });
            }
    }

    eusi.dv.editors.onChange.ER_ChangeFederalDistrict = function (form, isChange) {
        if (form.getPr('SibFederalDistrict')) {
            var fDistrict = form.getPr('SibFederalDistrict');
            if (fDistrict.ID && isChange) {
                pbaAPI.proxyclient.crud.get({
                    mnemonic: 'SibFederalDistrict',
                    id: fDistrict.ID
                }).done(function (obj) {
                    if (obj.model) {
                        SetCountry(obj, form);
                    }
                });
            }
        }
    }

    eusi.dv.editors.onChange.ER_ChangeRegion = function (form, isChange) {

        if (form.getPr('SibRegion')) {
            var region = form.getPr('SibRegion');
            if (region.ID && isChange) {
                pbaAPI.proxyclient.crud.get({
                    mnemonic: 'SibRegion',
                    id: region.ID
                }).done(function(obj) {
                    if (obj.model) {
                            SetCountry(obj, form);
                            SetFederalDistrict(obj, form);
                    }
                });
            }
        }

    }

    eusi.dv.editors.onChange.ER_ChangeSibCity = function (form, isChange) {

        if (form.getPr('SibCityNSI')) {
            var city = form.getPr('SibCityNSI');
            if (city.ID && isChange) {
                pbaAPI.proxyclient.crud.get({
                    mnemonic: 'SibCityNSI',
                    id: city.ID
                }).done(
                    function (obj) {
                        if (obj.model) {
                            SetCountry(obj, form);
                            SetFederalDistrict(obj, form);
                            SetRegion(obj, form);
                        }
                    });
            }
        }

    }

    eusi.dv.editors.onChange.Estate_EncumbranceExist = function (form, isChange) {
        var EncumbranceExist = form.getPr('EncumbranceExist');
        if (EncumbranceExist == null) {
            return;
        }
        if (EncumbranceExist) {
            form.requiredEditor('EncumbranceContractNumber', true);
        }
        else
            form.requiredEditor('EncumbranceContractNumber', false);
    }

    eusi.dv.editors.onChange.ER_ChangeIKCountry = function (form) { 
        var country = form.getPr("Country");
        if (country &&
            ((country.Code && country.Code.trim().toLowerCase() === "ru")
            || (country.Name && country.Name.trim().toLowerCase() === "россия"))
        )
        {
            form.enableEditor("SibFederalDistrict", true);            
        }  
        
        else {
            clearAndDesableSibFederalDistrict();
        }

        //очистка и отключение редактирования поля "Федеральный округ"
        function clearAndDesableSibFederalDistrict() {
            
            //находим и очищаем поле "Федеральный округ"
            var sibFederalDistrictInput = form.getPr("SibFederalDistrict");
            if (sibFederalDistrictInput) {
                form.setPr("SibFederalDistrict", "");
            }
            //откючаем редактирование
            form.enableEditor("SibFederalDistrict", false);
        }
    }

}());