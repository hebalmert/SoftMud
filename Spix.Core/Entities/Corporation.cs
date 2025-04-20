using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Spix.Core.Entities;

public class Corporation
{
    [Key]
    public int CorporationId { get; set; }

    [Display(Name = "Logo")]
    public string? Imagen { get; set; }

    [MaxLength(100)]
    [Required]
    public string? Name { get; set; }

    [MaxLength(15)]
    [Required]
    public string? NroDocument { get; set; }

    [MaxLength(12)]
    [Required]
    public string? Phone { get; set; }

    [MaxLength(200)]
    [Required]
    public string? Address { get; set; }

    public int CountryId { get; set; }

    [Required]
    public int SoftPlanId { get; set; }

    //Tiempo Activo de la cuenta
    [Required]
    public DateTime DateStart { get; set; }

    [Required]
    public DateTime DateEnd { get; set; }

    [Display(Name = "Activo")]
    public bool Active { get; set; }

    //TODO: Cambio de ruta para Imagenes
    //Propiedad virtual de Acceso a imagen
    public string ImageFullPath => Imagen == string.Empty || Imagen == null
        ? $"https://localhost:7204/Images/NoImage.png"
        : $"https://localhost:7204/Images/ImgCorporation/{Imagen}";

    [NotMapped]
    public string? ImgBase64 { get; set; }

    //Relaciones
    public SoftPlan? SoftPlan { get; set; }

    public Country? Country { get; set; }

    public ICollection<Manager>? Managers { get; set; }
}