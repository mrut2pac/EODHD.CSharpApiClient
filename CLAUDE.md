# CLAUDE.md — EODHD.CSharpApiClient

Guidelines for working on this repository with Claude Code or any AI coding assistant.

---

## Repo overview

Pure-managed C# (.NET 8) client for the [EOD Historical Data (EODHD) API](https://eodhd.com/financial-apis).
No external runtime dependencies — only `System.Text.Json` (inbox on .NET 8+).

```
src/
  EODHD.CSharpApiClient/        ← the library (packaged to NuGet)
    Transport/                  ← IHttpTransport seam + default HttpClient-backed impl
    Exceptions/                 ← EodhdHttpException + EodhdErrorCase
    DataModel/                  ← STJ model classes
    EodhdClient.cs              ← primary async-first REST client
tests/
  EODHD.CSharpApiClient.UnitTests/          ← deterministic, no network (mock IHttpTransport)
  EODHD.CSharpApiClient.IntegrationTests/   ← live API, skipped without EODHD_API_KEY
.github/workflows/ci.yml        ← CI: build + unit tests on every push/PR
.github/workflows/publish.yml   ← publish on v* tag (NuGet Trusted Publishing / OIDC)
```

---

## Contribution model — PRs only, squash-merge on a rebased branch

**No one pushes directly to `main`.** Every change goes through a Pull Request.

- Create a feature branch off `main` (`git checkout -b feat/my-change`)
- Make changes, build, test locally
- **Rebase the branch on the latest `main`** before merging (`git fetch origin && git rebase origin/main`)
- Push the branch and open a PR on GitHub
- Address review comments, then **squash-merge** via the GitHub UI

Repo merge settings (matching the Databento client repo): **squash-merge only** — merge
commits and rebase-merge are disabled, the head branch is auto-deleted on merge, and the
squash commit takes the PR title and body. With squash-only, `main` stays linear by
construction; rebasing the branch first keeps the squashed commit clean and conflict-free.

---

## Building

```bash
dotnet build EODHD.CSharpApiClient.slnx          # Debug by default
dotnet build EODHD.CSharpApiClient.slnx -c Release
```

Zero warnings required — `GenerateDocumentationFile=true` is enabled and `CS1591` is treated
as an error. Every public API surface needs an XML `<summary>` doc comment.

---

## Tests

```bash
# Unit tests only (fast, no network, always run)
dotnet test EODHD.CSharpApiClient.slnx --filter "FullyQualifiedName!~IntegrationTests"

# Integration tests (requires a live EODHD API key)
export EODHD_API_KEY=...
dotnet test tests/EODHD.CSharpApiClient.IntegrationTests
```

Integration tests use `[SkippableFact]` from `Xunit.SkippableFact`. Without the env var they
skip cleanly with an explanatory message — they never fail due to a missing key. CI runs unit
tests; integration tests are skipped there unless an `EODHD_API_KEY` secret is configured.

---

## Coding conventions

- **No external runtime dependencies.** The whole value proposition is zero deps on .NET 8+.
- **Async-first.** Every public endpoint is `Task`-returning with a trailing
  `CancellationToken ct = default`; a synchronous wrapper alongside delegates via
  `.GetAwaiter().GetResult()` and contains no logic of its own.
- **API key supplied at construction** — never read from the environment or a config file
  inside the library. (Tests read `EODHD_API_KEY` themselves and pass it in.)
- **`System.Text.Json` only** — no Newtonsoft. `[JsonPropertyName("...")]` on every
  deserialized property where the wire name differs from the C# name.
- **Numeric fields encoded as JSON strings** (common in financial APIs) need
  `[JsonNumberHandling(AllowReadingFromString)]` or a custom `JsonConverter` — not both.
- **`IHttpTransport` seam** for unit tests — production uses the default `HttpClient`-backed
  transport; unit tests inject a deterministic mock.
- **XML doc on every public member** — required by `GenerateDocumentationFile=true`.
- Allman brace style, 4-space indent, no `var` (explicit types only), `this.` qualification.

---

## Adding a new endpoint

1. Add the async + sync methods to `EodhdClient`.
2. Add/update the data model class under `DataModel/` with `[JsonPropertyName]` and XML docs.
3. Add a unit test in `UnitTests/` (mock transport via `IHttpTransport`).
4. Add an integration test in `IntegrationTests/` inheriting `IntegrationTestBase`, using
   `[SkippableFact]` + `SkipIfNoApiKey()`.
5. Build clean, run unit tests, open a PR.

---

## Releasing to NuGet.org

1. Bump `<Version>` in `src/EODHD.CSharpApiClient/EODHD.CSharpApiClient.csproj` and update
   `CHANGELOG.md` + `<PackageReleaseNotes>`.
2. Commit the bump on a branch and merge via PR.
3. After merge, on `main`: `git tag vX.Y.Z && git push origin vX.Y.Z`.
4. The `publish` workflow fires on the `v*` tag, builds Release, packs, and pushes to
   NuGet.org via Trusted Publishing (OIDC — no stored API key). `--skip-duplicate` is set.

> NuGet.org renders the README and release notes embedded in the `.nupkg` at pack time, not
> the current repo state. Docs-only changes need a patch bump + republish to show up.
