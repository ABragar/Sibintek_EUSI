using System;
using System.Collections.Generic;
using Base.DAL;
using CorpProp.DefaultData;
using CorpProp.Entities.NSI;
using CorpProp.Helpers;
using EUSI.Entities.Mapping;
using EUSI.Entities.NSI;

namespace EUSI.DefaultData
{
    public static class DefaultDataHelperEx
    {
        public static void CreateERTypeReRiceiptReason(
            this IDefaultDataHelper defaultDataHelper, 
            IUnitOfWork uow,
            List<ERTypeERReceiptReason> items)
        {
            try
            {
                if (items != null)
                {
                    var rep = uow.GetRepository<ERTypeERReceiptReason>();
                    var err = "";
                    foreach (var item in items)
                    {
                        var erType = ImportHelper.GetDictByCode(uow, typeof(EstateRegistrationTypeNSI),
                            item.ERType?.Code, ref err);
                        var erReceiptReason = ImportHelper.GetDictByCode(uow, typeof(ERReceiptReason),
                            item.ERReceiptReason?.Code, ref err);

                        item.ERType = erType as EstateRegistrationTypeNSI;
                        item.ERReceiptReason = erReceiptReason as ERReceiptReason;

                        rep.Create(item);
                    }

                    uow.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.ToString());
            }
        }

        public static void CreateEstateTypeMapping(
            this IDefaultDataHelper defaultDataHelper,
            IUnitOfWork uow,
            List<EstateTypesMapping> items)
        {
            try
            {
                if (items != null)
                {
                    var rep = uow.GetRepository<EstateTypesMapping>();
                    var err = "";
                    foreach (var item in items)
                    {
                        var estateDefinitionType = ImportHelper.GetDictByCode(uow, typeof(EstateDefinitionType),
                            item.EstateDefinitionType?.Code, ref err);
                        var estateType = ImportHelper.GetDictByCode(uow, typeof(EstateType),
                            item.EstateType?.Code, ref err);

                        item.EstateDefinitionType = estateDefinitionType as EstateDefinitionType;
                        item.EstateType = estateType as EstateType;

                        rep.Create(item);
                    }

                    uow.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.ToString());
            }
        }
    }
}
