// NewsApiService.cs
using AiStockSentiment.Data.Contracts;
using AiStockSentiment.Data.Models;
using AiStockSentiment.Data.Services;   // YahooPriceService, NewsApiService, SentimentClient
using AiStockSentiment.Data.Workers;    // MarketPoller
using AiStockSentiment.Realtime;        // MarketHub

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AiStockSentiment.Data.Services;
public sealed class NewsApiService(HttpClient http, IConfiguration cfg) : INewsService
{
    private readonly string? _apiKey = cfg["NewsApi:Key"];
    public async Task<IReadOnlyList<NewsItem>> GetHeadlinesAsync(string ticker, int max = 20, CancellationToken ct = default)
    {
        // TODO: call NewsAPI.org, Finnhub, etc., map to NewsItem
        await Task.Delay(50, ct);
        return new List<NewsItem> {
            new("demo","Example headline about " + ticker,"https://example.com", DateTime.UtcNow)
        };
    }
}
