using Spix.Core.Entities;
using Spix.Core.EntitiesOper;
using System.ComponentModel.DataAnnotations;

namespace Spix.Core.EntitiesGen;

public class DocumentType
{
    [Key]
    public Guid DocumentTypeId { get; set; }

    [Display(Name = "Documento")]
    [MaxLength(5, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public string DocumentName { get; set; } = null!;

    [MaxLength(200, ErrorMessage = "El {0} no puede tener mas de {1} Caracteres.")]
    [DataType(DataType.MultilineText)]
    [Display(Name = "Descripcion")]
    public string? Descripcion { get; set; }

    [Display(Name = "Activo")]
    public bool Active { get; set; }

    //Relaciones
    public int CorporationId { get; set; }

    public Corporation? Corporation { get; set; }

    public ICollection<Client>? Clients { get; set; }
}