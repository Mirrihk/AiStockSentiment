using Microsoft.AspNetCore.SignalR;
using AiStockSentiment.Data.Models;

namespace AiStockSentiment.Realtime;
public sealed class MarketHub : Hub
{
    // Clients receive: QuoteUpdated, SentimentUpdated, NewsUpdated
    // No server methods required yet unless i want subscriptions per ticker.
}
