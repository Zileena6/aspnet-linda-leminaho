using CoreFitness.Application.DTOs.Quotes;

namespace CoreFitness.Application.Interfaces;

public interface IQuoteService
{
    Task<QuoteDTO?> GetRandomQuoteAsync();
}