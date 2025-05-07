using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Spix.CoreShared.Enum;

namespace Spix.Core.Entities;

public class Manager
{
    [Key]
    public int ManagerId { get; set; }

    [Required(ErrorMessage = "El Campo {0} es Obligatorio")]
    [MaxLength(50, ErrorMessage = "El campo no puede ser mayor a {0} de largo")]
    [Display(Name = "Nombre")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "El Campo {0} es Obligatorio")]
    [MaxLength(50, ErrorMessage = "El campo no puede ser mayor a {1} de largo")]
    [Display(Name = "Apellido")]
    public string LastName { get; set; } = null!;

    [MaxLength(101, ErrorMessage = "El campo no puede ser mayor a {1} de largo")]
    [Display(Name = "Nombre")]
    public string? FullName { get; set; }

    [MaxLength(15, ErrorMessage = "El Maximo de caracteres es {0}")]
    [Required(ErrorMessage = "El {0} es Obligatorio")]
    [Display(Name = "RUC ó DNI")]
    public string? Nro_Document { get; set; }

    [Required(ErrorMessage = "El {0} es Obligatorio")]
    [MaxLength(25, ErrorMessage = "El {0} no puede tener mas de {1} Caracteres.")]
    [Display(Name = "Telefono")]
    public string PhoneNumber { get; set; } = null!;

    [Required(ErrorMessage = "El {0} es Obligatorio")]
    [MaxLength(256, ErrorMessage = "El campo no puede ser mayor a {0} de largo")]
    [DataType(DataType.MultilineText)]
    [Display(Name = "Direccion")]
    public string Address { get; set; } = null!;

    //Correo y Coirporation
    [MaxLength(256, ErrorMessage = "El campo no puede ser mayor a {0} de largo")]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email")]
    public string UserName { get; set; } = null!;

    [Required]
    [Display(Name = "Corporacion")]
    public int CorporationId { get; set; }

    [Required(ErrorMessage = "El {0} es Obligatorio")]
    [MaxLength(50, ErrorMessage = "El {0} no puede tener mas de {1} Caracteres.")]
    [Display(Name = "Puesto Trabajo")]
    public string Job { get; set; } = null!;

    [Display(Name = "Tipo Usuario")]
    public UserType UserType { get; set; }

    [Display(Name = "Foto")]
    public string? Imagen { get; set; }

    [Display(Name = "Activo")]
    public bool Active { get; set; }

    //TODO: Cambio de ruta para Imagenes
    public string ImageFullPath => Imagen == string.Empty || Imagen == null
        ? $"https://localhost:7224/Images/NoPicture.png"
        : $"https://localhost:7224/Images/ImgManager/{Imagen}";

    [NotMapped]
    public string? ImgBase64 { get; set; }

    //Relaciones
    public Corporation? Corporation { get; set; }
}