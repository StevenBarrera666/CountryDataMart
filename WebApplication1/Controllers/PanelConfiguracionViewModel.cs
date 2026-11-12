using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Controllers
{
    public class PanelConfiguracionViewModel
    {
        public List<CentroCostoModel> CentrosCosto { get; set; } = new List<CentroCostoModel>();
        public List<TarjetaCreditoModel> TarjetasCredito { get; set; } = new List<TarjetaCreditoModel>();
        
    }
}