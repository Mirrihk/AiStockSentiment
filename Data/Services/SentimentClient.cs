using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using AiStockSentiment.Data.Contracts;
using AiStockSentiment.Data.Models;

namespace AiStockSentiment.Data.Services;

public sealed class SentimentClient : ISentimentClient
{
    private readonly HttpClient _http;
    public SentimentClient(HttpClient http) => _http = http;

    // DTOs that mirror the Python API response
    private sealed class SentimentDto
    {
        public string Source { get; set; } = "";
        public double Score { get; set; }
        public double Ema5m { get; set; }
        public List<PointDto> Series { get; set; } = new();
    }

    private sealed class PointDto
    {
        public DateTime T { get; set; }
        public double V { get; set; }
    }

    public async Task<SentimentAggregate> AnalyzeAsync(
        string source,
        string queryOrTicker,
        TimeSpan lookback,
        int maxPosts,
        CancellationToken ct = default)
    {
        var payload = new
        {
            source,
            query = queryOrTicker,
            lookback_minutes = (int)lookback.TotalMinutes,
            max_posts = maxPosts
        };

        var resp = await _http.PostAsJsonAsync("sentiment/analyze", payload, ct);
        resp.EnsureSuccessStatusCode();

        var dto = await resp.Content.ReadFromJsonAsync<SentimentDto>(cancellationToken: ct)
                  ?? throw new InvalidOperationException("Empty sentiment response");

        var series = dto.Series.Select(p => new SentimentPoint(p.T, p.V)).ToList();

        // Map DTO -> your domain model
        return new SentimentAggregate(dto.Source, dto.Score, dto.Ema5m, series);
    }
}
