# CLAUDE.md — EODHD.CSharpApiClient

Guidelines for working on this repository with Claude Code or any AI coding assistant.

---

## Repo overview

Pure-managed, **async-first** C# (.NET 8) client for the [EOD Historical Data (EODHD) API](https://eodhd.com/financial-apis)
— full REST coverage plus live (WebSocket) streaming. **Zero runtime dependencies** — only
`System.Text.Json` (inbox on .NET 8+), no Newtonsoft. MIT-licensed, published to NuGet as
`EODHD.CSharpApiClient`.

```
src/
  EODHD.CSharpApiClient/        ← the library (packaged to NuGet)
    EodhdClient.cs              ← core: ctors/options, shared GetJsonAsync / GetBytesAsync /
                                  BuildRequestUri, formatters, Dispose, JsonSerializerOptions
    EodhdClient.<Domain>.cs     ← EodhdClient is a PARTIAL class; each endpoint group is its own
                                  file (Reference, MarketData, Fundamentals, Calendar, News,
                                  TechnicalIndicators, Screener, Options, Cboe, Treasury, Ticks, Logos)
    DataModel/                  ← STJ DTOs, one folder per domain (DTOs only — no behavior)
    Live/                       ← WebSocket client: EodhdWebSocketClient<T> + factories
    Transport/                  ← IHttpTransport / IWebSocketConnection seams + default impls
    Exceptions/                 ← EodhdHttpException + EodhdErrorCase
tests/
  EODHD.CSharpApiClient.UnitTests/          ← deterministic, no network (mock transports)
  EODHD.CSharpApiClient.IntegrationTests/   ← live API, skipped without EODHD_API_KEY
.editorconfig                   ← style rules enforced as build errors (see "Coding conventions")
.github/workflows/ci.yml        ← CI: build + unit tests on every push/PR
.github/workflows/publish.yml   ← publish on v* tag (NuGet Trusted Publishing / OIDC)
```

The solution file is **`EODHD.CSharpApiClient.slnx`** (the XML solution format) — pass it by name to
`dotnet build` / `test` / `format`.

---

## Contribution model — PRs only, squash-merge on a rebased branch

**No one pushes directly to `main`.** Every change goes through a Pull Request.

- Create a feature branch off `main` (`git checkout -b feat/my-change`).
- Make changes, build, test, and run the format gate locally.
- **Rebase on the latest `main`** before merging (`git fetch origin && git rebase origin/main`).
- Push and open a PR; address review; **squash-merge** via the GitHub UI.

Merge settings: **squash-merge only** — merge and rebase-merge are disabled, the head branch is
auto-deleted, and the squash commit takes the PR title/body. Because squashing rewrites history,
**build linearly** — merge each PR before starting the next; don't stack PRs.

---

## Building

```bash
dotnet build EODHD.CSharpApiClient.slnx -c Release
```

Zero warnings required. `GenerateDocumentationFile=true` is set in **every** project and `CS1591` is an
error in the library, so every public member needs an XML `<summary>`.

---

## Tests

```bash
# Unit tests (fast, no network, always run)
dotnet test tests/EODHD.CSharpApiClient.UnitTests/EODHD.CSharpApiClient.UnitTests.csproj -c Release

# Integration tests (live API)
export EODHD_API_KEY=...
dotnet test tests/EODHD.CSharpApiClient.IntegrationTests/EODHD.CSharpApiClient.IntegrationTests.csproj -c Release
```

Integration tests use `[SkippableFact]`; without `EODHD_API_KEY` they skip cleanly (never fail on a
missing key). Subscription-gated endpoints skip on `402`/`403` via `SkipIfNoLicense`. EODHD's public
`demo` token serves a few US symbols for endpoints that take a per-symbol argument. CI runs unit tests;
integration runs only where a key is configured.

---

## Coding conventions

These are enforced as **build errors** via `.editorconfig` + `<EnforceCodeStyleInBuild>`:

- **No external runtime dependencies** — the whole value proposition is zero deps on .NET 8+.
- **Async-first.** Every public endpoint is `Task`-returning with a trailing `CancellationToken ct = default`;
  the synchronous wrapper delegates via `.GetAwaiter().GetResult()` and holds no logic of its own.
- **API key supplied at construction** — never read from the environment or a config file inside the library.
- **`System.Text.Json` only.** `[JsonPropertyName("...")]` carries the exact wire key; deserialization is
  globally case-insensitive and reads numbers from JSON strings (`PropertyNameCaseInsensitive` +
  `NumberHandling.AllowReadingFromString`), so per-property number attributes are rarely needed.
