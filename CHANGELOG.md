# Changelog

All notable changes to this project are documented here. The format is based on
[Keep a Changelog](https://keepachangelog.com/en/1.1.0/), and this project adheres to
[Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added (REST coverage — core market data)
- Intraday historical prices (`GetIntradayHistoricalStockPrices[Async]`) with a
  `1m`/`5m`/`1h` interval and a converter for EODHD's space-separated timestamps.
- Historical dividends (`GetHistoricalDividends[Async]`).
- Live/delayed real-time quotes — single (`GetLivePrice[Async]`) and multi-symbol
  (`GetLivePrices[Async]`), with an `"NA"`-tolerant numeric converter.
- Search (`Search[Async]`), full exchanges list (`GetExchangesList[Async]`), and the
  IPO calendar (`GetUpcomingIpos[Async]`).
- Unit tests for the new converters and endpoints; `SkippableFact` integration tests.

### Added
- Initial repository scaffold: solution, package metadata, MIT license, CI + publish
  workflows (NuGet Trusted Publishing / OIDC), and the testing layout.
- `EodhdClient` async-first HTTP client with an `IHttpTransport` seam,
  `EodhdClientOptions`, and a typed `EodhdHttpException` / `EodhdErrorCase`.
- Ported the existing in-house EODHD client to System.Text.Json with full REST
  coverage for the original endpoint set: User, Exchanges (details, symbol-change
  history), Exchange Symbols, Calendar (upcoming earnings, upcoming splits),
  End-of-Day historical prices, Fundamentals (single + bulk + extended bulk),
  Historical Splits, and the bulk last-day EOD/splits feeds. Each endpoint ships as
  an `async` method with a `CancellationToken` plus a synchronous wrapper.
- 87 data-model classes migrated to `System.Text.Json` (`[JsonPropertyName]`,
  numbers read from JSON strings globally) with XML documentation throughout.
- Optional client-side leaky-bucket rate limiting via
  `EodhdClientOptions.MaxRequestsPerMinute` (`RequestRateLimiter`).
- Unit tests (mock transport): model serialization incl. computed adjusted prices,
  split-factor parsing, the rate limiter, and error paths. Integration tests
  (`SkippableFact`, gated on `EODHD_API_KEY`) covering every ported endpoint.

[Unreleased]: https://github.com/mrut2pac/EODHD.CSharpApiClient/commits/main
