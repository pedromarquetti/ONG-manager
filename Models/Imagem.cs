using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ONGManager.Models;

public class Imagem
{
    [Key]
    public int id { get; set; }

    [Column("animal_id")] 
    public int AnimalId { get; set; } 

    [ForeignKey("AnimalId")]
    public CadastroAnimal? Animal { get; set; } 

    public string imagem { get; set; } = string.Empty;
}