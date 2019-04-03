using Base.DAL;
using CorpProp.Entities.Import;
using System;
using System.Linq;
using System.Linq.Expressions;
using Base.Utils.Common;
using CorpProp.Helpers.Import.Extentions;
using CorpProp.Entities.Estate;

namespace EUSI.Helpers
{
    public class EUSIImportHelper
    {
        protected static readonly string[] ReceiptReasonNameForOutOfBalance =
        {
            "Rent",//"Взятие в аренду/пользование"
            "RentOut"//"Передача в аренду/пользование"
        };

        public EUSIImportHelper() { }

        /// <summary>
        /// Ищет существующий в Системе ОИ по номеру ЕУСИ.
        /// </summary>
        /// <param name="uofw"></param>
        /// <param name="EUSINumber"></param>
        /// <param name="history"></param>
        /// <returns></returns>
        public static object FindEstate(
        IUnitOfWork uofw
        , int EUSINumber
        , ref ImportHistory history)
        {
            object est = null;
            try
            {                
                if (EUSINumber != 0)
                {
                    
                    est = uofw.GetRepository<Estate>()
                        .Filter(x => !x.Hidden && !x.IsHistory && x.Number!= 0 && x.Number == EUSINumber)
                        .FirstOrDefault();
                }               
            }
            catch (Exception ex) { history.ImportErrorLogs.AddError(ex); }
            return est;
        }

        /// <summary>
        /// Получает объект типа DictObject по его публичному коду. 
        /// </summary>
        /// <param name="uofw">Сессия.</param>
        /// <param name="type">Тип объекта.</param>
        /// <param name="publishCode">Публичный код.</param>
        /// <param name="error">Текст ошибки.</param>
        /// <returns></returns>
        public static object GetDictByPublishCode(IUnitOfWork uofw, Type type, string publishCode)
        {
            object obj = null;
            try
            {
                if (String.IsNullOrEmpty(publishCode))
                    return null;
                //берем репозиторий
                var methodUow = uofw.GetType().GetMethod("GetRepository");
                var genericUow = methodUow?.MakeGenericMethod(type);
                var reposit = genericUow?.Invoke(uofw, null);

                //фильтруем по коду
                var parameter = Expression.Parameter(type, "dict");
                var lambda = Expression.Lambda(
                         Expression.And(
                        Expression.Equal(
                            Expression.Property(parameter, "PublishCode"),
                            Expression.Constant(publishCode)
                        ),
                        Expression.Equal(
                            Expression.Property(parameter, "Hidden"),
                            Expression.Constant(false)
                        )
                     )
                    , parameter);

                var method = reposit?.GetType().GetMethod("Filter");
                var filter = method?.Invoke(reposit, new object[] { lambda });

                obj = ((IQueryable)filter)?.Provider.Execute(Expression.Call(
                                  typeof(Enumerable),
                                  "FirstOrDefault",
                                  new [] { type },
                                  ((IQueryable)filter).Expression));

                return obj;

            }
            catch (Exception ex)
            {
                //error += $"{ex.ToStringWithInner()} {Environment.NewLine}";
            }
            return obj;
        }

        public static bool IsOutOfBalance(string receiptReasonName)
        {
            if (string.IsNullOrEmpty(receiptReasonName)) return false;
            return ReceiptReasonNameForOutOfBalance.Contains(receiptReasonName);
        }
    }
}
