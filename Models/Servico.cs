using System.ComponentModel.DataAnnotations;
namespace DSEB_projeto.Models;

public class Servico
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Nome { get; set; }

    [Required]
    public decimal Preco { get; set; }

    [Required]
    public string? Descricao { get; set; }

    [Required]
    public string? ImagemString { get; set; }

    // Navigation properties
    public ICollection<Pedido>? Pedidos { get; set; }
}