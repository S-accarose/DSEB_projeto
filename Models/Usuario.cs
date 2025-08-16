using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace DSEB_projeto.Models;

public class Usuario : IdentityUser
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    [MinLength(3)]
    [MaxLength(100)]
    public string Nome { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; }

    [Required]
    [StringLength(100)]
    public string Senha { get; set; }

    // Navigation properties
    public ICollection<Pedido>? Pedidos { get; set; }
}