using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using DSEB_projeto.Areas.Identity.Pages.Account; 
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DSEB_projeto.Services;

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

    public string? ImagemPerfil { get; set; }

    [Required]
    [StringLength(11, MinimumLength = 11, ErrorMessage = "O CPF deve conter exatamente 11 caracteres.")]
    [RegularExpression("^[0-9]+$", ErrorMessage = "Por favor, insira apenas números.")]
    [CustomValidation(typeof(ValidationCPF), nameof(ValidationCPF.IsValidCPF), ErrorMessage = "CPF inválido.")]
    public string CPF { get; set; }

}