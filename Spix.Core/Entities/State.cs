using System.ComponentModel.DataAnnotations;

namespace Spix.Core.Entities;

public class State
{
    [Key]
    public int StateId { get; set; }

    public int CountryId { get; set; }

    [MaxLength(100)]
    [Required]
    public string Name { get; set; } = null!;

    public int CitiesNumber => Cities == null ? 0 : Cities.Count;

    //Relaciones
    public Country? Country { get; set; }

    public ICollection<City>? Cities { get; set; }
}