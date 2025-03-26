using Microsoft.EntityFrameworkCore;
using ONGManager.Models;

namespace ONGManager.Data
{
    public class OngDbContext(DbContextOptions<OngDbContext> options) : DbContext(options)
    {
        public DbSet<Usuarios> usuario { get; set; }
        public DbSet<CadastroAnimal> cadastro_animal { get; set; }
        public DbSet<Porte> porte { get; set; }
        public DbSet<TipoAnimal> tipo_animal { get; set; }
        public DbSet<Imagem> imagem { get; set; }
        public DbSet<NiveisAcesso> niveis_acesso { get; set; }
        public DbSet<Rotina> rotina { get; set; }
    }
}
