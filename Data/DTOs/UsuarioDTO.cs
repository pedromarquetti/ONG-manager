using System.ComponentModel.DataAnnotations;

namespace ONGManager.Data.DTOs;
public class UsuarioDTO
{
    public int id { get; set; }
    public int nivel { get; set; }
    [Required]
    public required string senha { get; set; }
    [Required]
    public required string usuario { get; set; }
    [Required]
    public required string email { get; set; }


}
