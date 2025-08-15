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

    [Required]
    public decimal ValorPedidos { get; set; }

    [ForeignKey("ValorPedidos")]
    [Required]
    public Pedido Valor { get; set; }

    // Navigation properties
    public ICollection<Pedido>? Pedidos { get; set; }

    public ICollection<Servico>? Servicos { get; set; }

    public ICollection<Usuario>? Usuarios { get; set; }
}