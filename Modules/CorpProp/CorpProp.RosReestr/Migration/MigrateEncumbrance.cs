using Base.DAL;
using CorpProp.Entities.Law;
using CorpProp.RosReestr.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.RosReestr.Migration
{
    public static class MigrateEncumbrance
    {
        public static void Migrate(this Encumbrance enc, IUnitOfWork uow, Right rr, RestrictRecord item)
        {
            enc.AllShareOwner = item.AllShareOwner;
            enc.EncumbranceType = CorpProp.RosReestr.Helpers.ImportHelper.FindOrCreateDictByName<EncumbranceType>(uow, item.TypeName);
            enc.EncumbranceTypeCode = item.TypeCode;
            enc.EndDate = item.EndDate;
            enc.Name = $"{item.TypeName} {item.RegNumber} {item.RegDate}";
            enc.Owner = item.Owner;
            enc.OwnerName = item.Owner;
            enc.RegDate = item.RegDate;
            enc.RegNumber = item.RegNumber;
            enc.ShareText = item.ShareText;
            enc.StartDate = item.StartDate;
            enc.Term = item.Term;
            enc.Title = enc.Name;
            enc.Right = rr;
            enc.Estate = rr.Estate;
        }

        public static Encumbrance FindOrCreateEncumbrance(IUnitOfWork uow, RestrictRecord item, MigrateHolder holder)
        {
            if (item == null) return null;

            Encumbrance enc = null;

            var numb = item.RegNumber;            
            var rightNumb = item.RightRecord?.RegNumber;

            if (!String.IsNullOrEmpty(numb) && !String.IsNullOrEmpty(rightNumb))
                enc = uow.GetRepository<Encumbrance>()
                    .Filter(x => !x.Hidden && x.RegNumber == numb 
                    && x.Right != null && x.Right.RegNumber == rightNumb).FirstOrDefault();

            if (enc == null && !String.IsNullOrEmpty(numb) && String.IsNullOrEmpty(rightNumb))
                enc = uow.GetRepository<Encumbrance>()
                    .Filter(x => !x.Hidden && x.RegNumber == numb).FirstOrDefault();
            if (enc == null)
            {
                enc = uow.GetRepository<Encumbrance>().Create(new Encumbrance());
                holder.AddLog(nameof(Encumbrance), item.RegNumber, "101");
                return enc;
            }
            else
            {
                uow.GetRepository<Encumbrance>().Update(enc);
                holder.AddLog(nameof(Encumbrance), item.RegNumber, "102");
                return enc;
            }            
        }
    }
}
