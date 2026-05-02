using CoreFitness.Application.DTOs.Quotes;
using CoreFitness.Application.Interfaces;
using System.Net.Http.Json;
using Microsoft.Extensions.Caching.Memory;

namespace CoreFitness.Application.Services;

public class QuoteService(HttpClient httpClient, IMemoryCache cache) : IQuoteService
{
    private const string CacheKey = "random_quote";

    public async Task<QuoteDTO?> GetRandomQuoteAsync()
    {
        if (cache.TryGetValue(CacheKey, out QuoteDTO? cached))
            return cached;

        var response = await httpClient.GetFromJsonAsync<ZenQuoteResponse[]>("https://zenquotes.io/api/random");

        var quote = response?.FirstOrDefault();

        if (quote is null) return null;

        var dto = new QuoteDTO
        {
            Content = quote.Q,
            Author = quote.A
        };

        cache.Set(CacheKey, dto, TimeSpan.FromMinutes(1));

        return dto;
    }

    private class ZenQuoteResponse
    {
        public string Q { get; set; } = string.Empty;
        public string A { get; set; } = string.Empty;
    }
}