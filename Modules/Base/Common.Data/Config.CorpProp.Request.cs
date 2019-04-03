using System;
using Base.DAL;
using Base.DAL.EF;
using CorpProp.Entities.Request;
using CorpProp.Entities.Request.ResponseCells;

namespace Common.Data
{
    public partial class CorpPropConfig
    {
        static void InitRequest<TContext>(EntityConfigurationBuilder config) where TContext : EFContext
        {
            #region CorpProp.Entities.Request

            config
                .Context(EFRepositoryFactory<TContext>.Instance)
                .Entity<Request>(builder => builder.Save(saver => saver
                    .SaveOneObject(request => request.RequestStatus)
                    .SaveOneObject(request => request.RequestContent)
                    .SaveOneObject(request => request.Parent)
                    .SaveOneObject(request => request.Responsible)
                    .SaveOneToMany(request => request.Children)
                ))
                .Entity<RequestColumn>(
                    builder => builder.Save(saver => saver
                        .SaveOneObject(request => request.RequestContent)
                        .SaveOneObject(request => request.TypeData)))
                .Entity<RequestContent>(builder => builder.Save(saver => saver
                    .SaveOneObject(request => request.RequestTemplate)
                    .SaveOneObject(request => request.Author)
                    .SaveOneToMany(request => request.Requests)
                ))
                .Entity<RequestRow>(builder => builder.Save(saver => saver
                    .SaveOneObject(request => request.RequestContent)
                ))
                .Entity<RequestTemplate>(builder => builder.Save(saver => saver
                    .SaveOneToMany(request => request.RequestContents)
                ))
                .Entity<Response>(builder => builder.Save(saver => saver
                    .SaveOneObject(request => request.ResponseStatus)
                    .SaveOneObject(request => request.Request)
                    .SaveOneObject(response => response.Executor)
                ))
                .Entity<ResponseCellBoolean>(builder => builder.Save(SaveResponseBase<ResponseCellBoolean, bool?>))
                .Entity<ResponseCellDateTime>(builder => builder.Save(SaveResponseBase<ResponseCellDateTime, DateTime?>))
                .Entity<ResponseCellDecimal>(builder => builder.Save(SaveResponseBase<ResponseCellDecimal, decimal?>))
                .Entity<ResponseCellDict>(builder => builder.Save(SaveResponseBase<ResponseCellDict, int?>))
                .Entity<ResponseCellDouble>(builder => builder.Save(SaveResponseBase<ResponseCellDouble, double?>))
                .Entity<ResponseCellFloat>(builder => builder.Save(SaveResponseBase<ResponseCellFloat, float?>))
                .Entity<ResponseCellInt>(builder => builder.Save(SaveResponseBase<ResponseCellInt, int?>))
                .Entity<ResponseCellString>(builder => builder.Save(SaveResponseBase<ResponseCellString, string>))

                .Entity<ResponseRow>(builder => builder.Save(saver => saver.SaveOneObject(request => request.Response)))
                .Entity<RequestAndSibUserManyToManyAssociation>()
                .Entity<RequestColumnItems>(builder => builder.Save(saver => saver.SaveOneObject(items => items.RequestColumn)));

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