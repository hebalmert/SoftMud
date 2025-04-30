using Spix.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace Spix.Core.EntitiesGen;

public class PlanCategory
{
    [Key]
    public Guid PlanCategoryId { get; set; }

    [Display(Name = "Categoria")]
    [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public string PlanCategoryName { get; set; } = null!;

    [Display(Name = "Activo")]
    public bool Active { get; set; }

    //Propiedad Virtual de Consulta
    [Display(Name = "Planes")]
    public int PlanesNumer => Plans == null ? 0 : Plans.Count;

    //Relaciones
    public int CorporationId { get; set; }

    public Corporation? Corporation { get; set; }

    public ICollection<Plan>? Plans { get; set; }
}