using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DSEB_projeto.Models;

public class Pedido
{
    [Key]
    public int Id { get; set; }
    public string? Descricao { get; set; }
    public decimal Valor { get; set; }
    public DateTime DataPedido { get; set; }
    public DateTime DataEntrega { get; set; }
    // Relação com o usuário que fez o pedido
    [Required]
    public string UsuarioId { get; set; }

    [ForeignKey("UsuarioId")]
    public Usuario Usuario { get; set; }
}