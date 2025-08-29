using System;
using System.Collections.Generic;
using System.Threading;
using AiStockSentiment.Data.Models;

namespace AiStockSentiment.Data.Contracts;

public interface INewsService
{
    Task<IReadOnlyList<NewsItem>> GetHeadlinesAsync(string ticker, int max = 20, CancellationToken ct = default);
}
