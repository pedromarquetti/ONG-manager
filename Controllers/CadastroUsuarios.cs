using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ONGManager.Data;
using ONGManager.Models;

namespace ONGManager.Controllers
{
    public class CadastroUsuariosController(OngDbContext ongDbContext) : Controller
    {
        private readonly OngDbContext _ongDbContext = ongDbContext;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var usuario = await _ongDbContext.usuario.ToListAsync();
            return View(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Usuarios usr)
        {
            if (ModelState.IsValid) // check if input form is valid
            {
                try
                {
                    // try adding data to db
                    await _ongDbContext.AddAsync(usr);
                    await _ongDbContext.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("Ocorreu um erro ao tentar cadastrar o animal: " + ex.Message);
                }
            }

            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
    }
}
