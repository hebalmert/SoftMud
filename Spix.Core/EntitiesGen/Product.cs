using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Spix.Core.Entities;
using Spix.Core.EntitiesInven;

namespace Spix.Core.EntitiesGen;

public class Product
{
    [Key]
    public Guid ProductId { get; set; }

    [Required(ErrorMessage = "El {0} es Obligatorio")]
    [Display(Name = "Categoria")]
    public Guid ProductCategoryId { get; set; }

    [Display(Name = "Nombre")]
    [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public string ProductName { get; set; } = null!;

    [DataType(DataType.MultilineText)]
    [Display(Name = "Descripción")]
    [MaxLength(500, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
    public string Description { get; set; } = null!;

    [Column(TypeName = "decimal(18,2)")]
    [DisplayFormat(DataFormatString = "{0:C2}")]
    [Display(Name = "Costo")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public decimal Costo { get; set; }

    [Required(ErrorMessage = "El {0} es Obligatorio")]
    [Display(Name = "Impuesto")]
    public Guid TaxId { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    [DisplayFormat(DataFormatString = "{0:C2}")]
    [Display(Name = "Precio Venta Sin Iva")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public decimal Price { get; set; }

    [Display(Name = "Seriales")]
    public bool WithSerials { get; set; }

    [Display(Name = "Activo")]
    public bool Active { get; set; }

    //Relaciones
    public int CorporationId { get; set; }

    public Corporation? Corporation { get; set; }

    public ProductCategory? ProductCategory { get; set; }

    public Tax? Tax { get; set; }

    //Propiedades Virtuales

    public decimal TotalInventario => ProductStocks == null ? 0 : ProductStocks.Sum(x => x.Stock);

    //Releaciones en dos vias

    public ICollection<ProductStock>? ProductStocks { get; set; }

    public ICollection<PurchaseDetail>? PurchaseDetails { get; set; }

    public ICollection<TransferDetails>? TransferDetails { get; set; }

    public ICollection<Cargue>? Cargue { get; set; }
}