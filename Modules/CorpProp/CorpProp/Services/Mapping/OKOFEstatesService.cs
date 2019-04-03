using Base.DAL;
using Base.Service;
using CorpProp.Common;
using CorpProp.Entities.Import;
using CorpProp.Entities.Mapping;
using CorpProp.Helpers;
using CorpProp.Helpers.Import.Extentions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Services.Mapping
{


    public interface IOKOFEstatesService : IBaseObjectService<OKOFEstates>, IExcelImportEntity, ISystemImportEntity
    {

    }

    public class OKOFEstatesService : BaseObjectService<OKOFEstates>, IOKOFEstatesService
    {


        public OKOFEstatesService(IBaseObjectServiceFacade facade) : base(facade)
        {


        }

        /// <summary>
        /// Переопределяет метод при событии создания объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Создаваемый объект.</param>
        /// <returns>Судно.</returns>
        public override OKOFEstates Create(IUnitOfWork unitOfWork, OKOFEstates obj)
        {
            return base.Create(unitOfWork, obj);
        }

        /// <summary>
        /// Переопределяет метод при событии сохранения объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="objectSaver">Метаданные сохраняемого объекта.</param>
        /// <returns></returns>
        protected override IObjectSaver<OKOFEstates> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<OKOFEstates> objectSaver)
        {

            return
                base.GetForSave(unitOfWork, objectSaver)
                    .SaveOneObject(x => x.OKOF2014)
                    ;
        }

        public void Import(
           IUnitOfWork uofw
           , IUnitOfWork histUofw
           , DataTable table
           , Dictionary<string, string> colsNameMapping
           , ref int count
           , ref ImportHistory history)
        {
            try
            {

                for (int r = 9; r < table.Rows.Count; r++)
                {
                    var row = table.Rows[r];
                    string err = "";

                    var okof2 = ImportHelper.GetValueByName(uofw, typeof(string), row, nameof(CorpProp.Entities.NSI.OKOF2014), colsNameMapping)?.ToString()?.Trim();
                    var estType = ImportHelper.GetValueByName(uofw, typeof(string), row, nameof(OKOFEstates.EstateType), colsNameMapping)?.ToString()?.Trim();

                    if (!string.IsNullOrWhiteSpace(okof2) && !string.IsNullOrWhiteSpace(estType))
                    {
                        var obj = uofw.GetRepository<OKOFEstates>().FilterAsNoTracking(x => x.EstateType == estType && x.OKOF2014 != null && x.OKOF2014.Code == okof2).FirstOrDefault();
                        if (obj == null)
                        {
                            obj = new OKOFEstates();
                            obj.FillObject(uofw, typeof(OKOFEstates),
                                   row, row.Table.Columns, ref err, ref history, colsNameMapping, IdentyDictByCode());
                            uofw.GetRepository<OKOFEstates>().Create(obj);
                        }
                    }
                    else
                    {
                        history.ImportErrorLogs.AddError(r + 1, null, "", "Поле \"OKOF2014\" или \"EstateType\" не содержит значение.", ErrorType.BreaksBusinessRules, "", "", table.TableName);
                    }
                    count++;
                }
                uofw.SaveChanges();
            }
            catch (Exception ex)
            {
                history.ImportErrorLogs.AddError(ex);
            }
        }

        /// <summary>
        /// Идентификация справочников по коду.
        /// </summary>
        /// <returns></returns>
        public bool IdentyDictByCode()
        {
            return true;
        }

        public void CancelImport(
             IUnitOfWork uofw
            , ref ImportHistory history
            )
        {
            throw new NotImplementedException();
        }
    }
}
