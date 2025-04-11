using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ONGManager.Data;

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
    
    public IActionResult Create()
        {
            return View();
        }
    }
}