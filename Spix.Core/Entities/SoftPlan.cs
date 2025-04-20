using System.ComponentModel.DataAnnotations;

namespace Spix.Core.Entities;

public class SoftPlan
{
    [Key]
    public int SoftPlanId { get; set; }

    [MaxLength(50)]
    [Required]
    public string? Name { get; set; }

    public decimal Price { get; set; }

    [Required]
    public int Meses { get; set; }

    [Required]
    public int ClientCount { get; set; }

    public bool Active { get; set; }

    //Releaciones
    public ICollection<Corporation>? Corporations { get; set; }
}