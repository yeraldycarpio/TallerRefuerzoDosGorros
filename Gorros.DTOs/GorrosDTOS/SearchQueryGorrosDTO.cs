using System.ComponentModel.DataAnnotations;

namespace Gorros.DTOs.GorrosDTOS
{
    public class SearchQueryGorrosDTO
    {
        [Display(Name = "Nombre")]
        public string? Nombre_Like { get; set; }

        [Display(Name = "Color")]
        public string? Color_Like { get; set; }

        [Display(Name = "Material")]
        public string? Material_Like { get; set; }

        [Display(Name = "Precio")]
        public decimal? Precio_Like { get; set; }

        [Display(Name = "Pagina")]
        public int Skip { get; set; }

        [Display(Name = "CantReg x Pagina")]
        public int Take { get; set; }

        public byte SendRowCount { get; set; }
    }
}
