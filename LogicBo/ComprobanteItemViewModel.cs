using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicBo
{  
    public class ComprobanteItemViewModel
    {
        public int Id { get; set; }
        public int EmpleadoId { get; set; }
        public string PeriodoMes { get; set; }
        public decimal SueldoBruto { get; set; }
        public decimal TotalNeto { get; set; }
        public decimal AportePatronal { get; set; }
        public decimal HorasExtras { get; set; }
        public decimal HorasRecargos { get; set; }
        public decimal HorasNoTrabajadas { get; set; }
        public decimal DiasNoTrabajados { get; set; }
        public string RutaPdf { get; set; }
    }
}
