using Entity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models.Entities
{
    [Table("Empleados")]
    public class Empleado
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Nombres { get; set; }

        [Required, StringLength(100)]
        public string Apellidos { get; set; }

        [StringLength(300)]
        public string FotoUrl { get; set; }

        [Required, StringLength(20)]
        public string FichaCodigo { get; set; }

        [Required, StringLength(10)]
        public string EtiquetaEstado { get; set; }

        [Required, StringLength(50)]
        public string TipoIdentificacion { get; set; }

        [Required, StringLength(50)]
        public string NumeroIdentificacion { get; set; }

        [Required, StringLength(150)]
        public string CorreoCorporativo { get; set; }

        [StringLength(30)]
        public string TelefonoParticular { get; set; }

        [StringLength(30)]
        public string TelefonoOficina { get; set; }

        public DateTime FechaNacimiento { get; set; }

        [StringLength(200)]
        public string Direccion { get; set; }

        // Llaves foráneas
        public int CargoId { get; set; }
        [ForeignKey("CargoId")]
        public virtual Cargo Cargo { get; set; }

        public int AreaId { get; set; }
        [ForeignKey("AreaId")]
        public virtual Area Area { get; set; }

        public int EpsPrevisionId { get; set; }
        [ForeignKey("EpsPrevisionId")]
        public virtual EpsPrevision EpsPrevision { get; set; }

        [Required, StringLength(150)]
        public string Division { get; set; }

        [Required, StringLength(150)]
        public string Empresa { get; set; }

        [Required, StringLength(50)]
        public string CentroCostos { get; set; }

        [StringLength(150)]
        public string Supervisor { get; set; }

        [StringLength(150)]
        public string Suplente { get; set; }

        [Required, StringLength(100)]
        public string TipoContrato { get; set; }

        [Required, StringLength(100)]
        public string JornadaLaboral { get; set; }

        public DateTime FechaIngreso { get; set; }

        public decimal SaldoVacacionesDias { get; set; }
    }
}