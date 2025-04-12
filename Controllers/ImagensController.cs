using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ONGManager.Data;
using ONGManager.Models;
using ONGManager.Services;

namespace ONGManager.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImagensController : Controller
{
    private readonly ImagemService _imagemService;
    private readonly OngDbContext _context;


    public ImagensController(ImagemService imagemService, OngDbContext context)
    {
        _imagemService = imagemService;
        _context = context;
    }

    [HttpPost("upload/{animalId}")]
    public async Task<IActionResult> UploadImagem(int animalId, IFormFile arquivo)
    {
        try
        {
            var resultado = await _imagemService.UploadImagem(animalId, arquivo);
            return Ok(new
            {
                success = true,
                url = resultado,
                message = "Upload realizado com sucesso"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                success = false,
                message = ex.Message
            });
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        [Bind("id,nome,raca,idade,cidade,estado,disponivel,tipo_animal,porte_animal")] CadastroAnimal cadastroAnimal,
        List<IFormFile> arquivosUpload)
    {
        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(cadastroAnimal);

                if (arquivosUpload != null && arquivosUpload.Any(f => f.Length > 0))
                {
                    foreach (var arquivo in arquivosUpload.Where(f => f.Length > 0))
                    {
                        await _imagemService.UploadImagem(cadastroAnimal.id, arquivo);
                    }
                }

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Dados atualizados com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Erro ao salvar: {ex.Message}");
            }
        }

        ViewBag.TipoAnimal = new SelectList(_context.tipo_animal, "id", "animal");
        ViewBag.Porte = new SelectList(_context.porte, "id", "porte");
        return View(cadastroAnimal);
    }
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var imagens = await _context.imagem
            .Include(i => i.Animal)
            .ToListAsync();
        return View(imagens);
    }
    private bool CadastroAnimalExists(int id)
    {
        return _context.cadastro_animal.Any(e => e.id == id);
    }

    [HttpGet("animal/{animalId}")]
    public async Task<IActionResult> GetImagemPorAnimal(int animalId)
    {
        try
        {
            var imagem = await _context.imagem
                .Where(i => i.AnimalId == animalId)
                .OrderByDescending(i => i.id) 
                .FirstOrDefaultAsync();

            if (imagem == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Nenhuma imagem encontrada para este animal"
                });
            }

            return Ok(new
            {
                success = true,
                url = imagem.imagem,
                animalId = imagem.AnimalId
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = $"Erro ao buscar imagem: {ex.Message}"
            });
        }
    }
}

