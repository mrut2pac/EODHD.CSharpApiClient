# EODHD.CSharpApiClient

Pure-managed C# client for the [EOD Historical Data (EODHD)](https://eodhd.com/financial-apis) API.

- **Async-first** — every endpoint is `Task`-returning with an optional `CancellationToken`; synchronous convenience wrappers are provided alongside.
- **Zero runtime dependencies** — uses `System.Text.Json` (inbox on .NET 8+). No `Newtonsoft.Json`.
- **Testable transport** — an `IHttpTransport` seam lets you mock the network in unit tests; the API key is supplied at construction.

> **Status:** pre-release (`0.x`). The public surface is still being built out toward 100% REST + WebSocket coverage. See [CHANGELOG.md](CHANGELOG.md) for the full version history.

## Install

```bash
dotnet add package EODHD.CSharpApiClient
```

## Quick start

```csharp
using EODHD.CSharpApiClient;

using EodhdClient client = new EodhdClient("YOUR_API_TOKEN");

UserInfo user = await client.GetUserInfoAsync();
Console.WriteLine($"{user.Name}: {user.ApiRequests}/{user.DailyRateLimit} requests today");
```

A demo token (`demo`) works against a handful of US symbols (e.g. `AAPL.US`, `MCD.US`) for trying the client without a paid key.

## API key

Pass your token to the constructor — the client never reads it from the environment or a config file:

```csharp
EodhdClient client = new EodhdClient("YOUR_API_TOKEN");
```

For finer control (timeout, user agent, base URI) use `EodhdClientOptions`:

```csharp
EodhdClient client = new EodhdClient(new EodhdClientOptions
{
    ApiToken = "YOUR_API_TOKEN",
    Timeout = TimeSpan.FromSeconds(60),
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
