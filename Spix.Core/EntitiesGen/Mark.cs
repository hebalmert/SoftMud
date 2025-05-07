using Spix.Core.Entities;
using Spix.Core.EntitiesNet;
using System.ComponentModel.DataAnnotations;

namespace Spix.Core.EntitiesGen;

public class Mark
{
    [Key]
    public Guid MarkId { get; set; }

    [Display(Name = "Marca")]
    [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public string MarkName { get; set; } = null!;

    [Display(Name = "Activo")]
    public bool Active { get; set; }

    //Propiedad Virtual de Consulta
    [Display(Name = "Modelos")]
    public int ModelNumber => MarkModels == null ? 0 : MarkModels.Count;

    //Relaciones
    public int CorporationId { get; set; }

    public Corporation? Corporation { get; set; }

    public ICollection<MarkModel>? MarkModels { get; set; }

    public ICollection<Node>? Nodes { get; set; }

    public ICollection<Server>? Servers { get; set; }
}