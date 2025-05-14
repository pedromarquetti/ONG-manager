using System.ComponentModel.DataAnnotations;

namespace ONGManager.Models;

public class NiveisAcesso
{
    [Key]
    [Required]
    public int id { get; set; }
    [Required]
    public string nivel { get; set; } = string.Empty;
    public ICollection<Rotina> Rotinas { get; set; } = [];
}