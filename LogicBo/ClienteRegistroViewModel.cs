using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LogicBo
{
    public class ClienteRegistroViewModel
    {
        public int Id { get; set; } // 0 = Crear, >0 = Editar
        public string CodigoFicha { get; set; }
        public string EstadoEtiqueta { get; set; }
        public string LogoUrl { get; set; }

        // 1. Datos Personales
        [Required]
        public int NaturalezaId { get; set; }
        [Required]
        public int TipoId { get; set; }
        [Required]
        public string RazonSocial { get; set; }
        public string EstablecimientoComercial { get; set; }
        [Required]
        public string NombreComercial { get; set; }
        [Required]
        public int TipoDocumentoId { get; set; }
        [Required]
        public string NumeroDocumento { get; set; }
        [Required]
        public string Telefono1 { get; set; }
        public string Telefono2 { get; set; }
        [Required]
        [EmailAddress]
        public string CorreoPrincipal { get; set; }
        public string Correo2 { get; set; }
        [Required]
        public string Direccion { get; set; }
        public string CodigoPostal { get; set; }

        // 2. Datos Ubicación
        [Required]
        public string Origen { get; set; } // Nacional / Internacional
        [Required]
        public int PaisId { get; set; }
        [Required]
        public int DepartamentoId { get; set; }
        [Required]
        public int CiudadId { get; set; }

        // 3. Cobros
        [Required]
        public int TipoCobroId { get; set; }
        public int? DiaCierreFacturacion { get; set; }
        [Required]
        public int CategoriaTarifaId { get; set; }
        public decimal TarifaAdministrativa { get; set; }
        public decimal Descuentos { get; set; }

        // 4. Datos Fiscales
        [Required]
        public int ActividadEconomicaId { get; set; }
        [Required]
        public int IndustriaId { get; set; }
        public string RutaCamaraComercio { get; set; }
        public string RutaRut { get; set; }

        // Dentro de ClienteRegistroViewModel:
        public List<ClienteTarifaItemViewModel> Tarifas { get; set; } = new List<ClienteTarifaItemViewModel>();
    }
}