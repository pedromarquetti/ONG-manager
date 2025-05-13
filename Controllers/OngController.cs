using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ONGManager.Data;
using ONGManager.Models;

namespace ONGManager.Controllers
{
    public class OngController(OngDbContext ongDbContext) : Controller
    {
        private readonly OngDbContext _ongDbContext = ongDbContext;

        [HttpGet]
        [Authorize(Policy = "Administrador")]
        public async Task<IActionResult> Edit()
        {
            var ong = await _ongDbContext.ong.FirstOrDefaultAsync();
            
            if(ong != null)
            {
                return View(ong);
            }

            return NotFound();
        }

        [HttpPost]
        [Authorize(Policy = "Administrador")]
        public async Task<IActionResult> Edit(ONG ong)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    _ongDbContext.Update(ong);
                    await _ongDbContext.SaveChangesAsync();
                    return RedirectToAction("Index", "CadastroAnimais");
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("Ocorreu o seguinte erro ao tentar atualizar as informções: " + ex.Message.ToString());
                }
            }

            return View(ong);
        }
    }
}