using System.Linq;
using Base.DAL;
using CorpProp.Helpers;
using EUSI.Entities.Mapping;
using EUSI.Entities.NSI;

namespace EUSI.Validators
{
    public class EstateRegistrationDataValidator
    {
        /// <summary>
        /// Контрольная функция проверки соответствия атрибута "Способ поступления" атрибуту "Вид объекта заявки" 
        /// </summary>
        /// <param name="erTypeName">значения атрибута "Вид объекта заявки"</param>
        /// <param name="erRceiptReasonName">значения атрибута "Способ поступления"</param>
        /// <param name="iuow">Сессия</param>
        /// <returns></returns>
        public bool ValidateErTypeAndReceiptReason(IUnitOfWork iuow, string erTypeName, string erRceiptReasonName)
        {
            var error = string.Empty;

            var erType = ImportHelper.GetDictByName(iuow, typeof(EstateRegistrationTypeNSI), erTypeName, ref error) as EstateRegistrationTypeNSI;
            var erRceiptReason = ImportHelper.GetDictByName(iuow, typeof(ERReceiptReason), erRceiptReasonName, ref error) as ERReceiptReason;

            if (!string.IsNullOrEmpty(error) || erType == null)
                return false;
            
            if (erType != null && !iuow.GetRepository<ERTypeERReceiptReason>()
                .FilterAsNoTracking(x => x.ERType != null && x.ERType.Code == erType.Code).Any()) 
                return true;

            return iuow.GetRepository<ERTypeERReceiptReason>()
                        .FilterAsNoTracking(x => 
                        x.ERType != null && x.ERType.Code == erType.Code
                        && x.ERReceiptReason != null && x.ERReceiptReason.Code == erRceiptReason.Code)
                        .Any();
        }
    }
}
