using Spix.CoreShared.Enum;
using System.ComponentModel.DataAnnotations;

namespace Spix.Core.Entities;

public class UserRoleDetails
{
    [Key]
    public int UserRoleDetailsId { get; set; }

    [Display(Name = "Rol Usuario")]
    public UserType? UserType { get; set; }

    [Display(Name = "User Id")]
    public string? UserId { get; set; }

    //Relacion
    public User? User { get; set; }
}