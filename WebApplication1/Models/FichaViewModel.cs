using System;

namespace WebApplication1.Models
{
    public class FichaViewModel
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public bool Activo { get; set; } = true;
    }
}
