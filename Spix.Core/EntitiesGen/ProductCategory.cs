using Spix.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace Spix.Core.EntitiesGen;

public class ProductCategory
{
    [Key]
    public Guid ProductCategoryId { get; set; }

    [Display(Name = "Categoria")]
    [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public string Name { get; set; } = null!;

    [Display(Name = "Activo")]
    public bool Active { get; set; }

    //Propiedad Virtual de Consulta
    [Display(Name = "Productos")]
    public int ProductsNumer => Products == null ? 0 : Products.Count;

    //Relaciones
    public int CorporationId { get; set; }

    public Corporation? Corporation { get; set; }

    public ICollection<Product>? Products { get; set; }
}