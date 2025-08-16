using DSEB_projeto.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DSEB_projeto.Data;

public class ApplicationDbContext : IdentityDbContext<Usuario>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Servico> Servicos { get; set; }
        public DbSet<Receita> Receitas { get; set; }
}
