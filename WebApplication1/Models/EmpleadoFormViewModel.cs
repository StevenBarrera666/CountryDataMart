using System;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    // Clase simple e independiente para catálogos
    public class CatalogoItem
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }

    public class EmpleadoFormViewModel
    {
        public int Id { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string TipoIdentificacion { get; set; } = "Cédula de Ciudadanía";
        public string NumeroIdentificacion { get; set; }
        public string CorreoCorporativo { get; set; }
        public string TelefonoParticular { get; set; }
        public string TelefonoOficina { get; set; }
        public DateTime FechaNacimiento { get; set; } = DateTime.Today.AddYears(-25);
        public string Direccion { get; set; }
        public DateTime FechaIngreso { get; set; } = DateTime.Today;

        public string Cargo { get; set; }
        public string Area { get; set; }
        public string Empresa { get; set; }
        public string CentroCostos { get; set; }
        public string Supervisor { get; set; }
        public string Suplente { get; set; }
        public string TipoContrato { get; set; }
        public string JornadaLaboral { get; set; }

        public int? EpsPrevisionId { get; set; }
        public string FondoPensiones { get; set; }

        // Inicializados SIEMPRE con lista vacía para evitar NullReferenceException
        public List<int> FichasSeleccionadas { get; set; } = new List<int>();
        public List<FichaItemViewModel> FichasCatalogo { get; set; } = new List<FichaItemViewModel>();
        public List<CatalogoItem> EpsCatalogo { get; set; } = new List<CatalogoItem>();

        // Secciones adicionales (solo en edición): se cargan desde el perfil del empleado
        public int? FichaIdActual { get; set; }
        public string TabActiva { get; set; } = "comprobantes";
        public string FotoUrl { get; set; }
        public List<ComprobanteItemViewModel> Comprobantes { get; set; } = new List<ComprobanteItemViewModel>();
        public List<DocumentoItemViewModel> Documentos { get; set; } = new List<DocumentoItemViewModel>();
        public List<VacacionItemViewModel> Vacaciones { get; set; } = new List<VacacionItemViewModel>();
    }
}