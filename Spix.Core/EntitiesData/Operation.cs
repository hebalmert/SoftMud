using Spix.Core.EntitiesNet;
using System.ComponentModel.DataAnnotations;

namespace Spix.Core.EntitiesData;

public class Operation
{
    [Key]
    public int OperationId { get; set; }

    [Required(ErrorMessage = "El {0} es Obligatorio")]
    [MaxLength(50, ErrorMessage = "El {0} no puede tener mas de {1} Caracteres.")]
    [Display(Name = "Tipo Operacion")]
    public string OperationName { get; set; } = null!;

    [Display(Name = "Activo")]
    public bool Active { get; set; }

    //Relaciones

    public ICollection<Node>? Nodes { get; set; }
}