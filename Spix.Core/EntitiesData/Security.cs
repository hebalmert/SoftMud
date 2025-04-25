using System.ComponentModel.DataAnnotations;

namespace Spix.Core.EntitiesData;

public class Security
{
    [Key]
    public int SecurityId { get; set; }

    [Required(ErrorMessage = "El {0} es Obligatorio")]
    [MaxLength(50, ErrorMessage = "El {0} no puede tener mas de {1} Caracteres.")]
    [Display(Name = "Tipo Seguridad")]
    public string SecurityName { get; set; } = null!;

    [Display(Name = "Activo")]
    public bool Active { get; set; }
}