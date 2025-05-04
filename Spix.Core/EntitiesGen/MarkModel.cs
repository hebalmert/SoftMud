using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Spix.Core.Entities;
using Spix.Core.EntitiesNet;

namespace Spix.Core.EntitiesGen;

public class MarkModel
{
    [Key]
    public Guid MarkModelId { get; set; }

    [Display(Name = "Modelo")]
    [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public string MarkModelName { get; set; } = null!;

    public Guid MarkId { get; set; }

    [Display(Name = "Activo")]
    public bool Active { get; set; }

    [NotMapped]
    [Display(Name = "Marca")]
    public string? MarkName { get; set; }

    //Relaciones
    public int CorporationId { get; set; }

    public Corporation? Corporation { get; set; }

    public Mark? Mark { get; set; }

    public ICollection<Node>? Nodes { get; set; }
}