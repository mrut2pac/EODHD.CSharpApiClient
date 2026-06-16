# Changelog

All notable changes to this project are documented here. The format is based on
[Keep a Changelog](https://keepachangelog.com/en/1.1.0/), and this project adheres to
[Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added (REST coverage — Cboe indices, news word weights)
- Cboe index list (`GetCboeIndices[Async]`) — the available index snapshots (index-level fields).
- Cboe single index (`GetCboeIndex[Async]`) — a single index snapshot with its constituent
  components, addressed by index code, feed type, and date (all required by the API). The JSON:API
  `data[].attributes` envelope and the nested `components[].attributes` are flattened onto a
  `CboeIndex` with a `CboeIndexComponent[]`. These endpoints require the Cboe marketplace subscription.
- News word weights (`GetNewsWordWeights[Async]`) — the weighted keywords extracted from recent news
  for a symbol, returned as a word-to-weight dictionary.
- Unit tests for all groups; `SkippableFact` integration tests (verified live; the Cboe endpoints are
  gated on the marketplace entitlement).

### Added (REST coverage — commodities, ID mapping, US delayed quotes, option underlyings)
- Historical commodity prices (`GetCommodityHistoricalPrices[Async]`) by commodity code
  (e.g. `BRENT`, `WTI`, `GOLD`).
- Identifier mapping (`GetIdMapping[Async]`) between symbol, ISIN, FIGI, LEI, CUSIP, and CIK
  (at least one filter required).
- US delayed quotes (`GetUsDelayedQuotes[Async]`) — a wide reference + quote + statistics record per
  symbol, returned keyed by symbol.
- Marketplace option underlying-symbols (`GetOptionUnderlyingSymbols[Async]`) — the compact list of
  symbols that have options (requires the marketplace options subscription).
- Unit tests for all groups; `SkippableFact` integration tests (verified live; option underlyings via
  the public `demo` token).

### Added (REST coverage — upcoming dividends, US Treasury rates)
- Upcoming-dividends calendar (`GetUpcomingDividends[Async]`) — ex-dividend dates, filtered by symbol
  and/or date (the API requires at least one of symbol or an exact date).
- US Treasury rates: yield curve (`GetTreasuryYieldRates[Async]`), real yield curve
  (`GetTreasuryRealYieldRates[Async]`), bills (`GetTreasuryBillRates[Async]`), and long-term
  (`GetTreasuryLongTermRates[Async]`). Filterable by calendar `year`; the API defaults to the current
  year (its documented `from`/`to` parameters do not filter, so the client does not expose them).
- Unit tests for all groups; `SkippableFact` integration tests (verified live).

### Added (REST coverage — options)
- Legacy options chain (`GetOptionsChain[Async]`) — the nested per-expiration structure with call/put
  contracts, pricing, and greeks. Timestamp fields that use a zero sentinel
  (`0000-00-00 00:00:00`) are surfaced as raw strings.
- Marketplace options API (`GetOptionsEod[Async]`, `GetOptionContracts[Async]`) — end-of-day prices and
  contract snapshots with the full 43-field attribute set (OHLC, bid/ask, volume, open interest,
  volatility, and greeks), filtering (type, strike, expiration, trade date), sorting, and pagination.
  The JSON:API `data[].attributes` envelope is unwrapped to an `OptionData[]`. These endpoints require
  the separate marketplace options subscription.
- Unit tests for all three (mapping verified against captured live responses); `SkippableFact`
  integration tests — the legacy chain runs against the configured key, and the marketplace endpoints
  are verified live via EODHD's public `demo` token.

### Added (REST coverage — bonds, insider transactions, earnings trends)
- Insider transactions (`GetInsiderTransactions[Async]`) — SEC Form 4 records, optionally filtered
  by symbol and date range.
- Bond fundamentals (`GetBondFundamentals[Async]`) by ISIN, including the nested
  `ClassificationData` and `IssueData` blocks.
- Earnings trends (`GetEarningsTrends[Async]`) — analyst estimates, EPS trend, and revisions; the
  API's per-symbol nested arrays are flattened into a single `EarningsTrend[]`.
- Unit tests for all three groups; `SkippableFact` integration tests (verified live).

### Added (REST coverage — screener, macro, market cap)
- Stock-market screener (`GetScreener[Async]`) with strongly-typed `ScreenerFilter` conditions
  (serialized to EODHD's `[field, operation, value]` triples), pre-defined `signals`, `sort`,
  `limit`, and `offset`. The `data` envelope is unwrapped to a `ScreenerResult[]`.
- Macroeconomic indicators (`GetMacroIndicator[Async]`) covering all 39 indicators via a
  `MacroIndicator` enum, per country (ISO 3166-1 alpha-3 or World Bank aggregate code).
- Historical market capitalisation (`GetHistoricalMarketCap[Async]`) — the API's index-keyed
  object is projected into a date-ordered `HistoricalMarketCap[]`.
- Unit tests for all three groups; `SkippableFact` integration tests (verified live).

### Packaging
- Ship a symbol package (`.snupkg`) with Source Link (`Microsoft.SourceLink.GitHub`) and a
  deterministic build, so consumers can step into the client source while debugging. Source
  Link is a build-time-only dependency — the package keeps zero runtime dependencies.

### Added (REST coverage — technical indicators)
- Technical indicators (`GetTechnicalIndicator[Async]`) covering all ~20 functions via a
  `TechnicalFunction` enum and an `extraParameters` escape hatch for function-specific
  parameters. Each point exposes its outputs in a `Values` dictionary keyed by the API's
  field names (function-agnostic), so single-valued, multi-valued (MACD, Bollinger,
  stochastic), and OHLC (split-adjusted) shapes all work; `TechnicalIndicatorPointConverter`
  handles deserialization.

### Fixed
- Widened EOD `Volume` (single-symbol and bulk) from `long` to `decimal?` — high-volume
  penny stocks exceed `Int64`, which threw mid-array during bulk deserialization. Surfaced
  by the live integration suite.

### Added (REST coverage — news & events)
- Financial news (`GetNews[Async]`, by symbol and/or tag, with date range, limit, offset)
  including the per-article sentiment breakdown.
- News sentiment (`GetNewsSentiments[Async]`) and social/tweets sentiment
  (`GetTweetsSentiments[Async]`) — daily aggregated series keyed by symbol.
- Economic events calendar (`GetEconomicEvents[Async]`) with country/comparison/type
  filters; event timestamps parsed via the space-separated date converter.
- Unit tests for all three groups; `SkippableFact` integration tests (verified live).

### Added (REST coverage — core market data)
- Intraday historical prices (`GetIntradayHistoricalStockPrices[Async]`) with a
  `1m`/`5m`/`1h` interval and a converter for EODHD's space-separated timestamps.
- Historical dividends (`GetHistoricalDividends[Async]`).
- Live/delayed real-time quotes — single (`GetLivePrice[Async]`) and multi-symbol
  (`GetLivePrices[Async]`), with an `"NA"`-tolerant numeric converter.
- Search (`Search[Async]`), full exchanges list (`GetExchangesList[Async]`), and the
  IPO calendar (`GetUpcomingIpos[Async]`).
- Unit tests for the new converters and endpoints; `SkippableFact` integration tests.

### Fixed
- Widened large financial / share-count fundamentals fields (e.g. `Shares`, `EBITDA`,
  `RevenueTTM`, `SharesOutstanding`, `EnterpriseValue`, crypto supply) from `long` to
  `decimal?` — EODHD returns these with decimals / beyond `Int64` range, which threw
  during deserialization. Surfaced by running the integration suite against live data.

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
