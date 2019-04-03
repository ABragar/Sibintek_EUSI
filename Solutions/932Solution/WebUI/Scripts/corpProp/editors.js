window.corpProp = window.corpProp || {};
(function () {
    'use strict';
    //--------------------------------------------------------------------------------------
    corpProp.dv = corpProp.dv || {};
    corpProp.dv.editors = corpProp.dv.editors || {};
    corpProp.dv.editors.onChange = corpProp.dv.editors.onChange || {};
    corpProp.dv.wnd = corpProp.dv.wnd || {};

    corpProp.dv.editors.onChange.Appraisal_Executor = function (form, isChange) {
        var model = form.getModel();
        var val = false;
        if (form.getPr('Executor')) {
            form.enableEditor('Executor', true);
            var executor = form.getPr('Executor');
            if (executor.ID && isChange) {
                pbaAPI.proxyclient.crud.get({
                    mnemonic: 'SibUser',
                    id: executor.ID
                }).done(
                    function (profile) {
                        if (profile)
                            if (profile.model) {
                                form.setPr('ExecutorLastName', profile.model.LastName);
                                form.setPr('ExecutorFirstName', profile.model.FirstName);
                                form.setPr('ExecutorMiddleName', profile.model.MiddleName);
                                form.setPr('ExecutorDeptName', profile.model.DeptName);
                                form.setPr('ExecutorPhone', profile.model.Phone);
                                form.setPr('ExecutorEmail', profile.model.Email);
                                form.setPr('ExecutorMobile', profile.model.Mobile);
                                //form.setPr('ExecutorPostName', profile.model.PostName);
                            }
                    });
            }
        }
        else {
            val = true;
        }

        form.enableEditor('ExecutorLastName', val);
        form.enableEditor('ExecutorFirstName', val);
        form.enableEditor('ExecutorMiddleName', val);
        form.enableEditor('ExecutorDeptName', val);
        form.enableEditor('ExecutorPhone', val);
        form.enableEditor('ExecutorEmail', val);
        form.enableEditor('ExecutorMobile', val);
        form.enableEditor('ExecutorPostName', val);

    }


    corpProp.dv.editors.onChange.Appraisal_ExecutorInfo = function (form) {
        var model = form.getModel();
        if (form.getPr('Executor')) {
            form.enableEditor('Executor', true);
        }
        else {
            if (form.getPr('ExecutorLastName') || form.getPr('ExecutorFirstName') || form.getPr('ExecutorMiddleName'))
                form.enableEditor('Executor', false);
            else
                form.enableEditor('Executor', true);
        }

    }


    corpProp.dv.editors.onChange.SSRR_AccountingObject = function (form, isChange) {
        var model = form.getModel();
        var val = false;
        if (form.getPr('AccountingObject')) {
            var obj = form.getPr('AccountingObject');
            if (obj.ID) {
                pbaAPI.proxyclient.crud.get({
                    mnemonic: 'AccountingObject',
                    id: obj.ID
                }).done(
                    function (accObj) {
                        if (accObj)
                            if (accObj.model) {
                                form.setPr('Owner', accObj.model.Owner);
                                form.setPr('ObjectName', accObj.model.Name);
                                form.setPr('Location', accObj.model.Address);
                                form.setPr('SystemNumber', accObj.model.ExternalID);
                                form.setPr('InventoryNumber', accObj.model.InventoryNumber);
                                form.setPr('InitialCost', accObj.model.InitialCost);
                            }
                    });
            }
            else val = true;
        }
        else {
            val = true;
            if (model.ScheduleStateRegistration) {
                pbaAPI.proxyclient.crud.get({
                    mnemonic: 'ScheduleStateRegistration',
                    id: model.ScheduleStateRegistration.ID
                }).done(
                    function (SSR) {
                        if (SSR)
                            if (SSR.model) {
                                if (SSR.model.ScheduleStateRegistrationStatus
                                    && (SSR.model.ScheduleStateRegistrationStatus.Code !== "102"
                                        && SSR.model.ScheduleStateRegistrationStatus.Code !== "106")) {
                                    corpProp.dv.editors.onChange.SSRR_EnableFields(form, true);
                                }
                                else
                                    corpProp.dv.editors.onChange.SSRR_EnableFields(form, false);

                            }
                    });
                return;
            }
        }
        corpProp.dv.editors.onChange.SSRR_EnableFields(form, val);
    }

    corpProp.dv.editors.onChange.SSRR_RegistrationBasis = function (form, isChange)
    {
        var model = form.getModel();
        if (model.RegistrationBasis) {
            pbaAPI.proxyclient.crud.get({
                mnemonic: 'RegistrationBasis',
                id: model.RegistrationBasis.ID
            }).done(
                function (SSRRegistrationBasis) {
                    if (SSRRegistrationBasis)
                        if (SSRRegistrationBasis.model) {
                            if (SSRRegistrationBasis.model.Code == "100") {
                                corpProp.dv.editors.onChange.SSRR_EnableFields(form, false);
                                form.requiredEditor('RegistrationBasisNote', true);
                            }
                            else {
                                corpProp.dv.editors.onChange.SSRR_EnableFields(form, true);
                                form.requiredEditor('RegistrationBasisNote', false);
                            }
                        }
                });
        }
    }

    corpProp.dv.editors.onChange.SSRR_EnableFields = function (form, val) {
        form.enableEditor('Owner', val);
        form.enableEditor('ObjectName', val);
        form.enableEditor('Location', val);
        form.enableEditor('SystemNumber', val);
        form.enableEditor('InventoryNumber', val);
        form.enableEditor('InitialCost', val);
    }

    corpProp.dv.editors.onChange.SSRTR_AccountingObject = function (form, isChange) {
        var model = form.getModel();
        var val = false;
        if (form.getPr('AccountingObject')) {
            var obj = form.getPr('AccountingObject');
            if (obj.ID) {
                pbaAPI.proxyclient.crud.get({
                    mnemonic: 'AccountingObject',
                    id: obj.ID
                }).done(
                    function (accObj) {
                        if (accObj)
                            if (accObj.model) {
                                form.setPr('Owner', accObj.model.Owner);
                                form.setPr('ObjectName', accObj.model.Name);
                                form.setPr('Location', accObj.model.Address);
                                form.setPr('SystemNumber', accObj.model.ExternalID);
                                form.setPr('InventoryNumber', accObj.model.InventoryNumber);
                                form.setPr('InitialCost', accObj.model.InitialCost);
                            }
                    });
            }
            else val = true;
        }
        else {
            val = true;
            if (model.ScheduleStateTerminate) {
                pbaAPI.proxyclient.crud.get({
                    mnemonic: 'ScheduleStateTerminate',
                    id: model.ScheduleStateTerminate.ID
                }).done(
                    function (SSR) {
                        if (SSR)
                            if (SSR.model) {
                                if (SSR.model.ScheduleStateRegistrationStatus.Code !== "102" &&
                                    SSR.model.ScheduleStateRegistrationStatus.Code !== "106") {
                                    corpProp.dv.editors.onChange.SSRR_EnableFields(form, true);
                                }
                                else
                                    corpProp.dv.editors.onChange.SSRR_EnableFields(form, false);

                            }
                    });
                return;
            }
        }
        corpProp.dv.editors.onChange.SSRR_EnableFields(form, val);
    }

    corpProp.dv.editors.onChange.NonCoreAsset_SumBudget = function (form, isChange) {
        if (!isChange) return;

        var PublicationExpense = form.getPr('PublicationExpense');
        var AppraisalExpense = form.getPr('AppraisalExpense');
        var BiddingOrganizersBenefits = form.getPr('BiddingOrganizersBenefits');
        var OtherExpenses = form.getPr('OtherExpenses');

        var sum =
            (PublicationExpense ? PublicationExpense : 0) +
            (AppraisalExpense ? AppraisalExpense : 0) +
            (BiddingOrganizersBenefits ? BiddingOrganizersBenefits : 0) +
            (OtherExpenses ? OtherExpenses : 0);

        //form.setPr('BudgetProposedProcedure', sum);  

        var editors = $("div.label-editor-row[data-field='BudgetProposedProcedure']");
        if (editors) {
            var val = editors.find("span[data-field='BudgetProposedProcedure']");
            if (val) {
                val.html(sum);
            }
        }
    }

    corpProp.dv.editors.onChange.ObjectPermission_CheckBox = function (form, isChange) {
        if (!isChange) return;

        var canWrite = form.getPr('AllowWrite');
        var canDelete = form.getPr('AllowDelete');
        var oldVal = form.getPr('AllowRead');
        var newVal = (canWrite || canDelete);
        if (!oldVal && newVal)
            form.setPr('AllowRead', newVal);
    }//end ObjectPermission_CheckBox

    corpProp.dv.editors.onChange.NNARow_State = function (form, isChange) {
        if (!isChange) return;
        var model = form.getModel();
        var state = form.getPr('NonCoreAssetListItemState');
        if (state) {
            if (state.Name.includes("сключен")) {
                form.requiredEditor('NoticeCauk', true);
            }
            else form.requiredEditor('NoticeCauk', false);
        }
        else
            form.requiredEditor('NoticeCauk', false);
    }

    corpProp.dv.editors.onChange.NNA_ChangeIndicateCosts = function (form, isChange) {
        var IndicativeValuationWithoutVAT = form.getPr('IndicativeValuationWithoutVAT');
        var IndicativeValuationIncludingVAT = form.getPr('IndicativeValuationIncludingVAT');
        var IndicativeVAT = form.getPr('IndicativeVAT');

        if (IndicativeValuationWithoutVAT || IndicativeValuationIncludingVAT || IndicativeVAT)
        {

            form.requiredEditor('IndicativeValuationWithoutVAT', true);
            form.requiredEditor('IndicativeValuationIncludingVAT', true);
            form.requiredEditor('IndicativeVAT', true);

            form.setPr('MarketValuationWithoutVAT');
            form.setPr('MarketValuationIncludingVAT');
            form.setPr('MarketValuationVAT');

            form.requiredEditor('MarketValuationWithoutVAT', false);
            form.requiredEditor('MarketValuationIncludingVAT', false);
            form.requiredEditor('MarketValuationVAT', false);
        }

    }

    corpProp.dv.editors.onChange.NNA_ChangeMarketCosts = function (form, isChange) {

        var MarketValuationWithoutVAT = form.getPr('MarketValuationWithoutVAT');
        var MarketValuationIncludingVAT = form.getPr('MarketValuationIncludingVAT');
        var MarketValuationVAT = form.getPr('MarketValuationVAT');

        if (MarketValuationWithoutVAT || MarketValuationIncludingVAT || MarketValuationVAT)
        {
            form.requiredEditor('MarketValuationWithoutVAT', true);
            form.requiredEditor('MarketValuationIncludingVAT', true);
            form.requiredEditor('MarketValuationVAT', true);

            form.setPr('IndicativeValuationWithoutVAT');
            form.setPr('IndicativeValuationIncludingVAT');
            form.setPr('IndicativeVAT');

            form.requiredEditor('IndicativeValuationWithoutVAT', false);
            form.requiredEditor('IndicativeValuationIncludingVAT', false);
            form.requiredEditor('IndicativeVAT', false);

        }

    }

    //--------------------------------------------------------------------------------------
    corpProp.dv.editors.addAuditInfo = function (form, diffItems) {
        var tooltips = $(".sib-audit-property-info-tooltip");
        tooltips.empty();
        if (diffItems) {
            for (var i = 0; i < diffItems.length; i++) {
                var item = diffItems[i];
                var auditProperty = $("div#AuditPropertyInfo_" + item.Property);
                var tooltip = auditProperty.find(".sib-audit-property-info-tooltip");
                if (auditProperty) {
                    var html =
                        "<div class='sib-audit-property-info-item'>" +
                        "<div class='sib-audit-property-info-user'>{date} {user}:</div>" +
                        "<div class='sib-audit-property-info-value'>{oldValue} -> {newValue}</div>" +
                        "</div>";
                    html = html.replace("{date}", item.Date);
                    html = html.replace("{user}", item.UserName);
                    html = html.replace("{oldValue}", item.OldValue);
                    html = html.replace("{newValue}", item.NewValue);
                    tooltip.append(html);
                    auditProperty.removeClass('sib-hidden');
                }
            }
        }
    }

    //--------------------------------------------------------------------------------------

    corpProp.dv.wnd.saveWnd = function (mnemonic, alterMnemonic, id) {
        if (id === 0)
            return;

        var storage = sessionStorage.getItem("openedObjects");
        var openedObjects = JSON.parse(storage) || [];
        var order = openedObjects.length + 1;

        var itemObj = {
            Mnemonic: mnemonic,
            AlterMnemonic: alterMnemonic,
            Id: id,
            Order: order,
            TabIndex: 0,
            IsOpen: true
        };

        openedObjects.push(itemObj);
        sessionStorage.setItem("openedObjects", JSON.stringify(openedObjects));
    };

    corpProp.dv.wnd.testWnd = function (mnemonic, alterMnemonic, order) {
        var storage = sessionStorage.getItem("openedObjects");
        var openedObjects = JSON.parse(storage) || [];

        var el = openedObjects.find(function (o) {
            if(order)
                return (o.Mnemonic === mnemonic || o.AlterMnemonic === mnemonic) && o.IsOpen && o.Order === order;
            else
                return (o.Mnemonic === mnemonic || o.AlterMnemonic === mnemonic) && o.IsOpen;
        });

        if (el && el.Order <= openedObjects.length) {
            return true;
        }
        else
            return false;
    };

    corpProp.dv.wnd.openWnd = function (mnemonic) {
        var storage = sessionStorage.getItem("openedObjects");
        var openedObjects = JSON.parse(storage) || [];
        var data = [];
        if (openedObjects.length === 0)
            return;
        
        var sorted = corpProp.dv.wnd.sort("Order", openedObjects);

        sorted.forEach(function (item, index, arr) {
            setTimeout(function () {
                if (item.IsOpen)
                    return;

                item.IsOpen = true;
                data.push(item);
                pbaAPI.openDetailView(item.Mnemonic, {
                    id: item.Id,
                    callback: function () {
                        corpProp.dv.wnd.delWnd(item.Mnemonic);
                    }
                });
                sessionStorage.setItem("openedObjects", JSON.stringify(data));
            }, index * 1000);
        });
    }

    corpProp.dv.wnd.delWnd = function (mnemonic) {
        var storage = sessionStorage.getItem("openedObjects");
        var openedObjects = JSON.parse(storage) || [];
        var order = openedObjects.length;
        var item = openedObjects.find(function(o) {
            return o.Mnemonic === mnemonic || o.AlterMnemonic === mnemonic && o.Order === order;
        });

        var data = $.grep(openedObjects,
            function(o) {
                return o.Order !== item.Order ||
                    (o.Mnemonic !== item.Mnemonic && o.AlterMnemonic !== item.AlterMnemonic) ||
                    o.Id !== item.Id;
            });

        sessionStorage.setItem("openedObjects", JSON.stringify(data));
    };

    corpProp.dv.wnd.setWndStatus = function () {
        var storage = sessionStorage.getItem("openedObjects");
        var openedObjects = JSON.parse(storage) || [];
        var data = [];

        if (openedObjects.length === 0)
            return;

        openedObjects.forEach(function(item, index, arr) {
            item.IsOpen = false;
            data.push(item);
        });

        sessionStorage.setItem("openedObjects", JSON.stringify(data));
    };

    corpProp.dv.wnd.setTabIndex = function (form, tabIndex) {
        var storage = sessionStorage.getItem("openedObjects");
        var openedObjects = JSON.parse(storage) || [];
        var dialog = form.element.closest(".dialog").data("dialogVM");
        var item = openedObjects.find(function (o) {
            return (o.Mnemonic === dialog.mnemonic || o.AlterMnemonic !== dialog.mnemonic) &&
                o.Id === dialog.params.currentID &&
                o.Order === openedObjects.length;
        });

        if (item) {
            var data = $.grep(openedObjects, function (o) {
                return o.Order !== item.Order || (o.Mnemonic !== item.Mnemonic && o.AlterMnemonic !== item.Mnemonic) || o.Id !== item.Id;
            });
        
            item.TabIndex = tabIndex;
            data.push(item);
            sessionStorage.setItem("openedObjects", JSON.stringify(data));
        }

    };

    corpProp.dv.wnd.selectTab = function (form) {
        var storage = sessionStorage.getItem("openedObjects");
        var openedObjects = JSON.parse(storage) || [];
        var $form = form.element;
        var tabStrip = $form.find("[data-role=tabstrip]").data("kendoTabStrip");
        var formMnemonic = form.element.closest(".dialog").data("dialogVM").mnemonic;

        var item = openedObjects.find(function (o) {
            return (o.Mnemonic === formMnemonic) && o.Id === form.getModel().ID; // && o.Order === openedObjects.length
        });

        if (tabStrip && item)
            tabStrip.select(item.TabIndex);
    };

    corpProp.dv.wnd.sort = function (prop, arr) {
        prop = prop.split('.');
        var len = prop.length;

        arr.sort(function (a, b) {
            var i = 0;
            while (i < len) {
                a = a[prop[i]];
                b = b[prop[i]];
                i++;
            }
            if (a < b) {
                return -1;
            } else if (a > b) {
                return 1;
            } else {
                return 0;
            }
        });
        return arr;
    };

}());

/**
 * Events
 */
$(".k-animation-container input[data-role=autocomplete].k-textbox.tooltipstered").live("blur",
    function(event) {
        var $element = $(event.srcElement);
        if ($element) {
            $element.closest("li").next().hide();
        }
    });