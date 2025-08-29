namespace AiStockSentiment.Data.Models;


public sealed record SentimentAggregate(
    string Source,
    double Score,                   // latest score
    double Ema5m,                   // smoothed score
    IReadOnlyList<SentimentPoint> Series  // <- ensure this exists
);
