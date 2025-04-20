using System.ComponentModel.DataAnnotations;

namespace Spix.Core.Entities;

public class Country
{
    [Key]
    public int CountryId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    [MaxLength(10)]
    public string? CodPhone { get; set; }

    public int StatesNumber => States == null ? 0 : States.Count;

    //relaciones
    public ICollection<State>? States { get; set; }

    public ICollection<Corporation>? Corporations { get; set; }
}