using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ONGManager.Data;
using ONGManager.Models;
using X.PagedList.Extensions;

namespace ONGManager.Controllers
{
    public class CadastroAnimaisController(OngDbContext ongDbContext) : Controller
    {
        private readonly OngDbContext _ongDbContext = ongDbContext;

        [HttpGet]
        public IActionResult Index(int tipoAnimal, int porteAnimal, string raca, string estado, string cidade, int idade, int? pagina)
        {
            int pageSize = 12;
            int pageNumber = pagina ?? 1;

            var query = _ongDbContext.cadastro_animal.AsQueryable();
            query = query.Include(a => a.Imagens);

            var porteAnimalMap = new Dictionary<int, string>
            {
                { 1, "Pequeno" },
                { 2, "Médio" },
                { 3, "Grande" }
            };

            ViewBag.PorteAnimalMap = porteAnimalMap;


            if (tipoAnimal > 0)
            {
                query = query.Where(a => a.tipo_animal == tipoAnimal);
            }

            if(porteAnimal > 0)
            {
                query = query.Where(a => a.porte_animal == porteAnimal);
            }

            if (!raca.IsNullOrEmpty())
            {
                query = query.Where(a => a.raca == raca);
            }

            if (!estado.IsNullOrEmpty())
            {
                query = query.Where(a => a.estado == estado);
            }

            if (!cidade.IsNullOrEmpty())
            {
                query = query.Where(a => a.cidade == cidade);
            }

            if(idade > 0)
            {
                query = query.Where(a => a.idade == idade);
            }

            ViewBag.Racas = _ongDbContext.cadastro_animal.Select(a => a.raca).Distinct().ToList();
            ViewBag.PorteAnimal = _ongDbContext.cadastro_animal.Select(a => a.porte_animal).Distinct().ToList();
            ViewBag.Estados = _ongDbContext.cadastro_animal.Select(a => a.estado).Distinct().ToList();
            ViewBag.Cidades = _ongDbContext.cadastro_animal.Select(a => a.cidade).Distinct().ToList();
            ViewBag.Idades = _ongDbContext.cadastro_animal.Select(a => a.idade).Distinct().ToList();

            var animais = query
                .OrderBy(a => a.id)
                .ToPagedList(pageNumber, pageSize);

           return View(animais);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var animal = await _ongDbContext.cadastro_animal
                .Include(a => a.Imagens)
                .FirstOrDefaultAsync(a => a.id == id);

            if (animal != null)
                return View(animal);

            return NotFound();
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            ViewBag.TipoAnimal = new SelectList(_ongDbContext.tipo_animal, "id", "animal");
            ViewBag.Porte = new SelectList(_ongDbContext.porte, "id", "porte");
            return View();
        }

        [HttpPost]
        [Authorize]
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
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var animal = await _ongDbContext.cadastro_animal
                .Include(c => c.TipoAnimal)
                .Include(c => c.Porte)
                .Include(a => a.Imagens) // Adicionado
                .FirstOrDefaultAsync(a => a.id == id);

            if (animal != null)
            {
                ViewBag.TipoAnimal = new SelectList(_ongDbContext.tipo_animal, "id", "animal");
                ViewBag.Porte = new SelectList(_ongDbContext.porte, "id", "porte");

                return View(animal);
            }

            return NotFound();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(CadastroAnimal cadastroAnimal)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _ongDbContext.Update(cadastroAnimal);
                    await _ongDbContext.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("Ocorreu o seguinte erro ao tentar atualizar as informções: " + ex.Message.ToString());
                }

                ViewBag.TipoAnimal = new SelectList(_ongDbContext.tipo_animal, "id", "animal", cadastroAnimal.tipo_animal);
                ViewBag.Porte = new SelectList(_ongDbContext.porte, "id", "porte", cadastroAnimal.porte_animal);
            }

            return View(cadastroAnimal);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var animal = await _ongDbContext.cadastro_animal.FindAsync(id);

            if (animal != null)
            {
                return View(animal);
            }

            return NotFound();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var animal = await _ongDbContext.cadastro_animal.FindAsync(id);

            if (animal != null)
            {
                try
                {
                    _ongDbContext.cadastro_animal.Remove(animal);
                    await _ongDbContext.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("Ocorreu o seguinte erro ao tentar excluir o cadastro do animal: " + ex.Message.ToString());
                }
            }

            return NotFound();
        }

    }
}