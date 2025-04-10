using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ONGManager.Data;
using ONGManager.Data.DTOs;
using ONGManager.Models;
using Supabase;
using System.Text.Json;

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

    public async Task<string> UploadImagem(int animalId, IFormFile arquivo)
    {
        try
        {
            var animalExiste = await _context.cadastro_animal.AnyAsync(a => a.id == animalId);
            if (!animalExiste)
            {
                throw new ArgumentException($"Animal com ID {animalId} não encontrado");
            }

            var fileName = $"animal_{animalId}_{Guid.NewGuid()}{Path.GetExtension(arquivo.FileName)}";

            using var memoryStream = new MemoryStream();
            await arquivo.CopyToAsync(memoryStream);
            byte[] fileBytes = memoryStream.ToArray();


            var response = await _supabase.Storage
                .From(BucketName)
                .Upload(fileBytes, fileName);

            //Console.WriteLine($"Resposta do Supabase: {JsonSerializer.Serialize(response)}");

            var publicUrl = _supabase.Storage
                .From(BucketName)
                .GetPublicUrl(fileName);

            //Console.WriteLine($"URL pública gerada: {publicUrl}");

            var imagem = new Imagem
            {
                AnimalId = animalId,
                imagem = publicUrl,
                Animal = await _context.cadastro_animal.FindAsync(animalId) 
            };

            await _context.imagem.AddAsync(imagem);
            await _context.SaveChangesAsync();

            return publicUrl;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERRO NO UPLOAD: {ex.ToString()}");
            throw;
        }
    }
}