using System.ComponentModel.DataAnnotations;

namespace ONGManager.Data.DTOs;
public class TipoAnimalDTO
{
    public int Id { get; set; }
    public string Animal { get; set; } = string.Empty;
    public int PorteId { get; set; }
    public string? Porte { get; set; }
}