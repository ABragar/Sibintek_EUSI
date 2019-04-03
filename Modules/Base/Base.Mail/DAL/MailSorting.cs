using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Mail.DAL
{
    [NotMapped]
    public class MailSorting
    {
        public string Field { get; set; }
        public OrderDirection Direction { get; set; }
    }

    public enum OrderDirection
    {
        Ask = 0,
        Desc = 1,
    }
}
