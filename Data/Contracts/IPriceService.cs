using System;
using System.Threading;
using AiStockSentiment.Data.Models;

namespace AiStockSentiment.Data.Contracts;

public interface IPriceService
{
    Task<TickerQuote> GetQuoteAsync(string ticker, CancellationToken ct = default);
}
