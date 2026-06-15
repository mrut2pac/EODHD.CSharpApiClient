# Changelog

All notable changes to this project are documented here. The format is based on
[Keep a Changelog](https://keepachangelog.com/en/1.1.0/), and this project adheres to
[Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
- Initial repository scaffold: solution, package metadata, MIT license, CI + publish
  workflows (NuGet Trusted Publishing / OIDC), and the testing layout.
- `EodhdClient` walking skeleton — async-first HTTP client with an `IHttpTransport`
  seam, `EodhdClientOptions`, and a typed `EodhdHttpException` / `EodhdErrorCase`.
- First endpoint: `GetUserInfoAsync` / `GetUserInfo` (User API) with unit and
  integration test coverage.

[Unreleased]: https://github.com/mrut2pac/EODHD.CSharpApiClient/commits/main
