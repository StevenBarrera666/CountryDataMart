namespace WebApplication1.Models
{
    public class CabinaVueloViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string NombreIngles { get; set; }
        public string CodigoGDS { get; set; }
        public int? Orden { get; set; }
        public bool Activo { get; set; } = true;
    }
}
