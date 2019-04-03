using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.CRM.Entities;
using Base.CRM.UI.Presets;
using Base.Service;

namespace Base.CRM.Services.Abstract
{
    public interface IDealService: IService
    {
        List<SalesFunnel> GetSalesFunnel(SalesFunnelPreset preset);
    }
}
