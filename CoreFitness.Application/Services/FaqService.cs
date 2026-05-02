using CoreFitness.Application.DTOs.FaqItem;
using CoreFitness.Application.Interfaces;
using System.Text.Json;

namespace CoreFitness.Application.Services;

public class FaqService : IFaqService
{
    private readonly string _path = Path.Combine("wwwroot", "data", "faq.json");

    public async Task<IEnumerable<FaqItemDTO>> GetFaqItemsAsync()
    {
        var json = await File.ReadAllTextAsync(_path);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        return JsonSerializer.Deserialize<IEnumerable<FaqItemDTO>>(json, options) ?? [];
    }
}