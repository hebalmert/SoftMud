using System.ComponentModel.DataAnnotations;

namespace Spix.Core.Entities;

public class City
{
    [Key]
    public int CityId { get; set; }

    public int StateId { get; set; }

    [MaxLength(100)]
    [Required]
    public string Name { get; set; } = null!;

    //Relaciones
    public State? State { get; set; }
}