- **Seams for tests:** `IHttpTransport` (REST) and `IWebSocketConnection` (live) — production uses the
  default implementations; tests inject deterministic mocks.
- **XML docs on every public member** (`CS1591` is an error).
- Allman braces, 4-space indent, **no `var`** (explicit types only), `this.` qualification, **no space**
  after control-flow keywords (`if(`, `foreach(`, `catch(`), `System.*` usings first then a blank line,
  no unused usings (IDE0005), no consecutive blank lines (IDE2000). Analyzers `CA1051/1707/1711/1822/1859`
  are errors. Test projects `NoWarn CA1707;CA1861;CS1591`.

**Formatting gate — run before every PR:** `dotnet format EODHD.CSharpApiClient.slnx --verify-no-changes`
must exit `0`. `dotnet format` auto-fixes most violations; `CA1051` (visible field → property) is manual.

### Modeling data

- **Model from live responses, not just the docs.** The published docs sometimes disagree with the wire
  (field casing, types). Verify each endpoint against a live probe — and an official client library in
  another language when one exists — before finalizing; the live integration suite is the final gate.
- Large financial / share-count fields → `decimal?` (they exceed `Int64` or carry decimals).
- Messy, zero-sentinel, or mixed-format timestamps → keep as `string`.
- Unix-epoch fields carry their unit in the name: `…Ms` / `…Sec` (the raw `long?`), plus a `…Utc` companion
  (`DateTimeOffset?`) computed via the internal `UnixTime` helper — pick `FromMilliseconds` vs `FromSeconds`
  by the field's true unit (units are mixed across endpoints).
- JSON:API-style marketplace endpoints (`data[].attributes`, `filter[...]`, `page[...]`) unwrap via internal
  envelope types into flat arrays.

---

## Live streaming (WebSocket)

- `EodhdWebSocketClient<TMessage>`, created via `EodhdWebSocketClient.Forex` / `.Crypto` / `.UsTrades` /
  `.UsQuotes`. Messages are delivered as `IAsyncEnumerable<T>` (`ReadMessagesAsync`); `SubscribeAsync` /
  `UnsubscribeAsync` adjust the symbol set (≤ 50) on the fly. Built on `System.Net.WebSockets` — no new dep.
- Wire: `wss://ws.eodhistoricaldata.com/ws/{feed}?api_token=…`; subscribe with
  `{"action":"subscribe","symbols":"A,B"}`. The first frame is a control message (`{"status_code":200,…}`)
  and is skipped — only data frames are surfaced.
- Auto-reconnects with bounded exponential backoff and replays subscriptions. The reconnect-attempt budget
  resets only after a connection that actually delivered data, so a connection that opens then immediately
  drops is bounded by `MaxReconnectAttempts` rather than looping forever.
- There is **no options or index** WebSocket stream (the server returns `404 "No market found"`); the four
  markets are `us`, `us-quote`, `forex`, `crypto`. Use an index ETF (SPY, QQQ) for live index exposure. The
  `us` / `us-quote` feeds stream during regular and extended (pre/post-market) hours only.

---

## Adding a new endpoint

1. Add the `async` + sync methods to the relevant `EodhdClient.<Domain>.cs` partial (or a new partial file
   for a new group).
2. Add/update the DTO under `DataModel/<Domain>/` with `[JsonPropertyName]` and XML docs.
3. Add a unit test in `UnitTests/` (mock transport).
4. Add a `[SkippableFact]` integration test in `IntegrationTests/` (inherit `IntegrationTestBase`, call
   `SkipIfNoApiKey()`, and `SkipIfNoLicense` for subscription-gated endpoints).
5. Build clean, run the format gate + the full test suite, open a PR.

---

## Releasing to NuGet.org

1. Bump `<Version>` in `src/EODHD.CSharpApiClient/EODHD.CSharpApiClient.csproj` and cut the matching
   `CHANGELOG.md` section + `<PackageReleaseNotes>` in a preparation PR; merge it.
2. After merge, tag from `main`: `git tag vX.Y.Z && git push origin vX.Y.Z`.
3. `publish.yml` fires on the `v*` tag, builds Release, packs, and pushes to NuGet.org via **Trusted
   Publishing** (OIDC — no stored API key). Releases publish to **NuGet only** (no GitHub Releases).

> **Do not create a release tag unless the maintainer explicitly asks** — a tag publishes immediately and a
> published version cannot be unpublished.
>
> NuGet.org renders the README and release notes embedded in the `.nupkg` at pack time, not the current repo
> state. Docs-only changes need a patch bump + republish to show up.
