using System.Collections.Generic;
using LogicBo; // O el namespace donde tengas ClienteTarifaItemViewModel

namespace WebApplication1.Models
{
    public class ClientePerfilViewModel
    {
        public int Id { get; set; }
        public string CodigoFicha { get; set; }
        public string EstadoEtiqueta { get; set; }
        public string LogoUrl { get; set; }

        // Pestaña: Datos Personales
        public string Naturaleza { get; set; }
        public string Tipo { get; set; }
        public string RazonSocial { get; set; }
        public string EstablecimientoComercial { get; set; }
        public string NombreComercial { get; set; }
        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string Telefono1 { get; set; }
        public string Telefono2 { get; set; }
        public string CorreoPrincipal { get; set; }
        public string Correo2 { get; set; }
        public string Direccion { get; set; }

        // Pestaña: Datos Ubicación
        public string Origen { get; set; }
        public string Pais { get; set; }
        public string Departamento { get; set; }
        public string Ciudad { get; set; }

        // Pestaña: Cobros
        public string TipoCobro { get; set; }
        public int? DiaCierreFacturacion { get; set; }
        public List<ClienteTarifaItemViewModel> Tarifas { get; set; } = new List<ClienteTarifaItemViewModel>();

        // Pestaña: Datos Fiscales
        public string ActividadEconomica { get; set; }
        public string Industria { get; set; }
        public string RutaCamaraComercio { get; set; }
        public string RutaRut { get; set; }
    }
}