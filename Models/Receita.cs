using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;
namespace DSEB_projeto.Models;

public class Receita
{
    [Key]
    public int Id { get; set; }

    [Required]
    public DateTime DataReceita { get; set; }

    [Required]
    public decimal Lucro { get; set; }

    // Soma dos valores dos pedidos relacionados a esta receita
    [Required]
    public decimal ValorPedidos { get; set; }

    // Uma receita tem vários pedidos
    public ICollection<Pedido>? Pedidos { get; set; }
}