using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models.Entities
{
    [Table("Areas")]
    public class Area
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(150)]
        public string Nombre { get; set; }
    }

    [Table("Cargos")]
    public class Cargo
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(150)]
        public string Titulo { get; set; }
    }

    [Table("EpsPrevisiones")]
    public class EpsPrevision
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(150)]
        public string Nombre { get; set; }
    }
}