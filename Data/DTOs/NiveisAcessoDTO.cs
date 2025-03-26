using System.ComponentModel.DataAnnotations;

namespace ONGManager.Data.DTOs;
public class NiveisAcessoDTO
{
    public int Id { get; set; }
    public string Nivel { get; set; } = string.Empty;
}
