using Base.DAL;
using Base.Extensions;
using CorpProp.Entities.Document;
using EUSI.Entities.Accounting;
using EUSI.Entities.ManyToMany;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUSI.Helpers
{
    public static class AccountingMovingHelper
    {

        public static FileCardAndAccountingMoving AddFileLink(
            IUnitOfWork uofw
            , DataRow row
            , Dictionary<string, string> colsNameMapping
            , AccountingMoving obj
            )
        {
            //ссылка на первичный документ
            FileCardAndAccountingMoving link = null;
            if (colsNameMapping.Where(x => x.Value.ToLower() == "filecardlink").Any())
            {
                var colName = colsNameMapping.Where(x => x.Value.ToLower() == "filecardlink").FirstOrDefault().Key;
                DataColumn col = row.Table.Columns[colName];
                if (col != null && row[col] != System.DBNull.Value)
                {                   
                    link = uofw.GetRepository<FileCardAndAccountingMoving>()
                        .Create(new FileCardAndAccountingMoving()
                        {
                            ObjLeft = new FileCardOne()
                            {
                                Name = row[col].ToString(),
                                CategoryID = uofw.GetRepository<CardFolder>()
                                .Filter(f => !f.Hidden).Min(s => s.ID)
                            },
                            ObjRigth = obj
                        });
                }
            }
            return link;
        }

        public static void ClearFileLinks(
            IUnitOfWork uow           
            , AccountingMoving obj
            )
        {
            if (obj == null) return;
            var links = uow.GetRepository<FileCardAndAccountingMoving>()                
                .Filter(f => !f.Hidden && f.ObjRigthId == obj.ID)
                .Include(inc => inc.ObjLeft)
                .ToList();
            foreach (var link in links)
            {
                link.Hidden = true;
                link.ObjLeft.Hidden = true;
            }
        }
    }
}
