using System.ComponentModel.DataAnnotations;

namespace Spix.Core.EntitiesData;

public class FrecuencyType
{
    [Key]
    public int FrecuencyTypeId { get; set; }

    [Required(ErrorMessage = "El {0} es Obligatorio")]
    [MaxLength(10, ErrorMessage = "El {0} no puede tener mas de {1} Caracteres.")]
    [Display(Name = "Tipo Frecuencia")]
    public string TypeName { get; set; } = null!;

    [Display(Name = "Activo")]
    public bool Active { get; set; }

    //Propiedad Virtual de consulta
    [Display(Name = "Frecuencias")]
    public int TotalFrecuencia => Frecuencies == null ? 0 : Frecuencies.Count;

    //Relaciones
    public ICollection<Frecuency>? Frecuencies { get; set; }
}