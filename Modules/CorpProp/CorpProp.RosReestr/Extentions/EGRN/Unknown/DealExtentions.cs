using Base.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.RosReestr.Extentions
{
    public static class DealExtentions
    {


        public static void SetDissenting(
            this Entities.DealRecord dd
            , IUnitOfWork uow
            , List<SibRosReestr.EGRN.Unknown.DissentingEntitiesDissenting_entity> objs
            )
        {
            if (objs == null) return;
            foreach (var item in objs)
            {
                if (item.Item != null)
                {
                    Entities.NameRecord nr =
                        uow.GetRepository<Entities.NameRecord>()
                        .Create(new Entities.NameRecord());
                    nr.DealRecord = dd;
                    if (item.Item is SibRosReestr.EGRN.Unknown.Fio)
                    {
                        nr.Surname = ((SibRosReestr.EGRN.Unknown.Fio)(item.Item)).Surname;
                        nr.Name = ((SibRosReestr.EGRN.Unknown.Fio)(item.Item)).Name;
                        nr.Patronymic = ((SibRosReestr.EGRN.Unknown.Fio)(item.Item)).Patronymic;
                        nr.FullName = $"{nr.Surname} {nr.Name} {nr.Patronymic}";
                    }
                    if (item.Item is SibRosReestr.EGRN.Unknown.Name)
                    {
                        nr.Name = ((SibRosReestr.EGRN.Unknown.Name)(item.Item)).name;
                        nr.FullName = nr.Name;
                    }
                }
            }
        }
    }
}
