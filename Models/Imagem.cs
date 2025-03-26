using System.ComponentModel.DataAnnotations;

namespace ONGManager.Models;

public class Imagem
{
    [Key]
    [Required]
    public int Id { get; set; }
    [Required]
    public int AnimalId { get; set; }
    [Required]
    public string CaminhoImagem { get; set; } = string.Empty;

    [Required]
    public CadastroAnimal? Animal { get; set; }
}