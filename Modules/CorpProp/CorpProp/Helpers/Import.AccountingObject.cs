using Base.DAL;
using CorpProp.Entities.Accounting;
using CorpProp.Entities.Estate;
using CorpProp.Entities.Import;
using CorpProp.Entities.Mapping;
using CorpProp.Helpers.Import.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic;
using Base;

namespace CorpProp.Helpers
{
    /// <summary>
    /// Предоставляет методы по созданию ОИ при импорте ОБУ.
    /// </summary>
    public static class ImportAccountingObject
    {
        /// <summary>
        /// Проверяет ОБУ на соответсвие правилам создания ОИ при имопрте ОБУ и создает соответствующий объект.
        /// </summary>
        /// <param name="uofw"></param>
        /// <param name="obj"></param>
        /// <param name="history"></param>
        /// <returns></returns>
        public static object CheckRules(
            IUnitOfWork uofw
         , AccountingObject obj
         , ref ImportHistory history
         )
        {
            object est = null;
            try
            {
                List<AccountingObject> objs = new List<AccountingObject>() { obj };
                IQueryable<AccountingObject> q = objs.AsQueryable<AccountingObject>();
                IOrderedQueryable<EstateRulesCteation> rules =
                uofw.GetRepository<EstateRulesCteation>().Filter(x => x.Hidden == false).OrderBy(x=>x.Order);

                if (rules != null && rules.Count() > 0)
                    foreach (EstateRulesCteation rule in rules)
                    {
                        try
                        {
                            //Если правило выполнено и объект удовлетворяет условию, то создается соответсвующий ОИ.
                            var res = q.Where(rule.Criteria).Any();
                            if (res)
                            {
                                est = CreateEstateByMnemonic(uofw, obj, rule.EstateMnemonic, ref history);
                                return est;
                            }                               

                        }
                        catch (Exception e) { history.ImportErrorLogs.AddError(e); }
                        
                    }

            }
            catch (Exception ex)
            {
                history.ImportErrorLogs.AddError(ex);
            }
            return est;
        }


        /// <summary>
        /// Находит или создает ОИ из ОБУ.
        /// </summary>
        /// <param name="uofw"></param>
        /// <param name="obj"></param>
        /// <param name="history"></param>
        /// <returns></returns>
        public static object FindOrCreateEstate(
         IUnitOfWork uofw
         , AccountingObject obj
         , ref ImportHistory history
         )
        {
            object est = null;
            
            //ищем ОИ
            est = FindEstate(uofw, obj, ref history);
            if (est != null)
                return est;
            //создаем по правилам АИС КС
            est = CreateEstateByRules(uofw, obj, ref history);
            return est;
        }

        /// <summary>
        /// Создает новый ОИ по правилам АИС КС.
        /// </summary>
        /// <param name="uofw"></param>
        /// <param name="obj"></param>
        /// <param name="history"></param>
        /// <returns></returns>
        public static object CreateEstateByRules(
         IUnitOfWork uofw
         , AccountingObject obj
         , ref ImportHistory history
         )
        {
            object est = null;
                       
            //Проверка 1
            //Создаем из правил
            est = CheckRules(uofw, obj, ref history);
            if (est != null)
                return est;

            //Проверка 2
            //Не соответствует ни одному правилу, проверяем по мэппингу Класс БУ - Тип ОИ
            if (obj.ClassFixedAsset != null)
            {
                est = CreateEstateByClassCode(uofw, obj, ref history);
                if (est != null)
                    return est;
            }

            //Проверка 3
            //проверяем по мэппингу ОКОФ - Тип ОИ
            if (obj.OKOF2014 != null)
            {
                est = CreateEstateByOKOF(uofw, obj, ref history);
                if (est != null)
                    return est;
            }

            //правило создания движимого имущества
            if (obj.IsRealEstateImpl != null && obj.IsRealEstateImpl == false)
            {
                est = CreateEstateByMnemonic(uofw, obj, nameof(MovableEstate), ref history);
                if (est != null)
                    return est;
            }

            //Ничего не создали, смотрим на кадастровый номер
            if (!String.IsNullOrEmpty(obj.CadastralNumber) && obj.ClassFixedAsset == null)
            {
                var cad = ImportHelper.SimpleEstate<Cadastral>(uofw, obj, ref history);
                cad.IsRealEstate = obj.IsRealEstate;
                cad.CadastralNumbers = obj.CadastralNumber;
                est = uofw.GetRepository<Cadastral>().Create(cad);

                if (est != null)
                    return est;
            }

            //Не можем понять какой тип ОИ создавать, создаем просто ОИ
            if (est == null)
            {
                var ess = ImportHelper.SimpleEstate<Estate>(uofw, obj, ref history);
                est = uofw.GetRepository<Estate>().Create(ess);
            }

            return est;
        }

