using Spix.Core.EntitiesGen;
using Spix.Core.EntitiesInven;
using System.ComponentModel.DataAnnotations;

namespace Spix.Core.Entities;

public class State
{
    [Key]
    public int StateId { get; set; }

    public int CountryId { get; set; }

    [MaxLength(100, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [Display(Name = "Depart/Estado")]
    public string Name { get; set; } = null!;

    [Display(Name = "Ciudades")]
    public int CitiesNumber => Cities == null ? 0 : Cities.Count;

    //Relaciones
    public Country? Country { get; set; }

    public ICollection<City>? Cities { get; set; }

    public ICollection<Zone>? Zones { get; set; }

    public ICollection<ProductStorage>? ProductStorages { get; set; }

    public ICollection<Supplier>? Supplier { get; set; }
}