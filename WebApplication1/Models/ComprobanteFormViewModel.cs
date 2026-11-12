using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace WebApplication1.Models
{
    // Formulario para Comprobante de Pago
    public class ComprobanteFormViewModel
    {
        public int EmpleadoId { get; set; }
        public int? FichaId { get; set; }

        [Required(ErrorMessage = "El período es requerido (ej: 09-2026).")]
        public string PeriodoMes { get; set; }

        public decimal SueldoBruto { get; set; }
        public decimal TotalNeto { get; set; }
        public decimal AportePatronal { get; set; }
        public decimal HorasExtras { get; set; }
        public decimal HorasRecargos { get; set; }
        public decimal HorasNoTrabajadas { get; set; }
        public decimal DiasNoTrabajados { get; set; }

        // Archivo PDF adjunto
        public HttpPostedFileBase ArchivoPdf { get; set; }
    }

    // Formulario para Documento Digital
    public class DocumentoFormViewModel
    {
        public int EmpleadoId { get; set; }

        [Required(ErrorMessage = "El nombre del documento es requerido.")]
        public string NombreDocumento { get; set; }

        public string Categoria { get; set; } = "General"; // Identificación, Laboral, etc.

        [Required(ErrorMessage = "Debe adjuntar un archivo.")]
        public HttpPostedFileBase Archivo { get; set; }
    }

    // Formulario para Solicitud de Vacaciones
    public class VacacionFormViewModel
    {
        public int EmpleadoId { get; set; }

        [Required(ErrorMessage = "El período de cálculo es requerido.")]
        public string PeriodoCalculo { get; set; } // Ej: 2025 - 2026

        [Required(ErrorMessage = "Indique la cantidad de días solicitados.")]
        public decimal DiasTomados { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es requerida.")]
        public DateTime FechaInicio { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "La fecha de finalización es requerida.")]
        public DateTime FechaFin { get; set; } = DateTime.Today.AddDays(15);

        [Required(ErrorMessage = "La fecha de retorno es requerida.")]
        public DateTime FechaRetorno { get; set; } = DateTime.Today.AddDays(16);

        public string Observaciones { get; set; }
    }
}