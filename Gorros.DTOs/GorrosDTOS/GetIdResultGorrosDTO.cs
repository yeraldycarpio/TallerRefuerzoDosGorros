using System.ComponentModel.DataAnnotations;

namespace Gorros.DTOs.GorrosDTOS
{
    public class GetIdResultGorrosDTO
    {
            public int Id { get; set; }

            [Display(Name = "Nombre")]
            public string Nombre { get; set; }

            [Display(Name = "Color")]
            public string? Color { get; set; }

            [Display(Name = "Material")]
            public string? Material { get; set; }

            [Display(Name = "Precio")]
            public decimal Precio { get; set; }
    }
}
