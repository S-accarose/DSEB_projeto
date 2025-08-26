using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DSEB_projeto.Data;
using DSEB_projeto.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace DSEB_projeto.Controllers
{
    [Authorize]
    public class PedidosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Usuario> _userManager;

        // Apenas este construtor!
        public PedidosController(ApplicationDbContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Pedidos
        public async Task<IActionResult> Index()
        {
            var usuario = await _userManager.GetUserAsync(User);
            if (usuario == null)
            {
                // Usuário não autenticado, retorna lista vazia ou redireciona
                return View(new List<Pedido>());
            }

            var pedidos = await _context.Pedidos
                .Where(p => p.UsuarioId == usuario.Id)
                .OrderByDescending(p => p.DataPedido)
                .ToListAsync();
            var usuarios = _userManager.Users.ToList();
            var usuariosNaoAdmin = new List<Usuario>();
            foreach (var usuario1 in usuarios)
            {
                var roles = await _userManager.GetRolesAsync(usuario1);
                if (!roles.Contains("Admin"))
                {
                    usuariosNaoAdmin.Add(usuario1);
                }
            }

            ViewBag.Usuarios = usuariosNaoAdmin;

            return View(pedidos);
        }

        // GET: Pedidos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

        // GET: Pedidos/Create
        public IActionResult Create()
        {
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Id");
            return View();
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Pedidos(string usuarioId)
        {
            if (string.IsNullOrEmpty(usuarioId))
                return NotFound();

            var usuario = await _userManager.FindByIdAsync(usuarioId);
            if (usuario == null)
                return NotFound();

            var pedidos = await _context.Pedidos
                .Where(p => p.UsuarioId == usuarioId)
                .OrderByDescending(p => p.DataPedido)
                .ToListAsync();

            ViewBag.Usuario = usuario;
            return View("Receita", pedidos); // Views/Pedidos/Receitas.cshtml
        }


        // POST: Pedidos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Descricao,Valor,DataPedido,UsuarioId")] Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pedido);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Id", pedido.UsuarioId);
            return View(pedido);
        }

        // GET: Pedidos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
            {
                return NotFound();
            }
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Id", pedido.UsuarioId);
            return View(pedido);
        }

        // POST: Pedidos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descricao,Valor,DataPedido,UsuarioId")] Pedido pedido)
        {
            if (id != pedido.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pedido);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PedidoExists(pedido.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Id", pedido.UsuarioId);
            return View(pedido);
        }

        // GET: Pedidos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

        // POST: Pedidos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido != null)
            {
                _context.Pedidos.Remove(pedido);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PedidoExists(int id)
        {
            return _context.Pedidos.Any(e => e.Id == id);
        }

        [HttpPost]
        public async Task<IActionResult> CriarPedido(int servicoId, int convidados)
        {
            var usuario = await _userManager.GetUserAsync(User);
            if (usuario == null)
                return Unauthorized();

            var servico = await _context.Servicos.FindAsync(servicoId);
            if (servico == null)
                return NotFound();

            decimal valorFixos = 200 + 350;
            decimal valorPorPessoa = servico.Preco * convidados;
            decimal valorFinal = valorFixos + valorPorPessoa;

            var pedido = new Pedido
            {
                Descricao = $"Pedido número , cliente {usuario.Nome}, solicitou o serviço  {servico.Nome} no valor de {valorFinal.ToString("C")}",
                Valor = valorFinal,
                DataPedido = DateTime.Now,
                UsuarioId = usuario.Id
            };

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            // Atualiza novamente a descrição só que com o ID real
            pedido.Descricao = $"Pedido número {pedido.Id}, cliente {usuario.Nome}, solicitou o serviço {servico.Nome} no valor de {valorFinal.ToString("C")}";
            _context.Pedidos.Update(pedido);
            await _context.SaveChangesAsync();

            return Json(new { sucesso = true, pedidoId = pedido.Id });
        }

    }

}
