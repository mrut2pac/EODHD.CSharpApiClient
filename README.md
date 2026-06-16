# EODHD.CSharpApiClient

[![NuGet](https://img.shields.io/nuget/v/EODHD.CSharpApiClient.svg)](https://www.nuget.org/packages/EODHD.CSharpApiClient)
[![EODHD](https://img.shields.io/badge/EODHD-Financial%20APIs-5B6CF0)](https://eodhd.com/financial-apis)

Pure-managed C# client for the [EOD Historical Data (EODHD)](https://eodhd.com/financial-apis) API — full REST coverage plus live (WebSocket) streaming.

- **Async-first** — every endpoint is `Task`-returning with an optional `CancellationToken`; synchronous convenience wrappers are provided alongside.
- **Zero runtime dependencies** — uses `System.Text.Json` (inbox on .NET 8+). No `Newtonsoft.Json`.
- **Testable transport** — `IHttpTransport` / `IWebSocketConnection` seams let you mock the network in unit tests; the API key is supplied at construction, never read from the environment.
- **Debuggable** — deterministic build with a symbol package (`.snupkg`) and Source Link, so you can step straight into the client source.

## Install

```bash
dotnet add package EODHD.CSharpApiClient
```

## Quick start (REST)

```csharp
using EODHD.CSharpApiClient;
using EODHD.CSharpApiClient.DataModel;

using EodhdClient client = new EodhdClient("YOUR_API_TOKEN");

UserInfo user = await client.GetUserInfoAsync();
Console.WriteLine($"{user.Name}: {user.ApiRequests}/{user.DailyRateLimit} requests today");

List<HistoricalStockPrice> bars = await client.GetEndOfDayHistoricalStockPricesAsync(
    "AAPL.US", new DateTime(2024, 1, 1), new DateTime(2024, 3, 31), EndOfDayPeriod.Daily);
```

A demo token (`demo`) works against a handful of US symbols (e.g. `AAPL.US`, `MCD.US`) for trying the client without a paid key.

## Live streaming (WebSocket)

Messages are delivered as an `IAsyncEnumerable<T>`; the connection auto-reconnects and replays subscriptions.

```csharp
using EODHD.CSharpApiClient.Live;
using EODHD.CSharpApiClient.DataModel.Live;

using EodhdWebSocketClient<ForexQuote> live = EodhdWebSocketClient.Forex("YOUR_API_TOKEN");

await foreach (ForexQuote quote in live.ReadMessagesAsync(new[] { "EURUSD", "GBPUSD" }, cancellationToken))
{
    Console.WriteLine($"{quote.Symbol}: {quote.Bid} / {quote.Ask} @ {quote.TimestampUtc:O}");
}
```

Factories: `EodhdWebSocketClient.Forex`, `.Crypto`, `.UsTrades`, `.UsQuotes` (≤ 50 symbols each).

## Coverage

| Area | Endpoints |
|------|-----------|
| Prices | end-of-day, intraday, live/delayed quotes, US delayed quotes, tick data |
| Dividends & splits | historical & bulk, upcoming dividends |
| Fundamentals | company (single/bulk/extended), bond fundamentals, insider transactions, earnings trends |
| Reference | exchanges & symbols, search, ID mapping, ticker logos |
| Calendar | upcoming earnings, splits, IPOs |
| News & sentiment | financial news, news & tweet sentiment, news word weights, economic events |
| Analytics | technical indicators (~20 functions), stock screener, macro indicators, historical market cap |
| Options | chain (legacy) + marketplace EOD & contracts + underlying symbols |
| Indices & rates | CBOE indices, US Treasury rates (yield, real-yield, bill, long-term) |
| Live streaming | forex, crypto, US trades, US quotes (WebSocket) |

Endpoints that require a specific EODHD subscription or marketplace add-on surface a typed
`EodhdHttpException` (`PaymentRequired` / `Forbidden`) when your plan doesn't include them.

Unix-epoch timestamp fields carry their unit (`…Ms` / `…Sec`) and expose a `…Utc` companion
(`DateTimeOffset?`) so you get a correct instant without unit guesswork.

## API key

Pass your token to the constructor — the client never reads it from the environment or a config file:

```csharp
EodhdClient client = new EodhdClient("YOUR_API_TOKEN");
```

For finer control (timeout, user agent, base URI, client-side rate limiting) use `EodhdClientOptions`:

```csharp
EodhdClient client = new EodhdClient(new EodhdClientOptions
{
    ApiToken = "YOUR_API_TOKEN",
    Timeout = TimeSpan.FromSeconds(60),
    MaxRequestsPerMinute = 1000,
});
```

## Error handling

Non-success responses throw `EodhdHttpException`, which carries the HTTP `StatusCode`, the raw `ResponseBody`, and a strongly-typed `Code` (`EodhdErrorCase`) so you can branch without substring-matching:

```csharp
try
{
    await client.GetUserInfoAsync();
}
catch (EodhdHttpException ex) when (ex.Code == EodhdErrorCase.Unauthorized)
{
    // invalid or expired API token
}
```

## License

[MIT](LICENSE). This is an independent, community-maintained client and is not affiliated with or endorsed by EOD Historical Data.
