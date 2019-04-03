using System;
using System.Linq.Expressions;
using Base;
using Base.DAL;
using Base.DAL.EF;
using CorpProp.Entities.Request.ResponseValue;
using Data.EF;

namespace Data
{
    public partial class CorpPropConfig
    {
        static void InitRequest(EntityConfigurationBuilder config)
        {
            #region CorpProp.Entities.Request

            config
                .Context(EFRepositoryFactory<DataContext>.Instance)
                .Entity<CorpProp.Entities.Request.Request>(builder => builder.Save(saver => saver
                    .SaveOneObject(request => request.RequestStatus)
                    .SaveOneObject(request => request.RequestContent)
                    .SaveOneObject(request => request.Parent)
                    .SaveOneObject(request => request.Responsible)
                    .SaveOneToMany(request => request.Children)
                ))
                .Entity<CorpProp.Entities.Request.RequestColumn>(
                    builder => builder.Save(saver => saver
                        .SaveOneObject(request => request.RequestContent)
                        .SaveOneObject(request => request.TypeData)))
                .Entity<CorpProp.Entities.Request.RequestContent>(builder => builder.Save(saver => saver
                    .SaveOneObject(request => request.RequestTemplate)
                    .SaveOneObject(request => request.Author)
                    .SaveOneToMany(request => request.Requests)
                ))
                .Entity<CorpProp.Entities.Request.RequestRow>(builder => builder.Save(saver => saver
                    .SaveOneObject(request => request.RequestContent)
                ))
                .Entity<CorpProp.Entities.Request.RequestTemplate>(builder => builder.Save(saver => saver
                    .SaveOneToMany(request => request.RequestContents)
                ))
                .Entity<CorpProp.Entities.Request.Response>(builder => builder.Save(saver => saver
                    .SaveOneObject(request => request.ResponseStatus)
                    .SaveOneObject(request => request.Request)
                    .SaveOneObject(response => response.Executor)
                ))
                .Entity<CorpProp.Entities.Request.ResponseValue.ResponseCellString>(builder => builder.Save(SaveResponseBase<ResponseCellString, string>))
                .Entity<CorpProp.Entities.Request.ResponseValue.ResponseCellDict>(builder => builder.Save(SaveResponseBase<ResponseCellDict, int?>))
                .Entity<CorpProp.Entities.Request.ResponseRow>(builder => builder.Save(saver => saver.SaveOneObject(request => request.Response)

                #region ManyToMany
                .Entity<CorpProp.Entities.ManyToMany.CadastralAndExtract>()
                .Entity<CorpProp.Entities.ManyToMany.EstateAndEstateAppraisal>()
                .Entity<CorpProp.Entities.ManyToMany.FileCardAndAppraisal>()
                .Entity<CorpProp.Entities.ManyToMany.FileCardAndCertificateRight>()
                .Entity<CorpProp.Entities.ManyToMany.FileCardAndDoc>()
                .Entity<CorpProp.Entities.ManyToMany.FileCardAndEstate>()
                .Entity<CorpProp.Entities.ManyToMany.FileCardAndLegalRight>()
                .Entity<CorpProp.Entities.ManyToMany.FileCardAndNonCoreAsset>()
                .Entity<CorpProp.Entities.ManyToMany.FileCardAndRequestContent>()
                .Entity<CorpProp.Entities.ManyToMany.FileCardAndResponse>()
                .Entity<CorpProp.Entities.ManyToMany.FileCardAndScheduleStateYear>()
                .Entity<CorpProp.Entities.ManyToMany.FileCardAndScheduleStateRegistrationRecord>()
                .Entity<CorpProp.Entities.ManyToMany.FileCardAndScheduleStateTerminateRecord>()
                .Entity<CorpProp.Entities.ManyToMany.FileCardOneAndFileCardMany>()
                .Entity<CorpProp.Entities.ManyToMany.FileCardAndSibProject>()

                .Entity<CorpProp.Entities.ManyToMany.IntangibleAssetAndSibCountry>()


                .Entity<CorpProp.Entities.ManyToMany.SibTaskAndAppraisal>()
                .Entity<CorpProp.Entities.ManyToMany.SibTaskAndDeal>()
                .Entity<CorpProp.Entities.ManyToMany.SibTaskAndEstate>()
                .Entity<CorpProp.Entities.ManyToMany.SibTaskAndFileCard>()
                .Entity<CorpProp.Entities.ManyToMany.SibTaskAndRight>()
                .Entity<CorpProp.Entities.ManyToMany.SibTaskReportAndAppraisal>()
                .Entity<CorpProp.Entities.ManyToMany.SibTaskReportAndDeal>()
                .Entity<CorpProp.Entities.ManyToMany.SibTaskReportAndEstate>()
                .Entity<CorpProp.Entities.ManyToMany.SibTaskReportAndFileCard>()
                .Entity<CorpProp.Entities.ManyToMany.SibTaskReportAndRight>()
                #endregion
                ));

            #endregion
        }

        private static void SaveResponseBase<TResponseCellBase, T>(IObjectSaver<TResponseCellBase> saver)
            where TResponseCellBase: ResponseCellBase<T>
        {
            saver
                .SaveOneObject(request => request.LinkedResponse)
                .SaveOneObject(request => request.LinkedColumn)
                .SaveOneObject(request => request.LinkedRow);
        }
    }
}