using System.ComponentModel.DataAnnotations;

namespace Gorros.DTOs.GorrosDTOS
{
    public class EditGorrosDTO
    {
        public EditGorrosDTO(GetIdResultGorrosDTO get)
        {
            Id = get.Id;
            Nombre = get.Nombre;
            Color = get.Color;
            Material = get.Material;
            Precio = get.Precio;
        }

        public EditGorrosDTO()
        {
            Nombre = string.Empty;
        }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener mas de 50 caracteres")]
        public string Nombre { get; set; }

        [Display(Name = "Color")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener mas de 50 caracteres")]
        public string? Color { get; set; }

        [Display(Name = "Material")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener mas de 50 caracteres")]
        public string? Material { get; set; }

        [Display(Name = "Precio")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public decimal Precio { get; set; }
    }
}
