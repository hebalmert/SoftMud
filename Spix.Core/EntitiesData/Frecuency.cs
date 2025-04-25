using System.ComponentModel.DataAnnotations;

namespace Spix.Core.EntitiesData;

public class Frecuency
{
    [Key]
    public int FrecuencyId { get; set; }

    [Range(1, double.MaxValue, ErrorMessage = "Debe Seleccionar un {0}")]
    [Required(ErrorMessage = "El {0} es Obligatorio")]
    [Display(Name = "Tipo Frecuencia")]
    public int FrecuencyTypeId { get; set; }

    [Required(ErrorMessage = "El {0} es Obligatorio")]
    [Display(Name = "Frecuencia")]
    public int FrecuencyName { get; set; }

    [Display(Name = "Activo")]
    public bool Active { get; set; }

    //Relaciones
    public FrecuencyType? FrecuencyType { get; set; }
}