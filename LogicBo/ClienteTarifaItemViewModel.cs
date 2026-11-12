using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicBo
{
    public class ClienteTarifaItemViewModel
    {
        public int CategoriaTarifaId { get; set; }
        public string CategoriaTarifaNombre { get; set; }
        public decimal TarifaAdministrativa { get; set; }
        public decimal Descuento { get; set; }
    }
}