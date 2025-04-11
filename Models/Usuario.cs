using System.ComponentModel.DataAnnotations;

namespace ONGManager.Models;

public class Usuarios
{
    [Key]
    [Required]
    public int id { get; set; }

    [Required]
    public int nivel { get; set; }

    [Required]
    [StringLength(200)]
    public string senha { get; set; } = "senha padrão";


    [Required]
    [StringLength(200)]
    public string usuario { get; set; } = "nome padrão";

    [Required]
    [StringLength(200)]
    public string email { get; set; } = "email padrão";





}
