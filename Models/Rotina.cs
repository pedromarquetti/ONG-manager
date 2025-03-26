using System.ComponentModel.DataAnnotations;

namespace ONGManager.Models;

public class Rotina
{
    [Key]
    [Required]
    public int Id { get; set; }
    [Required]
    public string NomeRotina { get; set; } = string.Empty;
    [Required]
    public bool Ativo { get; set; }
    [Required]
    public int NivelAcessoId { get; set; }
    public NiveisAcesso? NivelAcesso { get; set; }
}