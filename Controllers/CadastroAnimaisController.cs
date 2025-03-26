using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Details(int idAnimal)
        {
            var animal = await _ongDbContext.cadastro_animal.FindAsync(idAnimal);
            
            if(animal != null)
                return View(animal);
            
            return NotFound();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CadastroAnimalDTO cadastroAnimalDTO)
        {
            CadastroAnimal animal = new()
            {
                disponivel = cadastroAnimalDTO.Disponivel,
                idade = cadastroAnimalDTO.Idade,
                nome = cadastroAnimalDTO.Nome,
                porte_animal = cadastroAnimalDTO.PorteId,
                raca = cadastroAnimalDTO.Raca,
                tipo_animal = cadastroAnimalDTO.TipoAnimalId
            };

            //TO DO: upload da imagem

            try
            {
                _ongDbContext.cadastro_animal.Add(animal);
                await _ongDbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                System.Console.WriteLine("Ocorreu o seguinte erro ao tentar cadastrar animal: " + ex.Message.ToString());
            }

            return View(cadastroAnimalDTO);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int idAnimal)
        {
            var animal = await _ongDbContext.cadastro_animal.FindAsync(idAnimal);
            
            if(animal != null)
            {
                CadastroAnimalDTO animalDTO = new()
                {
                    Disponivel = animal.disponivel,
                    Idade = animal.idade,
                    Nome = animal.nome,
                    PorteId = animal.porte_animal,
                    Raca = animal.raca,
                    TipoAnimalId = animal.tipo_animal
                };

                return RedirectToAction(nameof(Index));
            }

            return NotFound();
        }

        [HttpPut]
        public async Task<IActionResult> Edit(int idAnimal, CadastroAnimalDTO cadastroAnimalDTO)
        {
            var animal = await _ongDbContext.cadastro_animal.FindAsync(idAnimal);
            if(animal != null)
            {
                try
                {
                    animal.disponivel = cadastroAnimalDTO.Disponivel;
                    animal.idade = cadastroAnimalDTO.Idade;
                    animal.nome = cadastroAnimalDTO.Nome;
                    animal.porte_animal = cadastroAnimalDTO.PorteId;
                    animal.raca = cadastroAnimalDTO.Raca;
                    animal.tipo_animal = cadastroAnimalDTO.TipoAnimalId;

                    await _ongDbContext.SaveChangesAsync();
                }
                catch(Exception ex)
                {
                    System.Console.WriteLine("Ocorreu o seguinte erro ao tentar atualizar as informções: " + ex.Message.ToString());
                }

                return RedirectToAction(nameof(Index));
            }

            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int idAnimal)
        {
            var animal = await _ongDbContext.cadastro_animal.FindAsync(idAnimal);

            if(animal != null)
            {
                return View(animal);
            }   

            return NotFound();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteConfirmed(int idAnimal)
        {
            var animal = await _ongDbContext.cadastro_animal.FindAsync(idAnimal);

            if(animal != null)
            {
                _ongDbContext.cadastro_animal.Remove(animal);
                return RedirectToAction(nameof(Index));
            }   

            return NotFound();
        }
    }
}