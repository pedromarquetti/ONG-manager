using System.ComponentModel.DataAnnotations;

namespace ONGManager.Models;
public class TipoAnimal
{
    [Key]
    [Required]
    public int Id { get; set; }
    [Required]
    public string Animal { get; set; } = string.Empty;
    public ICollection<CadastroAnimal> Animais { get; set; } = [];
}
