using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ONGManager.Data;
using ONGManager.Data.DTOs;
using ONGManager.Models;

namespace ONGManager.Controllers
{
    public class CadastroAnimaisController(OngDbContext ongDbContext) : Controller
    {
        private readonly OngDbContext _ongDbContext = ongDbContext;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var animais = await _ongDbContext.cadastro_animal.ToListAsync();
            return View(animais);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var animal = await _ongDbContext.cadastro_animal.FindAsync(id);
            
            if(animal != null)
                return View(animal);
            
            return NotFound();
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.TipoAnimal = new SelectList(_ongDbContext.tipo_animal, "id", "animal");
            ViewBag.Porte = new SelectList(_ongDbContext.porte, "id", "porte");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CadastroAnimal cadastroAnimal)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _ongDbContext.AddAsync(cadastroAnimal);
                    await _ongDbContext.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("Ocorreu um erro ao tentar cadastrar o animal: " + ex.Message);
                }
            }

            ViewBag.TipoAnimal = new SelectList(_ongDbContext.tipo_animal, "id", "animal", cadastroAnimal.tipo_animal);
            ViewBag.Porte = new SelectList(_ongDbContext.porte, "id", "porte", cadastroAnimal.porte_animal);

            return View(cadastroAnimal);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var animal = await _ongDbContext.cadastro_animal
                        .Include(c => c.TipoAnimal)
                        .Include(c => c.Porte)
                        .FirstOrDefaultAsync(a => a.id == id);
            
            if(animal != null)
            {
                ViewBag.TipoAnimal = new SelectList(_ongDbContext.tipo_animal, "id", "animal");
                ViewBag.Porte = new SelectList(_ongDbContext.porte, "id", "porte");

                return View(animal);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CadastroAnimal cadastroAnimal)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    _ongDbContext.Update(cadastroAnimal);
                    await _ongDbContext.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch(Exception ex)
                {
                    System.Console.WriteLine("Ocorreu o seguinte erro ao tentar atualizar as informções: " + ex.Message.ToString());
                }

                ViewBag.TipoAnimal = new SelectList(_ongDbContext.tipo_animal, "id", "animal", cadastroAnimal.tipo_animal);
                ViewBag.Porte = new SelectList(_ongDbContext.porte, "id", "porte", cadastroAnimal.porte_animal);
            }

            return View(cadastroAnimal);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var animal = await _ongDbContext.cadastro_animal.FindAsync(id);

            if(animal != null)
            {
                return View(animal);
            }   

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var animal = await _ongDbContext.cadastro_animal.FindAsync(id);

            if(animal != null)
            {
                try
                {
                    _ongDbContext.cadastro_animal.Remove(animal);
                    await _ongDbContext.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch(Exception ex)
                {
                    System.Console.WriteLine("Ocorreu o seguinte erro ao tentar excluir o cadastro do animal: " + ex.Message.ToString());
                }
            }   

            return NotFound();
        }
    }
}