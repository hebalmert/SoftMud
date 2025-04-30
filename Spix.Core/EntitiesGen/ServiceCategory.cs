using Spix.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace Spix.Core.EntitiesGen;

public class ServiceCategory
{
    [Key]
    public Guid ServiceCategoryId { get; set; }

    [Display(Name = "Categoria")]
    [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public string Name { get; set; } = null!;

    [Display(Name = "Activo")]
    public bool Active { get; set; }

    //Propiedad Virtual de Consulta
    [Display(Name = "Servicios")]
    public int ServicesNumer => ServiceClients == null ? 0 : ServiceClients.Count;

    //Relaciones
    public int CorporationId { get; set; }

    public Corporation? Corporation { get; set; }

    public ICollection<ServiceClient>? ServiceClients { get; set; }
}