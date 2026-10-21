# Changelog

All notable changes to this project are documented here. The format is based on
[Keep a Changelog](https://keepachangelog.com/en/1.1.0/), and this project adheres to
[Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [2.0.0] - 2026-10-21

### Changed (breaking)
- Date and time fields that were exposed as raw strings are now typed.
  - The date and time properties listed here are plain settable properties, as are the previously get-only ones
    described below. Each is read and written by `JsonSupport.NaTolerantDateConverter` (`DateTime?`) or
    `JsonSupport.NaTolerantTimeConverter` (`TimeOnly?`), so a blank, `"NA"` or `"0000-00-00"` gives `null`.
  - Serializing a model now writes the normalized value (`yyyy-MM-dd[ HH:mm:ss]`, `HH:mm:ss`) instead of the raw wire
    text.
  - This replaces the private raw string plus get-only parsed property behind `General.IPODate`, `Earning.Date` and others,
    so those are now settable too, e.g. in test fakes. Their JSON keys are unchanged.
  - `DateTime?`:
    - bulk `BalanceSheetData` / `CashFlowData` / `IncomeStatementData` `.Date` / `.FilingDate`, `BulkEarnings.Date`
      and bulk `Highlights.MostRecentQuarter`
    - `ETFData.InceptionDate` / `DateOngoingCharge`, and `HistoricalComponent.StartDate` / `EndDate`
    - `CboeIndex.Date` / `EffectiveDate` / `ReviewDate`
    - `ScreenerResult.LastDayDataDate`, `SearchResult.PreviousCloseDate` and `UsDelayedQuote.PreviousCloseDate`
    - `BondFundamentals.UpdateDate` / `LastTradeDate` / `MaturityDate` / `NextCallDate`, and `BondIssueData.IssueDate` /
      `OfferingDate` / `FirstCouponDate` / `FirstTradingDay`
  - `TimeOnly?`: `TradingHours.Open` / `Close` / `OpenUTC` / `CloseUTC`.
  - The options models (`OptionData`, `OptionContract`) keep their raw strings for now. EODHD sends their timestamps in
    inconsistent formats (`bid_date` is UTC with `Z`; `ask_date` has no zone).
- `ETFData.AverageMarketCapitalizationMillions` is now `decimal?` and `MarketCapitalizationETF.Mega/Big/Medium/Small/Micro`
  are now `double?` (were `string`; see Fixed).
- Bulk `BalanceSheetData.СashAndEquivalents` is renamed to `CashAndEquivalents` (see Fixed).
- The mutual-fund row types are corrected to the wire shape. These are `AssetAllocation`, `MarketCapitalization`,
  `SectorWeights`, `TopHoldings` and `WorldRegion`, ported earlier but never reachable.
  - `AssetAllocation` percentages are `double?`.
  - `SectorWeights.Type` is now `Name`.
  - `TopHoldings.Weight` is the API's `string` (e.g. `"4.10%"`).
  - `Category_Average`, `Portfolio_%`, `Amount_%` and `Stocks_%` now bind.
- `GetFundamentalsDataAsync` rejects field-path filters (`"General::Code"`) with `ArgumentException`. They used to
  throw `JsonException` or return an empty result.

### Fixed
- `GetFundamentalsDataAsync` threw for every ETF: `ETF_Data.Average_Mkt_Cap_Mil` and the
  `Market_Capitalisation` buckets arrive as JSON numbers but were modeled as `string`.
  Both are now numeric and still accept a quoted number.
- `GetLivePriceAsync` / `GetLivePricesAsync` threw for a delisted symbol, whose `timestamp` comes back as `"NA"`;
  `LiveStockPrice.TimestampSec` now maps it to `null`, like the price fields.
- Any `string` property now accepts a JSON number or boolean and keeps its wire text, instead of failing the whole
  response, since EODHD switches some fields between quoted and bare values from one symbol to the next.
- Fields that were silently dropped now bind: `ETFData.IndexName` (`Index_Name`), bulk `CashFlowData.FilingDate`
  (`filing_date`), and bulk `BalanceSheetData.CashAndEquivalents` (renamed from `СashAndEquivalents`, whose
  first letter was a Cyrillic `С`).
- `GetFundamentalsDataAsync` with `filters` now returns the requested sections. A single section used to come back
  empty, because the API returns it unwrapped. A section the symbol lacks (sent as `"NA"`) used to throw and is now
  left `null`.

### Added
- Fields the API sends that were not modeled: `SectorWeightETF.RealEstate` / `CommunicationServices`; fundamentals
  `General.OperatingMIC` / `ExchangeMarket` / `DelistedDate` / `MarketCap` (indices); `Component.Weight`;
  `CashFlowData.IssuanceOfCapitalStock`; crypto `Statistics.MaxSupply`; bulk `General.OperatingMIC` / `OpenFigi`
  and `BulkEarnings.ReportDate`; `ExchangeDetails.ExchangeEarlyCloseDays`; `UserInfo.SubscriptionMode` /
  `CanManageOrganizations`; `UpcomingEarnings.From` / `To`.
- `FundamentalData.MutualFundData` (`MutualFund_Data`): NAV, net assets, yields, expense ratio, asset allocation,
  value/growth, top holdings, market-cap buckets, sector and region weights, and the new `TopCountry` rows. The
  payload repeats each section under a lower-case key holding `null`; the new
  `JsonSupport.CaseInsensitiveDuplicateKeyConverter<T>` keeps the populated one.
- Crypto `FundamentalData.Tech` (`CryptoTech.Developers`) and `FundamentalData.Resources` (`CryptoResources.Links` /
  `Thumbnail`).
- New date and time fields are typed, using the same converters as above.
  - `DateTime?`: `General.DelistedDate`, `UpcomingEarnings.From` / `To`, `BulkEarnings.ReportDate`, and
    `MutualFundData.UpdateDate` / `InceptionDate` / `ExpenseRatioDate`.
  - `TimeOnly?`: `ExchangeEarlyCloseDay.EarlyClose`, the exchange's local closing time.
- `JsonSupport.NumberTolerantStringConverter` (registered globally for REST and WebSocket),
  `JsonSupport.NaTolerantInt64Converter`, `JsonSupport.NaTolerantDateConverter` and `JsonSupport.NaTolerantTimeConverter`.

## [1.0.2] - 2026-09-28

### Fixed
- `RequestRateLimiter.Dispose` (and so `EodhdClient.Dispose`) no longer blocks for up to a minute: it used to wait out the
  refill loop's pending delay - the rest of the one-minute warm-up after the first request, or one refill interval after that -
  and now cancels it instead.
- `RequestRateLimiter` refilled permits from `Stopwatch.ElapsedTicks` (Stopwatch ticks) as if they were `TimeSpan` ticks, so
  wherever `Stopwatch.Frequency` is not 10 MHz (e.g. Linux) it refilled about 100x too fast and did not enforce the configured rate;
  at 28 requests/minute or fewer the 32-bit tick arithmetic could also overflow. Refill now uses elapsed `TimeSpan`s in 64-bit math.

## [1.0.1] - 2026-06-24

### Fixed
- README LICENSE link now uses an absolute GitHub URL so it resolves on the NuGet package page (relative links parked on the package page).

## [1.0.0] - 2026-06-16

### Changed (explicit epoch units + UTC accessors)
- Renamed every raw Unix-epoch field to carry its unit: millisecond fields are now `…Ms`
  (`TimestampMs`, `BidTimeMs`, `AskTimeMs`, `LastTradeTimeMs`, `EthTimeMs`) and second fields are now
  `…Sec` (`TimestampSec` on intraday bars and the real-time quote). The JSON wire keys are unchanged.
- Added a `…Utc` companion (`DateTimeOffset?`, offset `+00:00`) next to each, computed via a shared
  `UnixTime` helper — so consumers get a correct UTC instant without knowing the unit (e.g.
  `quote.TimestampUtc`). `DateTimeOffset` is used (over `DateTime`) so the value is an unambiguous instant
  that can't be misread as local time.

### Added (live streaming — WebSocket: US trades & quotes)
- US live feeds via `EodhdWebSocketClient.UsTrades(...)` (trade prints) and `EodhdWebSocketClient.UsQuotes(...)`
  (top-of-book), with typed messages `UsTrade` (price, sale-condition codes, volume, dark-pool flag, market
  status) and `UsQuote` (bid/ask price and size). They stream during US regular and extended (pre/post-market)
  hours; an index ETF (SPY, QQQ, …) gives live index exposure. Message shapes captured live at pre-market.
- Unit tests (US trade/quote parsing incl. condition-code arrays); `SkippableFact` integration tests that
  stream live US trades and quotes (skipped when the US market is closed).

### Added (live streaming — WebSocket: forex & crypto)
- `EodhdWebSocketClient<TMessage>` — a streaming client for the EODHD live (WebSocket) feeds, created via
  `EodhdWebSocketClient.Forex(...)` and `EodhdWebSocketClient.Crypto(...)`. Messages are delivered as an
  `IAsyncEnumerable<T>` (`ReadMessagesAsync`); `SubscribeAsync`/`UnsubscribeAsync` adjust the symbol set
  (max 50) on the fly. The connection auto-reconnects with exponential backoff and replays subscriptions
  (configurable via `EodhdWebSocketOptions`). Built on `System.Net.WebSockets` — no new dependency — behind
  an `IWebSocketConnection` seam for testing.
- Typed messages `ForexQuote` (bid/ask) and `CryptoTrade` (price/quantity).
- Unit tests (mock connection: parsing, control-message skipping, subscribe payloads, reconnect/resubscribe,
  validation); `SkippableFact` integration tests streaming live forex and crypto.
- The `us` and `us-quote` feeds are entitled too but only stream during US market hours; they will be added
  once their message shapes can be captured live.

### Added (REST coverage — tick data)
- Historical trade-level tick data for US equities (`GetTicks[Async]`). The API returns the ticks as
  parallel columns (struct-of-arrays); the client transposes them into one `Tick` per trade (exchange,
  market, sub-market, price, sequence, shares, sales condition, and millisecond timestamp). Filterable by
  `from`/`to` (sent as Unix timestamps) and `limit`. Tolerates absent columns (e.g. `ex`).
- Unit tests (transpose, missing-column tolerance, Unix-timestamp parameters); `SkippableFact` integration
  test (verified live).

### Added (REST coverage — ticker logos)
- Ticker logos (`GetLogoUrl[Async]`, `GetLogoBytes[Async]`, `DownloadLogoBytes[Async]`). Because the logo
  path is not predictably derivable from the symbol (its casing varies by exchange), the URL is resolved
  from fundamentals `General.LogoURL`; `GetLogoUrl` returns `null` when a symbol has no logo. The images
  are public PNGs served from the site root, fetched through a new raw-bytes transport path;
  `DownloadLogoBytes` takes a logo URL (absolute or site-relative) so a URL already held on a
  `FundamentalData` record can be fetched without a second fundamentals call.
- Unit tests (sequenced mock transport covering the two-step flow); `SkippableFact` integration tests
  (verified live, including the no-logo case).

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

[Unreleased]: https://github.com/mrut2pac/EODHD.CSharpApiClient/compare/v2.0.0...main
[2.0.0]: https://github.com/mrut2pac/EODHD.CSharpApiClient/compare/v1.0.2...v2.0.0
[1.0.2]: https://github.com/mrut2pac/EODHD.CSharpApiClient/compare/v1.0.1...v1.0.2
[1.0.1]: https://github.com/mrut2pac/EODHD.CSharpApiClient/compare/v1.0.0...v1.0.1
[1.0.0]: https://github.com/mrut2pac/EODHD.CSharpApiClient/tree/v1.0.0
