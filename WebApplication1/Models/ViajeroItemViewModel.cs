using System;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    public class ViajeroItemViewModel
    {
        public int Id { get; set; }
        public string FichaCodigo { get; set; }
        public string NombreCompleto { get; set; }
        public string Amadeus_Nombre { get; set; }
        public string Identificacion_Tipo { get; set; }
        public string Identificacion_Numero { get; set; }
        public string Cliente_Cuenta { get; set; }
        public string Pasajero_Cargo { get; set; }
        public string Pasajero_Celular { get; set; }
        public string Pasajero_Correo { get; set; }
        public string Pasaporte_Numero { get; set; }
        public DateTime? Pasaporte_FechaCaducidad { get; set; }
    }

    public class ViajeroFormViewModel
    {
        public int Id { get; set; }
        public int? FichaId { get; set; }
        public string FichaCodigo { get; set; }

        // 1. Datos del Pasajero
        public string Pasajero_Titulo { get; set; } = "Sr.";
        public string Pasajero_Apellido { get; set; }
        public string Pasajero_Nombre { get; set; }
        public string Pasajero_Sexo { get; set; } = "Hombre";
        public string Pasajero_PaisNacimiento { get; set; } = "Colombia";
        public string Pasajero_Nacionalidad { get; set; } = "Colombiano";
        public DateTime? Pasajero_FechaNacimiento { get; set; }
        public string Pasajero_Cargo { get; set; }
        public string Pasajero_Tipo { get; set; } = "Adulto";
        public string Pasajero_CiudadResidencia { get; set; }

        // 2. Datos del Cliente / Cuenta Corporativa
        public string Cliente_Cuenta { get; set; }
        public string Cliente_ID { get; set; }
        public string Cliente_Estacion { get; set; }
        public string Cliente_OrdenInterna { get; set; }
        public string Cliente_Vicepresidencia { get; set; }
        public string Cliente_Gerencia { get; set; }
        public string Cliente_Correo { get; set; }

        // 3. Documentación e Identificación
        public string Identificacion_Tipo { get; set; } = "CC";
        public string Identificacion_Numero { get; set; }
        public string Pasaporte_Numero { get; set; }
        public string Pasaporte_PaisEmision { get; set; }
        public DateTime? Pasaporte_FechaEmision { get; set; }
        public DateTime? Pasaporte_FechaCaducidad { get; set; }
        public string Visa_Numero { get; set; }
        public string Visa_PaisEmision { get; set; }
        public DateTime? Visa_FechaEmision { get; set; }
        public DateTime? Visa_FechaCaducidad { get; set; }

        // 4. Contacto Personal
        public string Pasajero_Celular { get; set; }
        public string Pasajero_Whatsapp { get; set; }
        public string Pasajero_Correo { get; set; }

        // 5. Preferencias de Vuelo
        public string Pasajero_Cabina { get; set; }
        public string Pasajero_Clase { get; set; }
        public string Pasajero_Silla { get; set; }
        public string Pasajero_ViajeroFrecuenteAero { get; set; }
        public string Pasajero_ViajeroFrecuenteNumero { get; set; }

        // 6. GDS Amadeus
        public string Amadeus_Nombre { get; set; }

        // Catálogo de Fichas
        public List<FichaItemViewModel> FichasCatalogo { get; set; } = new List<FichaItemViewModel>();
    }
}