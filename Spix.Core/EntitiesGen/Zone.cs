using Spix.Core.Entities;
using Spix.Core.EntitiesNet;
using System.ComponentModel.DataAnnotations;

namespace Spix.Core.EntitiesGen;

public class Zone
{
    [Key]
    public Guid ZoneId { get; set; }

    [Required(ErrorMessage = "El Campo {0} es Requerido")]
    [Display(Name = "Depart/Estado")]
    public int StateId { get; set; }

    [Required(ErrorMessage = "El Campo {0} es Requerido")]
    [Display(Name = "Ciudad")]
    public int CityId { get; set; }

    [Display(Name = "Zona")]
    [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public string ZoneName { get; set; } = null!;

    [Display(Name = "Activo")]
    public bool Active { get; set; }

    //Relaciones
    public int CorporationId { get; set; }

    public Corporation? Corporation { get; set; }

    public State? state { get; set; }

    public City? city { get; set; }

    public ICollection<Node>? Nodes { get; set; }
}