        /// <summary>
        /// Создает ОИ согласно мэппингу класса БУ и мнемоники ОИ.
        /// </summary>
        /// <param name="uofw">Сессия.</param>
        /// <param name="obj">Объект БУ.</param>
        /// <param name="history">История импорта.</param>
        /// <returns></returns>
        public static object CreateEstateByClassCode(
            IUnitOfWork uofw
            , AccountingObject obj            
            , ref ImportHistory history)
        {
            if (obj.ClassFixedAsset == null) return null;
            string code = obj.ClassFixedAsset.Code;
            var tps = uofw.GetRepository<AccountingEstates>()
                .FilterAsNoTracking(x => x.Hidden == false
                && x.ClassFixedAsset != null
                && x.ClassFixedAsset.Code == code).FirstOrDefault();
            if (tps != null)
            {
                return CreateEstateByMnemonic(uofw, obj, tps.EstateType, ref history);               
            }
            return null;
        }

        /// <summary>
        /// Создает ОИ согласно мэппингу с кодами ОКОФ.
        /// </summary>
        /// <param name="uofw"></param>
        /// <param name="obj"></param>
        /// <param name="history"></param>
        /// <returns></returns>
        public static object CreateEstateByOKOF(
          IUnitOfWork uofw
          , AccountingObject obj         
          , ref ImportHistory history)
        {
            if (obj.OKOF2014 == null) return null;
            string code = obj.OKOF2014.Code;
            var tps = uofw.GetRepository<OKOFEstates>()
                .Filter(x => x.Hidden == false
                && x.OKOF2014 != null
                && x.OKOF2014.Code == code).FirstOrDefault();
            if (tps != null)
            {
                return CreateEstateByMnemonic(uofw, obj, tps.EstateType, ref history);
            }
            return null;
        }


        /// <summary>
        /// Создает ОИ по мнемонике.
        /// </summary>
        /// <param name="uofw"></param>
        /// <param name="obj"></param>
        /// <param name="mnemonic"></param>
        /// <param name="history"></param>
        /// <returns></returns>
        public static object CreateEstateByMnemonic(
           IUnitOfWork uofw
           , AccountingObject obj
           , string mnemonic          
           , ref ImportHistory history
           )
        {
            if (String.IsNullOrEmpty(mnemonic))
                return null;
            Type entityType = TypesHelper.GetTypeByName(mnemonic);
            if (entityType != null)
            {
                MethodInfo methodUow = typeof(ImportHelper).GetMethod("SimpleEstate");
                MethodInfo genericUow = methodUow.MakeGenericMethod(entityType);
                var ob = genericUow.Invoke(null, new object[] { uofw, obj, history });
                var bo = ImportHelper.CreateRepositoryObject(uofw, entityType, ob);               
                return bo;
            }
            return null;
        }

        /// <summary>
        /// Ищет существующий в Системе ОИ по данным ОБУ.
        /// </summary>
        /// <param name="uofw"></param>
        /// <param name="obj"></param>
        /// <param name="history"></param>
        /// <returns></returns>
        public static object FindEstate(
        IUnitOfWork uofw
        , AccountingObject obj
        , ref ImportHistory history)
        {
            object est = null;
            try
            {
                //1 ищем по кадастровому номеру
                if (!String.IsNullOrEmpty(obj.CadastralNumber))
                {
                    string cad = obj.CadastralNumber;
                    est = uofw.GetRepository<Cadastral>()
                        .Filter(x => x.Hidden == false && x.CadastralNumber == cad)
                        .FirstOrDefault();
                }
                //другие варианты связки
                //TODO: связывание ОБУ с ОИ
            }
            catch (Exception ex) { history.ImportErrorLogs.AddError(ex); }
            return est;
        }

        /// <summary>
        /// Получает значение признака движимого/недвижимого имущества.
        /// </summary>
        /// <param name="obj">Текущее значение свойства IsRealEstateImpl.</param>
        /// <param name="value">Значение IsRealEstate импортируемого файла.</param>
        /// <returns></returns>
        public static bool? GetIsRealEstate(object obj, object value)
        {
            bool? curr = obj as bool?;
            bool bb = false;
            if (curr != true && value != null && bool.TryParse(value.ToString(), out bb))
            {
                bool.TryParse(value.ToString(), out bb);
                return bb;
            }
            return curr;                       
        }


        public static void UpdateEstateData(IUnitOfWork uow, AccountingObject obj)
        {
            if (obj == null || obj.EstateID == null || obj.IsHistory) return;
            Estate estate = uow.GetRepository<Estate>().Find(f => f.ID == obj.EstateID);
            estate.UpdateByAccounting(uow, obj);
            uow.GetRepository<Estate>().Update(estate);
        }
        

    }
}
