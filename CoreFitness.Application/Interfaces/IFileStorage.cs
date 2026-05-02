namespace CoreFitness.Application.Interfaces;

public interface IFileStorage
{
    Task<string> SaveAsync(Stream fileStream, string fileName, CancellationToken ct = default);
}