function clean(obj) {
    var propNames = Object.getOwnPropertyNames(obj);
    for (var i = 0; i < propNames.length; i++) {
        var propName = propNames[i];
        if (obj[propName] === null || obj[propName] === undefined) {
            delete obj[propName];
        }
    }
}

function setEntityByID(obj) {
    var result = {};
    var propNames = Object.getOwnPropertyNames(obj);
    for (var i = 0; i < propNames.length; i++) {
        var propName = propNames[i];
        if (obj[propName] !== null || (obj[propName] !== undefined && obj[propName] !== null)) {
            var id = "ID";
            if (propName.endsWith(id) && propName !== id) {
                var entityProp = propName.substring(0, propName.length - id.length);
                if (result[entityProp] == null || obj[propName] == undefined)
                    result[entityProp] = { "id": obj[propName] };
            }
        }
    }
    return result;
}

function otmLinkRow(grid, mnemonic, title) {
    var props = grid.composite.params.initProps;
    pbaAPI.openModalDialog(mnemonic,
        function (res) {
            var selectedObject = res[0];
            clean(props);
            clean(selectedObject);
            delete props.ID;
            var propsWithFakeEntitiesForSave = setEntityByID(props);
            var save = Object.assign({}, selectedObject, propsWithFakeEntitiesForSave);
            delete save.ID;
            delete save.RowVersion;
            pbaAPI.proxyclient.crud.patch({ mnemonic: mnemonic, id: save.id }, { model: save })
                .done(function (doneRes) {
                    if (doneRes.error && doneRes.error != 0) {
                        if (!res["message"])
                            pbaAPI.uploadMsg(res.message);
                    } else {
                        grid.widget().dataSource.pushCreate(res[0]);
                    }
                });
        },
        {
            title: "ВЫБОР - " + title,
            multiselect: false,
            //filter: getSysFilter
        }
    );
}

function getIDFieldsForSave(obj) {
    var unlinkedIds = {};
    var propNames = Object.getOwnPropertyNames(obj);
    for (var i = 0; i < propNames.length; i++) {
        var propName = propNames[i];
        if (obj[propName] !== null || (obj[propName] !== undefined && obj[propName] !== null)) {
            var id = "ID";
            if (propName.endsWith(id) && propName !== id) {
                var entityProp = propName.substring(0, propName.length - id.length);
                unlinkedIds[entityProp] = null;
                unlinkedIds[propName] = null;
            }
        }
    }
    return unlinkedIds;
}


function otmUnlinkRow(grid, mnemonic, title) {
    var props = grid.composite.params.initProps;
    var id = grid.selectID();
    var selectedItem = grid.dataItem(grid.select());
    if (selectedItem) {
        pbaAPI.confirm(title,
            "Отвязать?",
            function () {
                var save = getIDFieldsForSave(props);
                save.ID = selectedItem.ID;
                pbaAPI.proxyclient.crud.patch({ mnemonic: mnemonic, id: id }, { model: save }).done(function (res) {
                    if (res.error === 0 || !res["error"]) {
                        grid.removeRow(grid.select());
                        if (!res["message"])
                            pbaAPI.uploadMsg(res.message);
                    } else {
                        pbaAPI.errorMsg(res.message);
                    }
                });
                ;
            });
    }
}