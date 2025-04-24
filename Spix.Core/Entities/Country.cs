using System.ComponentModel.DataAnnotations;

namespace Spix.Core.Entities;

public class Country
{
    [Key]
    public int CountryId { get; set; }

    [Required(ErrorMessage = "El Campo {0} es obligatorio")]
    [MaxLength(100, ErrorMessage = "El Campo {0} no puede ser mayor de {1} Caracteres")]
    [Display(Name = "Pais")]
    public string Name { get; set; } = null!;

    [MaxLength(10, ErrorMessage = "El campo {0} no puede tener mas de {1} Caracter")]
    [Display(Name = "Cod Phone")]
    public string? CodPhone { get; set; }

    [Display(Name = "Estados")]
    public int StatesNumber => States == null ? 0 : States.Count;

    //relaciones
    public ICollection<State>? States { get; set; }

    public ICollection<Corporation>? Corporations { get; set; }
}