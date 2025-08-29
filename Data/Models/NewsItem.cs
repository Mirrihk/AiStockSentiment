// NewsItem.cs
namespace AiStockSentiment.Data.Models;
public sealed record NewsItem(string Source, string Title, string Url, DateTime PublishedUtc);
