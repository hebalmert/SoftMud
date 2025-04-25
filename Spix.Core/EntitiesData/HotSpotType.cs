using System.ComponentModel.DataAnnotations;

namespace Spix.Core.EntitiesData;

public class HotSpotType
{
    [Key]
    public int HotSpotTypeId { get; set; }

    [Required(ErrorMessage = "El {0} es Obligatorio")]
    [MaxLength(25, ErrorMessage = "El {0} no puede tener mas de {1} Caracteres.")]
    [Display(Name = "Tipo HotSpot")]
    public string TypeName { get; set; } = null!;

    [Display(Name = "Activo")]
    public bool Active { get; set; }
}