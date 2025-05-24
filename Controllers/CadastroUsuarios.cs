using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public async Task<IActionResult> Create(Usuarios usuarios)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var passwordHasher = new PasswordHasher<object>();
                    string senhaCriptografada = passwordHasher.HashPassword(usuarios.usuario, usuarios.senha);

                    usuarios.senha = senhaCriptografada;

                    await _ongDbContext.usuario.AddAsync(usuarios);
                    await _ongDbContext.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("Ocorreu um erro ao tentar cadastrar o usuário: " + ex.Message);
                }
            }

            ViewBag.NivelDeAcesso = new SelectList(_ongDbContext.niveis_acesso, "id", "nivel");
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Usuarios usuariosModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var usuario = _ongDbContext.usuario
                        .FirstOrDefault(u => u.usuario == usuariosModel.usuario);

                    if (usuario == null)
                        return Unauthorized();

                    var passwordHasher = new PasswordHasher<object>();
                    var result = passwordHasher.VerifyHashedPassword(usuario, usuario.senha, usuariosModel.senha);

                    if (result == PasswordVerificationResult.Failed)
                        return Unauthorized();

                    var claims = new List<Claim>
                    {
                        new(ClaimTypes.Name, usuario.usuario),
                        new("Nivel", usuario.nivel.ToString()),
                        new("UserId", usuario.id.ToString())
                    };

                    var identity = new ClaimsIdentity(claims, "LoginCookie");
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync("LoginCookie", principal);

                    return RedirectToAction("Index", "CadastroAnimais");
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("Ocorreu o seguinte erro ao tentar atualizar as informções: " + ex.Message.ToString());
                }
            }

            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.NivelDeAcesso = new SelectList(_ongDbContext.niveis_acesso, "id", "nivel");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("LoginCookie");
            return RedirectToAction("Login", "CadastroUsuarios");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id, int nivel)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _ongDbContext.usuario.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            var niveis = await _ongDbContext.niveis_acesso.ToListAsync();


            ViewBag.NiveisAcesso = niveis.Select(n => new SelectListItem
            {
                Value = n.id.ToString(),
                Text = n.nivel,
                Selected = n.id == usuario.nivel
            }).ToList();

            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Usuarios usuarioModel)
        {
            if (id != usuarioModel.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Atualiza diretamente o modelo recebido do front-end
                    _ongDbContext.Update(usuarioModel);
                    await _ongDbContext.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {

                    if (!UsuarioExists(usuarioModel.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        System.Console.WriteLine("", "O registro foi modificado por outro usuário. Por favor, recarregue e tente novamente." + ex.Message.ToString());
                        return View(usuarioModel);
                    }
                }
            }

            return View(usuarioModel);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var usuario = await _ongDbContext.usuario.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _ongDbContext.usuario.FindAsync(id);
            if (usuario != null)
            {
                try
                {
                    _ongDbContext.usuario.Remove(usuario);
                    await _ongDbContext.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("Ocorreu o seguinte erro ao tentar excluir o usuário: " + ex.Message.ToString());
                }
            }

            return View(usuario);
        }

        private bool UsuarioExists(int id)
        {
            return _ongDbContext.usuario.Any(e => e.id == id);
        }
    }
}
