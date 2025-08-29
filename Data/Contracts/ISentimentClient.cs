using System;
using System.Threading;
using AiStockSentiment.Data.Models;

namespace AiStockSentiment.Data.Contracts;

public interface ISentimentClient
{
    Task<SentimentAggregate> AnalyzeAsync(
        string source,
        string queryOrTicker,
        TimeSpan lookback,
        int maxPosts,
        CancellationToken ct = default);
}
