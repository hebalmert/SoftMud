using Spix.Core.Entities;
using Spix.Core.EntitiesContratos;
using System.ComponentModel.DataAnnotations;

namespace Spix.Core.EntitiesGen;

public class ServiceClient
{
    [Key]
    public Guid ServiceClientId { get; set; }

    [Required(ErrorMessage = "El {0} es Obligatorio")]
    [Display(Name = "Categoria")]
    public Guid ServiceCategoryId { get; set; }

    [Display(Name = "Nombre")]
    [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public string ServiceName { get; set; } = null!;

    [Display(Name = "Descripción")]
    [MaxLength(500, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
    public string Description { get; set; } = null!;

    [DisplayFormat(DataFormatString = "{0:C2}")]
    [Display(Name = "Costo")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public decimal Costo { get; set; }

    [Required(ErrorMessage = "El {0} es Obligatorio")]
    [Display(Name = "Impuesto")]
    public Guid TaxId { get; set; }

    [DisplayFormat(DataFormatString = "{0:C2}")]
    [Display(Name = "Precio Venta Sin Iva")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public decimal Price { get; set; }

    [Display(Name = "Activo")]
    public bool Active { get; set; }

    //Relaciones
    public int CorporationId { get; set; }

    public Corporation? Corporation { get; set; }

    public ServiceCategory? ServiceCategory { get; set; }

    public Tax? Tax { get; set; }

    public ICollection<ContractClient>? ContractClients { get; set; }
}