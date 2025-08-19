using DSEB_projeto.Data;
using DSEB_projeto.Models;


namespace DSEB_projeto.Services
{
    public class ServicosSeeder
    {
        private readonly ApplicationDbContext _context;
        

        public ServicosSeeder(ApplicationDbContext context)
        {
            _context = context;
        }

        public void GerarProdutos(int quantidade)
        {
            var Servicos = new List<Servico>();

            for (int i = 1; i <= quantidade; i++)
            {
                Servicos.Add(new Servico
                {
                    Nome = $"{i}",
                    Preco = 10 + i,
                    Descricao = $"Produto {i}"
                });
            }

            _context.Servicos.AddRange(Servicos);
            _context.SaveChanges();
        }
    }
}