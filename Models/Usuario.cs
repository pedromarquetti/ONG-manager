using System.ComponentModel.DataAnnotations;

namespace ONGManager.Models;

public class Usuarios
{
    [Key]
    [Required]
    public int Id { get; set; }
    [Required]
    [StringLength(200)]
    public string Usuario { get; set; } = "nome padrão";
    [Required]
    [StringLength(200)]
    public string Senha { get; set; } = "senha padrão";
    [Required]
    public int Nivel { get; set; }
}
