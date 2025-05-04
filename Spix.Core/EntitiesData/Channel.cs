using Spix.Core.EntitiesNet;
using System.ComponentModel.DataAnnotations;

namespace Spix.Core.EntitiesData;

public class Channel
{
    [Key]
    public int ChannelId { get; set; }

    [Required(ErrorMessage = "El {0} es Obligatorio")]
    [MaxLength(10, ErrorMessage = "El {0} no puede tener mas de {1} Caracteres.")]
    [Display(Name = "Canal Wireless")]
    public string ChannelName { get; set; } = null!;

    [Display(Name = "Activo")]
    public bool Active { get; set; }

    //Relaciones

    public ICollection<Node>? Nodes { get; set; }
}