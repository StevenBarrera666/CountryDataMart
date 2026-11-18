using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class CategoriaTarifaViewModel
    {
        public int Id { get; set; }

        [Required, StringLength(200)]
        public string Nombre { get; set; }
    }
}
