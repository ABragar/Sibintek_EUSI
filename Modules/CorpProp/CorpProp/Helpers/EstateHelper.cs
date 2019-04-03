using Base;
using Base.DAL;
using CorpProp.Entities.Base;
using CorpProp.Entities.Estate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Helpers
{

    /// <summary>
    /// Помощник при работе с классами объектов имущества.
    /// </summary>
    public static class EstateHelper
    {
        public const string DuplInPcError = "Ошибка добавления в ИК: В Системе существуют другие объекты имущества с тем же кадастровым номером {0}, которые не добавлены в данный ИК.";

        private const string ComplexIOErrorMessage = "В системе существуют кадастровые объекты, которые объедены одним \"Правом\" и они не включены в выбранный имущественный комплекс.";

        private const string EstateErrorMessage = "Тип не является инвентарным объектом";

        ///// <summary>
        ///// Создаёт набор динамических атрибутов на основании настроенных шаблонов для ОИ.
        ///// </summary>
        ///// <param name="unitOfWork">Сессия.</param>
        ///// <param name="obj">Создаваемый ОИ.</param>
        ///// <returns>Типизированный объект имущества.</returns>
        //public static T AddMembers<T>(IUnitOfWork unitOfWork, T obj)  where T : CorpProp.Entities.Estate.Estate
        //{
        //    return default(T);
        //    if (obj != null && obj.EstateTypeID != null && obj.EstateTypeID != 0 )
        //    {
        //        try
        //        {
        //            var addons = unitOfWork.GetRepository<AddonMember>();
        //            var templates = unitOfWork.GetRepository<AddonMemberTemplate>()
        //                .Filter(x => x.EstateTypes.Where(s => s.ObjectID == obj.EstateTypeID)
        //                .Select(s => s.AddonMemberTemplateID).Contains(x.ID))
        //                .ToArray();

        //            foreach (var tmp in templates)
        //            {
        //                foreach (var item in tmp.AddonMemberTemplateItems)
        //                {
        //                    if (obj.AddonMembers != null &&
        //                        obj.AddonMembers
        //                        .Where(x => x.TemplateID == tmp.ID &&
        //                                    x.TemplateItemID == item.ID)
        //                        .Count() < 1)
        //                    {
        //                        AddonMember pr = new AddonMember();
        //                        pr.Name = item.Name;
        //                        pr.EstateID = obj.ID;
        //                        pr.TypeDataID = item.TypeDataID;
        //                        pr.TemplateID = tmp.ID;
        //                        pr.TemplateItemID = item.ID;
        //                        var prop = addons.Create(pr);
        //                    }
        //                }
        //            }
        //            unitOfWork.SaveChanges();
        //        }
        //        catch (Exception ex)
        //        {
        //            throw new Exception("Не удалось добавить Дополнительные характеристики. " + ex.Message);
        //        }

        //    }
        //    return obj;

        //}

        /// <summary>
        /// Возвращает наименования всех родительских элементов в формате: nameRoot\nameChild1\nameChild11.
        /// </summary>
        /// <param name="item">Иерархичный объект.</param>
        /// <param name="names">Строка с ранее полученными наименованиями дочерних объектов.</param>
        /// <returns></returns>
        public static string GetAllParentNames(HCategory item, string names)
        {
            string str = names;
            if (String.IsNullOrEmpty(names))
                str = item.Name;
            if (item != null && item.Parent != null && String.IsNullOrEmpty(item.Parent.Name))
            {
                str = String.Format("{0}\\{1}", item.Parent.Name, str);
                str = GetAllParentNames(item.Parent, str);
            }
            return str;
        }

        public static bool CheckDuplInPC(IUnitOfWork uow, Cadastral cad, string cadMumber, string[] ids, int pc, bool isInvObj = false)
        {

            if (String.IsNullOrEmpty(cadMumber))
                return false;

            if (cad != null)
            {
                if (isInvObj)
                {
                    return uow.GetRepository<Cadastral>()
                        .Filter(f =>
                            !f.Hidden
                            && !ids.Contains(f.ID.ToString())
                            && f.ID != cad.ID
                            && f.CadastralNumber == cadMumber
                            && !f.IsHistory &&
                            (f.Parent == null || (f.Parent != null && f.Parent.ID != pc)))
                        .Any();
                }

                return uow.GetRepository<Cadastral>()
                    .Filter(f =>
                        !f.Hidden
                        && !ids.Contains(f.ID.ToString())
                        && f.ID != cad.ID
                        && f.CadastralNumber == cadMumber
                        && !f.IsHistory &&
                        (f.PropertyComplex == null || (f.PropertyComplex != null && f.PropertyComplex.ID != pc)))
                    .Any();



            }
            return false;
        }

        public static void CheckRightsEstate(IUnitOfWork uofw, BaseObject model)
        {
            var invObj = model as InventoryObject;
            if (invObj == null)
            {
                throw new Exception(EstateErrorMessage);
            }

            if (invObj.Parent == null)
            {
                return;
            }

            if (invObj.Fake != null)
            {
                int estatesInCadastralAndNotInIK = uofw.GetRepository<InventoryObject>()
                    .FilterAsNoTracking(k => k.Fake.ID == invObj.Fake.ID && k.Parent.ID != invObj.Parent.ID)
                    .Count();

                if (estatesInCadastralAndNotInIK > 0)
                {
                    throw new Exception(ComplexIOErrorMessage);
                }
            }
        }
    }
}
