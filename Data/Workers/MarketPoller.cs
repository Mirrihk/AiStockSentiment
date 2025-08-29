using AiStockSentiment.Data.Contracts;
using AiStockSentiment.Data.Models;
using AiStockSentiment.Realtime;
using Microsoft.AspNetCore.SignalR;
using AiStockSentiment.Data.Contracts;  // IPriceService, INewsService, ISentimentClient
using AiStockSentiment.Data.Services;   // YahooPriceService, NewsApiService, SentimentClient
using AiStockSentiment.Data.Workers;    // MarketPoller
using AiStockSentiment.Realtime;        // MarketHub

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AiStockSentiment.Data.Workers;
public sealed class MarketPoller(
    IPriceService prices,
    INewsService news,
    ISentimentClient sentiment,
    IHubContext<MarketHub> hub,
    ILogger<MarketPoller> log) : BackgroundService
{
    private static readonly string[] Tickers = ["AAPL", "MSFT", "NVDA", "TSLA", "BTC-USD"];

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            foreach (var t in Tickers)
            {
                try
                {
                    var q = await prices.GetQuoteAsync(t, stoppingToken);
                    await hub.Clients.All.SendAsync("QuoteUpdated", q, stoppingToken);

                    var sReddit = await sentiment.AnalyzeAsync("reddit", t, TimeSpan.FromMinutes(60), 200, stoppingToken);
                    await hub.Clients.All.SendAsync("SentimentUpdated", t, sReddit, stoppingToken);

                    var n = await news.GetHeadlinesAsync(t, 10, stoppingToken);
                    await hub.Clients.All.SendAsync("NewsUpdated", t, n, stoppingToken);
                }
                catch (Exception ex) { log.LogWarning(ex, "Polling failed for {Ticker}", t); }
            }
            await Task.Delay(TimeSpan.FromSeconds(15), stoppingToken); // adjust cadence
        }
    }
}
