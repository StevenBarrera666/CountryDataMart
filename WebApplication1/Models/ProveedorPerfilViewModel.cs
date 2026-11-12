using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
        public class ProveedorPerfilViewModel
        {
            public int Id { get; set; }
            public string CodigoFicha { get; set; }
            public string EstadoEtiqueta { get; set; }
            public string LogoUrl { get; set; }

            // 1. Datos Personales
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

            // 2. Datos Ubicación
            public string Origen { get; set; }
            public string Pais { get; set; }
            public string Departamento { get; set; }
            public string Ciudad { get; set; }

            // 3. Cobros (Tarifa Única)
            public string TipoCobro { get; set; }
            public string CategoriaTarifa { get; set; }
            public decimal TarifaAdministrativa { get; set; }
            public decimal Descuentos { get; set; }

            // 4. Datos Fiscales y Bancarios
            public string ActividadEconomica { get; set; }
            public string Industria { get; set; }
            public string RutaCamaraComercio { get; set; }
            public string RutaRut { get; set; }
            public string RutaCertificadoBancario { get; set; }
        }
    }
