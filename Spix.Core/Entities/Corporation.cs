﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Spix.Core.EntitesSoftSec;

namespace Spix.Core.Entities;

public class Corporation
{
    [Key]
    public int CorporationId { get; set; }

    [MaxLength(100, ErrorMessage = "El Maximo de caracteres es {0}")]
    [Required(ErrorMessage = "El campo {0} es Requerido")]
    [Display(Name = "Empresa/Persona")]
    public string? Name { get; set; }

    [MaxLength(15, ErrorMessage = "El Maximo de caracteres es {0}")]
    [Required(ErrorMessage = "El {0} es Obligatorio")]
    [Display(Name = "RUC ó DNI")]
    public string? NroDocument { get; set; }

    [MaxLength(12, ErrorMessage = "El Máximo de caracteres es {1}")]
    [Required(ErrorMessage = "El {0} es Obligatorio")]
    [DataType(DataType.PhoneNumber)]
    [Display(Name = "Teléfono")]
    public string? Phone { get; set; }

    [MaxLength(200, ErrorMessage = "El campo no puede ser mayor a {1} de largo")]
    [Required(ErrorMessage = "El {0} es Obligatorio")]
    [Display(Name = "Dirección")]
    public string? Address { get; set; }

    [Required]
    [Display(Name = "Pais")]
    public int CountryId { get; set; }

    [Required]
    [Display(Name = "Plan de Software")]
    public int SoftPlanId { get; set; }

    //Tiempo Activo de la cuenta
    [Required(ErrorMessage = "El {0} es Obligatorio")]
    [Display(Name = "Inicio")]
    public DateTime DateStart { get; set; }

    [Required(ErrorMessage = "El {0} es Obligatorio")]
    [Display(Name = "Vencimiento")]
    public DateTime DateEnd { get; set; }

    [Display(Name = "Logo")]
    public string? Imagen { get; set; }

    [Display(Name = "Activo")]
    public bool Active { get; set; }

    //TODO: Cambio de ruta para Imagenes
    public string ImageFullPath => Imagen == string.Empty || Imagen == null
        ? $"https://localhost:7224/Images/NoImage.png"
        : $"https://localhost:7224/Images/ImgCorporation/{Imagen}";

    [NotMapped]
    public string? ImgBase64 { get; set; }

    //Relaciones
    public SoftPlan? SoftPlan { get; set; }

    public Country? Country { get; set; }

    public ICollection<Manager>? Managers { get; set; }

    public ICollection<Usuario>? Usuarios { get; set; }

    public ICollection<UsuarioRole>? UsuarioRoles { get; set; }
}