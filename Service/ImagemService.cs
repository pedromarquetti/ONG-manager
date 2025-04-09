using Microsoft.AspNetCore.Http;
using ONGManager.Data;
using ONGManager.Data.DTOs;
using ONGManager.Models;
using Supabase.Storage;

namespace ONGManager.Services;

public class ImagemService
{
    private readonly Client _supabase;
    private readonly OngDbContext _context;
    private const string BucketName = "bucketong";

    public ImagemService(Client supabase, OngDbContext context)
    {
        _supabase = supabase;
        _context = context;
    }

    public async Task<ImagemDTO> UploadImagem(int animalId, IFormFile arquivo)
    {
        // Validação básica
        if (arquivo == null || arquivo.Length == 0)
            throw new ArgumentException("Arquivo inválido");

        if (arquivo.Length > 5 * 1024 * 1024) // 5MB
            throw new ArgumentException("Tamanho máximo excedido (5MB)");

        // Gera nome único para o arquivo
        var extensao = Path.GetExtension(arquivo.FileName).ToLower();
        var fileName = $"animal_{animalId}_{Guid.NewGuid()}{extensao}";

        // Upload para o Supabase Storage
        using var stream = arquivo.OpenReadStream();
        var storageResult = await _supabase.Storage
            .From(BucketName)
            .Upload(stream, fileName, new FileOptions
            {
                CacheControl = "public, max-age=31536000",
                Upsert = false
            });

        // Obtém URL pública
        var publicUrl = _supabase.Storage
            .From(BucketName)
            .GetPublicUrl(fileName);

        // Salva no banco
        var imagem = new Imagem
        {
            AnimalId = animalId,
            CaminhoImagem = publicUrl
        };

        await _context.Imagem.AddAsync(imagem);
        await _context.SaveChangesAsync();

        return new ImagemDTO
        {
            Id = imagem.Id,
            AnimalId = imagem.AnimalId,
            CaminhoImagem = imagem.CaminhoImagem
        };
    }
}