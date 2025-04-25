using System.ComponentModel.DataAnnotations;

namespace Spix.Core.EntitiesData;

public class ChainType
{
    [Key]
    public int ChainTypeId { get; set; }

    [Required(ErrorMessage = "El {0} es Obligatorio")]
    [MaxLength(25, ErrorMessage = "El {0} no puede tener mas de {1} Caracteres.")]
    [Display(Name = "Tipo Chain")]
    public string ChainName { get; set; } = null!;

    [Display(Name = "Activo")]
    public bool Active { get; set; }
}