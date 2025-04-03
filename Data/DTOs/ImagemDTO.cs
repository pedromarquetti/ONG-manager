using System.ComponentModel.DataAnnotations;

namespace ONGManager.Data.DTOs;
public class ImagemDTO
{
    public int Id { get; set; }
    public int AnimalId { get; set; }
    public string CaminhoImagem { get; set; } = string.Empty;
}
