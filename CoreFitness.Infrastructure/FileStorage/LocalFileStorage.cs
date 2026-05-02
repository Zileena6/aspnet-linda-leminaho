using CoreFitness.Application.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace CoreFitness.Infrastructure.FileStorage;

public class LocalFileStorage(IWebHostEnvironment env) : IFileStorage
{
    public async Task<string> SaveAsync(Stream fileStream, string fileName, CancellationToken ct = default)
    {
        var uploadsPath = Path.Combine(env.WebRootPath, "images", "uploads");

        if (!Directory.Exists(uploadsPath))
            Directory.CreateDirectory(uploadsPath);

        var fullpath = Path.Combine(uploadsPath, fileName);

        using var file = File.Create(fullpath);

        await fileStream.CopyToAsync(file, ct);

        return $"/images/uploads/{fileName}";
    }
}