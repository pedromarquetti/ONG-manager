using System.ComponentModel.DataAnnotations;

namespace ONGManager.Models;
public class TipoAnimal
{
    [Key]
    [Required]
    public int id { get; set; }
    [Required]
    public string animal { get; set; } = string.Empty;
    public ICollection<CadastroAnimal> Animais { get; set; } = [];
}
