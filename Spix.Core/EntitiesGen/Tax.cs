using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Spix.Core.Entities;

namespace Spix.Core.EntitiesGen;

public class Tax
{
    [Key]
    public Guid TaxId { get; set; }

    [MaxLength(50, ErrorMessage = "El Maximo de caracteres es {0}")]
    [Required(ErrorMessage = "El campo {0} es Requerido")]
    [Display(Name = "Impuesto")]
    public string TaxName { get; set; } = null!;

    [Range(0, 99, ErrorMessage = "EL Valor del {0} debe estar entre {1} y {2}")]
    [Required(ErrorMessage = "El campo {0} es Requerido")]
    [Column(TypeName = "decimal(5,2)")]
    [Display(Name = "Tasa")]
    public decimal Rate { get; set; }

    [Display(Name = "Activo")]
    public bool Active { get; set; }

    //Relaciones
    public int CorporationId { get; set; }

    public Corporation? Corporation { get; set; }

    public ICollection<Product>? Products { get; set; }
}