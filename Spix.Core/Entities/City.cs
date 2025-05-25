using Spix.Core.EntitiesGen;
using Spix.Core.EntitiesInven;
using System.ComponentModel.DataAnnotations;

namespace Spix.Core.Entities;

public class City
{
    [Key]
    public int CityId { get; set; }

    public int StateId { get; set; }

    [MaxLength(100, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [Display(Name = "Ciudad")]
    public string Name { get; set; } = null!;

    //Relaciones
    public State? State { get; set; }

    public ICollection<Zone>? Zones { get; set; }

    public ICollection<ProductStorage>? ProductStorages { get; set; }

    public ICollection<Supplier>? Supplier { get; set; }
}