using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ONGManager.Data;
using ONGManager.Data.DTOs;
using ONGManager.Models;

namespace ONGManager.Controllers
{
    public class UsuarioController(OngDbContext ongDbContext) : Controller
    {
        private readonly OngDbContext _ctx = ongDbContext;

        // POST method for handling user creation
        [HttpPost]
        public async Task<IActionResult> Create(Usuarios usr)
        {
            // Check if the incoming values are correct
            if (ModelState.IsValid)
            {
                try
                {

                    // tries running the query
                    await _ctx.AddAsync(usr);
                    await _ctx.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    System.Console.WriteLine("Erro ao tentar cadastrar usuário", e.Message);

                }

            }
            return View();
        }

    }

}

