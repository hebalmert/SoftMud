using Spix.Core.Entities;
using Spix.CoreShared.Enum;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Spix.Core.EntitiesGen;

public class Plan
{
    [Key]
    public Guid PlanId { get; set; }

    [Required(ErrorMessage = "La {0} es Obligatorio")]
    [DisplayName("Categoria")]
    public Guid PlanCategoryId { get; set; }

    [Display(Name = "Plan")]
    [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public string PlanName { get; set; } = null!;

    [Required(ErrorMessage = "La {0} es Obligatorio")]
    [Display(Name = "UpLoad")]
    public int SpeedUp { get; set; }

    [Display(Name = "Medida")]
    public SpeedUpType SpeedUpType { get; set; }

    [Required(ErrorMessage = "La {0} es Obligatorio")]
    [Display(Name = "Download")]
    public int SpeedDown { get; set; }

    [Display(Name = "Medida")]
    public SpeedDownType SpeedDownType { get; set; }

    //[Required(ErrorMessage = "La {0} es Obligatorio")]
    [Range(1, 12, ErrorMessage = "Debe Seleccionar un {0}")]
    [Display(Name = "Tasa Reuso de 1 a ...")]
    public int TasaReuso { get; set; }

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

    public PlanCategory? PlanCategory { get; set; }

    public Tax? Tax { get; set; }
}