using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LogicBo
{
    public class EmpleadoRegistroViewModel
    {
        
        [Required]
        [StringLength(100)]
        public string Nombres { get; set; }

        [Required]
        [StringLength(100)]
        public string Apellidos { get; set; }

        public string FotoUrl { get; set; }

        [Required]
        [StringLength(20)]
        public string FichaCodigo { get; set; }

        [Required]
        [StringLength(10)]
        public string EtiquetaEstado { get; set; }

        [Required]
        public string TipoIdentificacion { get; set; }

        [Required]
        [StringLength(50)]
        public string NumeroIdentificacion { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string CorreoCorporativo { get; set; }

        [StringLength(30)]
        public string TelefonoParticular { get; set; }

        [StringLength(30)]
        public string TelefonoOficina { get; set; }

        [Required]
        public DateTime FechaNacimiento { get; set; }

        [StringLength(200)]
        public string Direccion { get; set; }

        [Required]
        public int CargoId { get; set; }

        [Required]
        public int AreaId { get; set; }

        [Required]
        public int EpsPrevisionId { get; set; }

        [Required]
        [StringLength(150)]
        public string Division { get; set; }

        [Required]
        [StringLength(150)]
        public string Empresa { get; set; }

        [Required]
        [StringLength(50)]
        public string CentroCostos { get; set; }

        [StringLength(150)]
        public string Supervisor { get; set; }

        [StringLength(150)]
        public string Suplente { get; set; }

        [Required]
        [StringLength(100)]
        public string TipoContrato { get; set; }

        [Required]
        [StringLength(100)]
        public string JornadaLaboral { get; set; }

        [Required]
        public DateTime FechaIngreso { get; set; }

        public decimal SaldoVacacionesDias { get; set; }
        // Dentro de EmpleadoPerfilViewModel.cs:
        public int Id { get; set; } // Asegúrate de incluir el Id del empleado
        public List<ComprobanteItemViewModel> Comprobantes { get; set; } = new List<ComprobanteItemViewModel>();
    }
}