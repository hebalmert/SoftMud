using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Spix.CoreShared.Enum;

namespace Spix.Core.Entities;

public class Manager
{
    [Key]
    public int ManagerId { get; set; }

    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = null!;

    [Required]
    [MaxLength(50)]
    public string LastName { get; set; } = null!;

    [MaxLength(100)]
    public string? FullName { get; set; }

    [MaxLength(15)]
    [Required]
    public string? Nro_Document { get; set; }

    [Required]
    [MaxLength(25)]
    public string PhoneNumber { get; set; } = null!;

    [Required]
    [MaxLength(256)]
    [Display(Name = "Direccion")]
    public string Address { get; set; } = null!;

    //Correo y Coirporation
    [MaxLength(256)]
    public string UserName { get; set; } = null!;

    public int CorporationId { get; set; }

    [Required]
    [MaxLength(50)]
    public string Job { get; set; } = null!;

    public UserType UserType { get; set; }

    public string? Photo { get; set; }

    public bool Active { get; set; }

    //TODO: Cambio de ruta para Imagenes
    //Propiedad viertual de Acceso a imagen
    public string ImageFullPath => Photo == string.Empty || Photo == null
        ? $"https://localhost:7204/Images/NoImage.png"
        : $"https://localhost:7204/Images/ImgManager/{Photo}";

    [NotMapped]
    public string? ImgBase64 { get; set; }

    //Relaciones
    public Corporation? Corporation { get; set; }
}