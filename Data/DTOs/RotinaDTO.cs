using System.ComponentModel.DataAnnotations;

namespace ONGManager.Data.DTOs;
public class RotinaDTO
{
    public int Id { get; set; }
    public string NomeRotina { get; set; } = string.Empty;
    public int NivelAcessoId { get; set; }
    public string? NivelAcesso { get; set; }
    public bool Ativo { get; set; }
}