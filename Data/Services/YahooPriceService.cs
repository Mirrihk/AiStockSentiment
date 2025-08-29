// YahooPriceService.cs
using AiStockSentiment.Data.Contracts;
using AiStockSentiment.Data.Models;

namespace AiStockSentiment.Data.Services;
public sealed class YahooPriceService(HttpClient http) : IPriceService
{
    public async Task<TickerQuote> GetQuoteAsync(string ticker, CancellationToken ct = default)
    {
        // TODO: call a price API or your own proxy.
        // For now, fake it:
        await Task.Delay(50, ct);
        var now = DateTime.UtcNow;
        return new TickerQuote(ticker.ToUpperInvariant(), 123.45m, 0.67m, now);
    }
}
