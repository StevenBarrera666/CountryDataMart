using System;

namespace LogicBo
{
    /// <summary>Campos de dbo.Viajeros (el nombre de cada propiedad coincide con la columna).</summary>
    public class ViajeroRegistroViewModel
    {
        public int Id { get; set; }
        public int? FichaId { get; set; }
        public bool Activo { get; set; } = true;

        // Pasajero
        public string Pasajero_Titulo { get; set; }
        public string Pasajero_Apellido { get; set; }
        public string Pasajero_Nombre { get; set; }
        public string Pasajero_Sexo { get; set; }
        public string Pasajero_PaisNacimiento { get; set; }
        public string Pasajero_Nacionalidad { get; set; }
        public DateTime? Pasajero_FechaNacimiento { get; set; }
        public string Pasajero_Cargo { get; set; }
        public string Pasajero_Tipo { get; set; }
        public string Pasajero_CiudadResidencia { get; set; }

        // Cliente
        public string Cliente_Cuenta { get; set; }
        public string Cliente_ID { get; set; }
        public string Cliente_Estacion { get; set; }
        public string Cliente_OrdenInterna { get; set; }
        public string Cliente_Vicepresidencia { get; set; }
        public string Cliente_Gerencia { get; set; }
        public string Cliente_Correo { get; set; }

        // Identificacion
        public string Identificacion_Tipo { get; set; }
        public string Identificacion_Numero { get; set; }

        // Pasaporte
        public string Pasaporte_Numero { get; set; }
        public string Pasaporte_PaisEmision { get; set; }
        public DateTime? Pasaporte_FechaEmision { get; set; }
        public DateTime? Pasaporte_FechaCaducidad { get; set; }

        // Visa
        public string Visa_Numero { get; set; }
        public string Visa_PaisEmision { get; set; }
        public DateTime? Visa_FechaEmision { get; set; }
        public DateTime? Visa_FechaCaducidad { get; set; }

        // Contacto y preferencias
        public string Pasajero_Celular { get; set; }
        public string Pasajero_Whatsapp { get; set; }
        public string Pasajero_Correo { get; set; }
        public string Pasajero_Cabina { get; set; }
        public string Pasajero_Clase { get; set; }
        public string Pasajero_Silla { get; set; }
        public string Pasajero_ViajeroFrecuenteAero { get; set; }
        public string Pasajero_ViajeroFrecuenteNumero { get; set; }
        public string Amadeus_Nombre { get; set; }
    }
}
