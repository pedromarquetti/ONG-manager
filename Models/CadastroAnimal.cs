using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ONGManager.Models;

public class CadastroAnimal
{
    [Key]
    [Required]
    public int id { get; set; }
    [Required]
    [StringLength(200)]
    public string nome { get; set; } = string.Empty;
    [Required]
    [StringLength(200)]
    public string raca { get; set; } = string.Empty;
    [Required]
    public int idade { get; set; }
    [Required]
    public bool disponivel { get; set; }
    [Required]

    public int tipo_animal { get; set; }
    [ForeignKey("tipo_animal")]
    public TipoAnimal? TipoAnimal { get; set; }
    [Required]

    public int porte_animal { get; set; }
    [ForeignKey("porte_animal")]
    public Porte? Porte { get; set; }

    public ICollection<Imagem> Imagens { get; set; } = [];
}
