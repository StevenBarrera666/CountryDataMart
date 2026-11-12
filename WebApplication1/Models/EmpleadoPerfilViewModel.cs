using System;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    /// <summary>
    /// ViewModel principal para renderizar la vista de perfil del empleado (Perfil.cshtml).
    /// </summary>
    public class EmpleadoPerfilViewModel
    {
        // Identificación y Ficha activa
        public int Id { get; set; }
        public int FichaIdActual { get; set; }
        public string FichaCodigo { get; set; }
        public string EtiquetaEstado { get; set; }
        public string FotoUrl { get; set; }

        // Datos Personales y Contacto
        public string NombreCompleto { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string TipoIdentificacion { get; set; }
        public string NumeroIdentificacion { get; set; }
        public string CorreoCorporativo { get; set; }
        public string TelefonoParticular { get; set; }
        public string TelefonoOficina { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public int Edad { get; set; }
        public string Direccion { get; set; }
        public DateTime FechaIngreso { get; set; }
        public string AntiguedadTexto { get; set; }

        // Previsión y Seguridad Social
        public string EpsPrevision { get; set; }
        public string FondoPensiones { get; set; }

        // Puesto y Organización (Pestaña Resumen)
        public string Cargo { get; set; }
        public string Area { get; set; }
        public string Division { get; set; }
        public string Empresa { get; set; }
        public string CentroCostos { get; set; }
        public string Supervisor { get; set; }
        public string Suplente { get; set; }
        public string TipoContrato { get; set; }
        public string JornadaLaboral { get; set; }
        public decimal SaldoVacacionesDias { get; set; }

        // Listas secundarias pobladas por los ResultSets del SP
        public List<FichaItemViewModel> FichasDisponibles { get; set; } = new List<FichaItemViewModel>();
        public List<ComprobanteItemViewModel> Comprobantes { get; set; } = new List<ComprobanteItemViewModel>();
        public List<DocumentoItemViewModel> Documentos { get; set; } = new List<DocumentoItemViewModel>();
        public List<VacacionItemViewModel> Vacaciones { get; set; } = new List<VacacionItemViewModel>();
    }

    /// <summary>
    /// Representa cada opción disponible en el menú desplegable de selección de fichas.
    /// </summary>
    public class FichaItemViewModel
    {
        public int FichaId { get; set; }
        public string FichaCodigo { get; set; }
        public string FichaNombre { get; set; }
        public bool EsPrincipal { get; set; }
    }

    /// <summary>
    /// Detalle de liquidaciones y nómina mensual para la pestaña Comprobantes.
    /// </summary>
    public class ComprobanteItemViewModel
    {
        public int Id { get; set; }
        public string PeriodoMes { get; set; }
        public decimal SueldoBruto { get; set; }
        public decimal TotalNeto { get; set; }
        public decimal AportePatronal { get; set; }
        public decimal HorasExtras { get; set; }
        public decimal HorasRecargos { get; set; }
        public decimal HorasNoTrabajadas { get; set; }
        public decimal DiasNoTrabajados { get; set; }
        public string RutaArchivoPdf { get; set; }
    }

    /// <summary>
    /// Archivos digitales adjuntos para la pestaña Documentos.
    /// </summary>
    public class DocumentoItemViewModel
    {
        public int Id { get; set; }
        public string NombreDocumento { get; set; }
        public string Categoria { get; set; }
        public string RutaArchivo { get; set; }
        public DateTime FechaCarga { get; set; }
    }

    /// <summary>
    /// Registro de ausencias y descansos remunerados para la pestaña Vacaciones.
    /// </summary>
    public class VacacionItemViewModel
    {
        public int Id { get; set; }
        public string PeriodoCalculo { get; set; }
        public decimal DiasTomados { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public DateTime FechaRetorno { get; set; }
        public string Estado { get; set; }
        public string Observaciones { get; set; }
    }
}