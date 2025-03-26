using System.ComponentModel.DataAnnotations;

namespace ONGManager.Data.DTOs;
public class UsuarioDTO
{
    public int Id { get; set; }
    public int Nivel { get; set; }
    public string Usuario { get; set; } = string.Empty;
}
