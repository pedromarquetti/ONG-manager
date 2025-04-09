using System.ComponentModel.DataAnnotations;

namespace ONGManager.Models;

public class ONG
{
    public int id { get; set; }
    public string nome_ong { get; set; } = string.Empty;
    public string cidade { get; set; } = string.Empty;
    public string estado { get; set; } = string.Empty;
    public string telefone { get; set; } = string.Empty;
    public string email { get; set; } = string.Empty;
    public string caminho_imagem_logo { get; set;} = string.Empty;
    public string facebook { get; set; } = string.Empty;
    public string instagram { get; set; } = string.Empty;
}