using System.ComponentModel.DataAnnotations;

namespace ONGManager.Models;

public class NiveisAcesso
{
    [Key]
    [Required]
    public int Id { get; set; }
    [Required]
    public string Nivel { get; set; } = string.Empty;
    public ICollection<Rotina> Rotinas { get; set; } = [];
}