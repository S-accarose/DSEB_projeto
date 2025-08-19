using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using DSEB_projeto.Areas.Identity.Pages.Account; 
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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