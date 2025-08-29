using AiStockSentiment.Data.Contracts;  // IPriceService, INewsService, ISentimentClient
using AiStockSentiment.Data.Services;   // YahooPriceService, NewsApiService, SentimentClient
using AiStockSentiment.Data.Workers;    // MarketPoller
using AiStockSentiment.Realtime;        // MarketHub

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Settings-bound clients
builder.Services.AddSignalR();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpClient<IPriceService, YahooPriceService>();
builder.Services.AddHttpClient<INewsService, NewsApiService>();
builder.Services.AddHttpClient<ISentimentClient, SentimentClient>(c => {
    c.BaseAddress = new Uri(builder.Configuration["SentimentApi:BaseUrl"]!); // e.g., http://localhost:8000/
});

builder.Services.AddHostedService<MarketPoller>();

var app = builder.Build();
if (!app.Environment.IsDevelopment()) app.UseExceptionHandler("/Error");
app.UseStaticFiles();
app.UseRouting();

app.MapBlazorHub();
app.MapHub<MarketHub>("/hubs/market");
app.MapFallbackToPage("/_Host"); // (Blazor Server)

app.Run();
