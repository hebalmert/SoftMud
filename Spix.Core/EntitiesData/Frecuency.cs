using System.ComponentModel.DataAnnotations;

namespace Spix.Core.EntitiesData;

public class Frecuency
{
    [Key]
    public int FrecuencyId { get; set; }

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