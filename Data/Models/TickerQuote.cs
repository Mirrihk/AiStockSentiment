// TickerQuote.cs
namespace AiStockSentiment.Data.Models;
public sealed record TickerQuote(string Ticker, decimal Price, decimal Change, DateTime TimestampUtc);

