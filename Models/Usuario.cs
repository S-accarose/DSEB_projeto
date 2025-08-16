using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace DSEB_projeto.Models;

public class Usuario : IdentityUser
{
    [Required]
    [StringLength(100)]
    [MinLength(3)]
    [MaxLength(100)]
    public string? Nome { get; set; }

    // Navigation properties
    public ICollection<Pedido>? Pedidos { get; set; }
}