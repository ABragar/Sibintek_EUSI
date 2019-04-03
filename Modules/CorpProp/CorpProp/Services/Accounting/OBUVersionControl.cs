using Base.DAL;
using Base.Extensions;
using CorpProp.Common;
using CorpProp.Entities.Accounting;
using CorpProp.Entities.Import;
using CorpProp.Helpers;
using CorpProp.Helpers.Import.Extentions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CorpProp.Services.Accounting
{
    /// <summary>
    /// Представляет управление историей и версионностью ОБУ.
    /// </summary>
    public class OBUVersionControl : BaseVersionControl<AccountingObject>
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса OBUVersionControl.
        /// </summary>
        public OBUVersionControl(
            IUnitOfWork _uow
            ,DataTable _table
            ,Dictionary<string, string> _colsNameMapping
            ,DateTime _period
            ,ref ImportHistory history
            ) : base(_uow, _table, _colsNameMapping, _period, ref history)
        {
           
        }

        /// <summary>
        /// Переопределяет наполнение ОБУ данными импорта.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="obj"></param>
        /// <param name="history"></param>
        protected override void Fill(DataRow row, ref AccountingObject obj, ref ImportHistory history)
        {
            base.Fill(row, ref obj, ref history);
            //Признаки недвижимого имущества
            if (obj.IsRealEstateImpl == true)
                obj.IsRealEstate = true;
            else
                obj.IsRealEstate = false;
            //владелец
            if (obj.MainOwner == null)
                obj.MainOwner = obj.Owner;
            return;
        }

        /// <summary>
        /// Переопределяет создание старой версии истории для ОБУ.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="obj"></param>
        /// <param name="history"></param>
        protected override void CreateOldVersion(DataRow row, ref AccountingObject obj, ref ImportHistory history)
        {
            var estate = obj.Estate;
            var estateID = obj.EstateID;
            base.CreateOldVersion(row, ref obj, ref history);
            obj.Estate = estate;
            obj.EstateID = estateID;
        }

        /// <summary>
        /// Переопределяет выполение контроля версий.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="obj"></param>
        /// <param name="history"></param>
        public override void Execute(DataRow row, ref AccountingObject obj, ref ImportHistory history)
        {
            if (obj.ID == 0)
            {
                Fill(row, ref obj, ref history);
                obj.ActualDate = StartPeriod;
                obj.NonActualDate = EndPeriod;
            }
            else if (obj.ActualDate == StartPeriod)
                UpdateVersion(row, ref obj, ref history);
            else if (obj.ActualDate < StartPeriod)
                CreateNewVersion(row, ref obj, ref history);
            else if (obj.ActualDate > StartPeriod)
            {
                if (obj.StateObjectRSBUID != null)
                {
                    var stateID = obj.StateObjectRSBUID.Value;
                    var code = Uow.GetRepository<Entities.NSI.StateObjectRSBU>()
                        .FilterAsNoTracking(f => f.ID == stateID)
                        .FirstOrDefault()?.Code;
                    if (!String.IsNullOrEmpty(code) && (code.ToLower() == "draft" || code.ToLower() == "outbus"))
                    {
                        UpdateVersion(row, ref obj, ref history);
                        
                        var newCode = obj.StateObjectRSBU?.Code;
                        if (String.IsNullOrEmpty(newCode) || 
                            (!String.IsNullOrEmpty(newCode) && (code.ToLower() == "draft" || code.ToLower() == "outbus")))
                        {
                            obj.StateObjectRSBU = null;
                            obj.StateObjectRSBUID = null;
                        }
                    }
                    else
                        CreateOldVersion(row, ref obj, ref history);
                }
                else
                    CreateOldVersion(row, ref obj, ref history);
            }
            return;
        }
    }
}
