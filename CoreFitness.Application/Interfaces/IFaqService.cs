using CoreFitness.Application.DTOs.FaqItem;

namespace CoreFitness.Application.Interfaces;

public interface IFaqService
{
    Task<IEnumerable<FaqItemDTO>> GetFaqItemsAsync();
}