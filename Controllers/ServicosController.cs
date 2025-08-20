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
using DSEB_projeto.Services;

namespace DSEB_projeto.Controllers
{
    [Authorize]
    public class ServicosController : Controller
    {
        private readonly ApplicationDbContext _context;


        public ServicosController(ApplicationDbContext context)
        {
            _context = context;
        }

        private (string base64String, string imageUrl) ProcessUploadedFile(IFormFile ImageFile)
        {
            if (ImageFile == null)
                return (string.Empty, string.Empty);

            // Verificar o tipo MIME do arquivo
            var allowedTypes = new[] { "image/jpeg", "image/png", "image/gif" };
            if (!allowedTypes.Contains(ImageFile.ContentType.ToLower()))
            {
                throw new ArgumentException("O arquivo enviado não é uma imagem válida. Apenas arquivos JPEG, PNG e GIF são permitidos.");
            }

            // Verificar o tamanho do arquivo (máximo 5MB)
            if (ImageFile.Length > 5 * 1024 * 1024)
            {
                throw new ArgumentException("O arquivo é muito grande. O tamanho máximo permitido é 5MB.");
            }

            using (var ms = new MemoryStream())
            {
                ImageFile.CopyTo(ms);
                byte[] imageBytes = ms.ToArray();

                // Verificar se os bytes realmente representam uma imagem usando SkiaSharp
                try
                {
                    using (var stream = new MemoryStream(imageBytes))
                    {
                        using (var codec = SkiaSharp.SKCodec.Create(stream))
                        {
                            if (codec == null)
                            {
                                throw new ArgumentException("O arquivo enviado não é uma imagem válida.");
                            }

                            // Opcional: verificar dimensões máximas
                            if (codec.Info.Width > 4096 || codec.Info.Height > 4096)
                            {
                                throw new ArgumentException("A imagem é muito grande. As dimensões máximas permitidas são 4096x4096 pixels.");
                            }
                        }
                    }
                }
                catch (ArgumentException)
                {
                    throw;
                }
                catch (Exception)
                {
                    throw new ArgumentException("O arquivo enviado não é uma imagem válida.");
                }

                // Gerar nome único para o arquivo
                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "uploads");
                
                // Criar diretório se não existir
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                // Salvar arquivo físico
                string filePath = Path.Combine(uploadPath, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    ImageFile.CopyTo(fileStream);
                }

                // Retornar tanto a string base64 quanto o caminho da imagem
                string base64String = Convert.ToBase64String(imageBytes);
                string imageUrl = $"/images/uploads/{uniqueFileName}";
                return ($"data:{ImageFile.ContentType};base64,{base64String}", imageUrl);
            }
        }

        // GET: Servicos
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var servicos = await _context.Servicos.ToListAsync();
            int totalServicos = _context.Servicos.Count();

            // Se houver menos de 10, gera até completar 10
            if (totalServicos < 10)
            {
                int quantidadeFaltando = 10 - totalServicos;

                var seeder = new ServicosSeeder(_context);
                seeder.GerarProdutos(quantidadeFaltando);
            }
            
            return View(servicos);

        }

        // GET: Servicos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var servico = await _context.Servicos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (servico == null)
            {
                return NotFound();
            }

            return View(servico);
        }

        // GET: Servicos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Servicos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Preco,Descricao")] Servico servico, IFormFile ImageFile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (ImageFile == null)
                    {
                        ModelState.AddModelError("ImageFile", "Por favor, selecione uma imagem.");
                        return View(servico);
                    }

                    var (base64String, imageUrl) = ProcessUploadedFile(ImageFile);
                    servico.ImagemString = base64String;
                    servico.ImagemUrl = imageUrl;
                    _context.Add(servico);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError("ImageFile", ex.Message);
                    return View(servico);
                }
            }
            return View(servico);
        }

        // GET: Servicos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var servico = await _context.Servicos.FindAsync(id);
            if (servico == null)
            {
                return NotFound();
            }
            return View(servico);
        }

        // POST: Servicos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Preco,Descricao")] Servico servico, IFormFile ImageFile)
        {
            if (id != servico.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var servicoExistente = await _context.Servicos.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
                    if (servicoExistente == null)
                    {
                        return NotFound();
                    }

                    try
                    {
                        if (ImageFile != null)
                        {
                            var (base64String, imageUrl) = ProcessUploadedFile(ImageFile);
                            servico.ImagemString = base64String;
                            servico.ImagemUrl = imageUrl;
                        }
                        else
                        {
                            servico.ImagemString = servicoExistente.ImagemString;
                            servico.ImagemUrl = servicoExistente.ImagemUrl;
                        }
                    }
                    catch (ArgumentException ex)
                    {
                        ModelState.AddModelError("ImageFile", ex.Message);
                        return View(servico);
                    }
                    
                    _context.Update(servico);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServicoExists(servico.Id))
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
            return View(servico);
        }

        // GET: Servicos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var servico = await _context.Servicos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (servico == null)
            {
                return NotFound();
            }

            return View(servico);
        }

        // POST: Servicos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var servico = await _context.Servicos.FindAsync(id);
            if (servico != null)
            {
                _context.Servicos.Remove(servico);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServicoExists(int id)
        {
            return _context.Servicos.Any(e => e.Id == id);
        }
    }

}
