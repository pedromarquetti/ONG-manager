using System.ComponentModel.DataAnnotations;

namespace ONGManager.Data.DTOs;
public class CadastroAnimalDTO
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Raca { get; set; } = string.Empty;
    public int Idade { get; set; }
    public bool Disponivel { get; set; }
    public int TipoAnimalId { get; set; }
    public string TipoAnimal { get; set; } = string.Empty;

    public int PorteId { get; set; }
    public string Porte { get; set; } = string.Empty;

    public List<ImagemDTO> Imagens { get; set; } = [];
}
