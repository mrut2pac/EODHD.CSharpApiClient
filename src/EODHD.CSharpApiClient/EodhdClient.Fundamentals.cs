using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

using EODHD.CSharpApiClient.DataModel.Bonds;
using EODHD.CSharpApiClient.DataModel.BulkFundamental;
using EODHD.CSharpApiClient.DataModel.EarningsTrends;
using EODHD.CSharpApiClient.DataModel.Fundamental;
using EODHD.CSharpApiClient.DataModel.InsiderTransactions;

namespace EODHD.CSharpApiClient
{
    public sealed partial class EodhdClient
    {
        // ================================================================
        // Fundamental Data API
        // ================================================================

        /// <summary>
        /// Returns the full fundamentals dataset for a symbol, or only the requested top-level sections.
        /// </summary>
        /// <param name="symbol">The full EODHD symbol (e.g. <c>"AAPL.US"</c>).</param>
        /// <param name="filters">Optional comma-separated top-level sections to return, by their wire names (e.g.
        /// <c>"General,Highlights"</c>, <c>"ETF_Data"</c>, <c>"MutualFund_Data"</c>). A section the symbol lacks, or an
        /// unknown name, is left <c>null</c>. Field paths (<c>"General::Code"</c>) are not supported.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The fundamentals data.</returns>
        public async Task<FundamentalData> GetFundamentalsDataAsync(string symbol, string filters = null, CancellationToken ct = default)
        {
            if(string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentNullException(nameof(symbol));
            }

            string path = "fundamentals/" + Uri.EscapeDataString(symbol);
            if(string.IsNullOrWhiteSpace(filters))
            {
                return await this.GetJsonAsync<FundamentalData>(path, ct).ConfigureAwait(false);
            }

            if(filters.Contains("::", StringComparison.Ordinal))
            {
                throw new ArgumentException(
                    "Field paths (Section::Field) return values that do not fit FundamentalData; filter by whole sections, e.g. \"General,Highlights\".",
                    nameof(filters));
            }

            string[] sections = filters
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray();
            if(sections.Length == 0)
            {
                throw new ArgumentException("No section names were given.", nameof(filters));
            }

            // Read as a JsonElement: a JsonNode read with the client's case-insensitive options would build
            // case-insensitive objects, which reject the case-variant duplicate keys EODHD sends.
            JsonElement body = await this.GetJsonAsync<JsonElement>(path, ct, ("filter", string.Join(",", sections))).ConfigureAwait(false);
            return ToFilteredFundamentals(sections, body);
        }

        /// <summary>
        /// Returns the full fundamentals dataset for a symbol, or only the requested top-level sections.
        /// </summary>
        /// <param name="symbol">The full EODHD symbol (e.g. <c>"AAPL.US"</c>).</param>
        /// <param name="filters">Optional comma-separated top-level sections to return, by their wire names (e.g.
        /// <c>"General,Highlights"</c>, <c>"ETF_Data"</c>, <c>"MutualFund_Data"</c>). A section the symbol lacks, or an
        /// unknown name, is left <c>null</c>. Field paths (<c>"General::Code"</c>) are not supported.</param>
        /// <returns>The fundamentals data.</returns>
        public FundamentalData GetFundamentalsData(string symbol, string filters = null)
        {
            return this.GetFundamentalsDataAsync(symbol, filters).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Returns bulk fundamentals for all symbols on an exchange, keyed by symbol code.
        /// </summary>
        /// <param name="exchangeCode">The EODHD exchange code (e.g. <c>"US"</c>).</param>
        /// <param name="offset">Optional pagination offset.</param>
        /// <param name="limit">Optional page size.</param>
        /// <param name="symbols">Optional list of symbols to restrict the response to.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A dictionary of bulk fundamentals keyed by symbol code.</returns>
        public Task<Dictionary<string, BulkFundamentalData>> GetBulkFundamentalsDataAsync(string exchangeCode, int? offset = null, int? limit = null, IReadOnlyList<string> symbols = null, CancellationToken ct = default)
        {
            if(string.IsNullOrWhiteSpace(exchangeCode))
            {
                throw new ArgumentNullException(nameof(exchangeCode));
            }

            return this.GetJsonAsync<Dictionary<string, BulkFundamentalData>>(
                "bulk-fundamentals/" + Uri.EscapeDataString(exchangeCode),
                ct,
                ("offset", FormatInt(offset)),
                ("limit", FormatInt(limit)),
                ("symbols", JoinSymbols(symbols)));
        }

        /// <summary>
        /// Returns bulk fundamentals for all symbols on an exchange, keyed by symbol code.
        /// </summary>
        /// <param name="exchangeCode">The EODHD exchange code (e.g. <c>"US"</c>).</param>
        /// <param name="offset">Optional pagination offset.</param>
        /// <param name="limit">Optional page size.</param>
        /// <param name="symbols">Optional list of symbols to restrict the response to.</param>
        /// <returns>A dictionary of bulk fundamentals keyed by symbol code.</returns>
        public Dictionary<string, BulkFundamentalData> GetBulkFundamentalsData(string exchangeCode, int? offset = null, int? limit = null, IReadOnlyList<string> symbols = null)
        {
            return this.GetBulkFundamentalsDataAsync(exchangeCode, offset, limit, symbols).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Returns the extended (version 1.2) bulk fundamentals for an exchange as a list.
        /// </summary>
        /// <param name="exchangeCode">The EODHD exchange code (e.g. <c>"US"</c>).</param>
        /// <param name="offset">Optional pagination offset.</param>
        /// <param name="limit">Optional page size.</param>
        /// <param name="symbols">Optional list of symbols to restrict the response to.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The bulk fundamentals records.</returns>
        public Task<List<BulkFundamentalData>> GetBulkFundamentalsExtendedDataAsync(string exchangeCode, int? offset = null, int? limit = null, IReadOnlyList<string> symbols = null, CancellationToken ct = default)
        {
            if(string.IsNullOrWhiteSpace(exchangeCode))
            {
                throw new ArgumentNullException(nameof(exchangeCode));
            }

            return this.GetJsonAsync<List<BulkFundamentalData>>(
                "bulk-fundamentals/" + Uri.EscapeDataString(exchangeCode),
                ct,
                ("version", "1.2"),
                ("offset", FormatInt(offset)),
                ("limit", FormatInt(limit)),
                ("symbols", JoinSymbols(symbols)));
        }

        /// <summary>
        /// Returns the extended (version 1.2) bulk fundamentals for an exchange as a list.
        /// </summary>
        /// <param name="exchangeCode">The EODHD exchange code (e.g. <c>"US"</c>).</param>
        /// <param name="offset">Optional pagination offset.</param>
        /// <param name="limit">Optional page size.</param>
        /// <param name="symbols">Optional list of symbols to restrict the response to.</param>
        /// <returns>The bulk fundamentals records.</returns>
        public List<BulkFundamentalData> GetBulkFundamentalsExtendedData(string exchangeCode, int? offset = null, int? limit = null, IReadOnlyList<string> symbols = null)
        {
            return this.GetBulkFundamentalsExtendedDataAsync(exchangeCode, offset, limit, symbols).GetAwaiter().GetResult();
        }

        // ================================================================
        // Insider Transactions API
        // ================================================================

        /// <summary>
        /// Returns insider (SEC Form 4) transactions, optionally filtered by symbol and date range.
        /// </summary>
        /// <param name="symbol">Optional ticker to filter to (e.g. <c>"AAPL.US"</c>); <c>null</c> returns all symbols.</param>
        /// <param name="from">Optional inclusive start date (default: one year ago).</param>
        /// <param name="to">Optional inclusive end date (default: today).</param>
        /// <param name="limit">Optional number of entries (1–1000, default 100).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The insider transactions.</returns>
        public Task<InsiderTransaction[]> GetInsiderTransactionsAsync(string symbol = null, DateTime? from = null, DateTime? to = null, int? limit = null, CancellationToken ct = default)
        {
            return this.GetJsonAsync<InsiderTransaction[]>(
                "insider-transactions",
                ct,
                ("code", symbol),
                ("from", FormatDate(from)),
                ("to", FormatDate(to)),
                ("limit", FormatInt(limit)));
        }

        /// <summary>
        /// Returns insider (SEC Form 4) transactions, optionally filtered by symbol and date range.
        /// </summary>
        /// <param name="symbol">Optional ticker to filter to (e.g. <c>"AAPL.US"</c>); <c>null</c> returns all symbols.</param>
        /// <param name="from">Optional inclusive start date (default: one year ago).</param>
        /// <param name="to">Optional inclusive end date (default: today).</param>
        /// <param name="limit">Optional number of entries (1–1000, default 100).</param>
        /// <returns>The insider transactions.</returns>
        public InsiderTransaction[] GetInsiderTransactions(string symbol = null, DateTime? from = null, DateTime? to = null, int? limit = null)
        {
            return this.GetInsiderTransactionsAsync(symbol, from, to, limit).GetAwaiter().GetResult();
        }

        // ================================================================
        // Bond Fundamentals API
        // ================================================================

        /// <summary>
        /// Returns the fundamentals for a single bond, identified by ISIN.
        /// </summary>
        /// <param name="isin">The bond ISIN (e.g. <c>"DE000CB83CF0"</c>).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The bond fundamentals.</returns>
        public Task<BondFundamentals> GetBondFundamentalsAsync(string isin, CancellationToken ct = default)
        {
            if(string.IsNullOrWhiteSpace(isin))
            {
                throw new ArgumentNullException(nameof(isin));
            }

            return this.GetJsonAsync<BondFundamentals>("bond-fundamentals/" + Uri.EscapeDataString(isin), ct);
        }

        /// <summary>
        /// Returns the fundamentals for a single bond, identified by ISIN.
        /// </summary>
        /// <param name="isin">The bond ISIN (e.g. <c>"DE000CB83CF0"</c>).</param>
        /// <returns>The bond fundamentals.</returns>
        public BondFundamentals GetBondFundamentals(string isin)
        {
            return this.GetBondFundamentalsAsync(isin).GetAwaiter().GetResult();
        }

        // ================================================================
        // Earnings Trends API
        // ================================================================

        /// <summary>
        /// Returns earnings-trend records (analyst estimates, EPS trend, and revisions) for one or more
        /// symbols. The API nests one record set per symbol; the results are flattened into a single
        /// array (each record carries its own <see cref="EarningsTrend.Code"/>).
        /// </summary>
        /// <param name="symbols">The symbols to fetch trends for (e.g. <c>"AAPL.US"</c>, <c>"MSFT.US"</c>).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The earnings-trend records across all requested symbols.</returns>
        public async Task<EarningsTrend[]> GetEarningsTrendsAsync(IReadOnlyList<string> symbols, CancellationToken ct = default)
        {
            if(symbols == null || symbols.Count == 0)
            {
                throw new ArgumentException("At least one symbol must be supplied.", nameof(symbols));
            }

            EarningsTrendsResponse response = await this.GetJsonAsync<EarningsTrendsResponse>(
                "calendar/trends",
                ct,
                ("symbols", JoinSymbols(symbols))).ConfigureAwait(false);

            if(response?.Trends == null)
            {
                return Array.Empty<EarningsTrend>();
            }

            List<EarningsTrend> flattened = new List<EarningsTrend>();
            foreach(EarningsTrend[] perSymbol in response.Trends)
            {
                if(perSymbol != null)
                {
                    flattened.AddRange(perSymbol);
                }
            }

            return flattened.ToArray();
        }

        /// <summary>
        /// Returns earnings-trend records (analyst estimates, EPS trend, and revisions) for one or more
        /// symbols, flattened into a single array (each record carries its own
        /// <see cref="EarningsTrend.Code"/>).
        /// </summary>
        /// <param name="symbols">The symbols to fetch trends for (e.g. <c>"AAPL.US"</c>, <c>"MSFT.US"</c>).</param>
        /// <returns>The earnings-trend records across all requested symbols.</returns>
        public EarningsTrend[] GetEarningsTrends(IReadOnlyList<string> symbols)
        {
            return this.GetEarningsTrendsAsync(symbols).GetAwaiter().GetResult();
        }

        // A filtered response is shaped differently from the full one: a single section comes back unwrapped
        // (the section's own object), and a section the symbol lacks comes back as "NA" (the whole body for a
        // single section, or that section's value for several). Re-wrap and drop "NA" so it binds as usual.
        private static FundamentalData ToFilteredFundamentals(string[] sections, JsonElement body)
        {
            JsonNode node = JsonNode.Parse(body.GetRawText());
            JsonObject root;
            if(sections.Length == 1)
            {
                root = new JsonObject { [sections[0]] = node };
            }
            else if(node == null)
            {
                root = new JsonObject();
            }
            else
            {
                root = node as JsonObject ?? throw new JsonException("Expected a JSON object for a multi-section fundamentals filter.");
            }

            List<string> missing = new List<string>();
            foreach(KeyValuePair<string, JsonNode> section in root)
            {
                if(section.Value is JsonValue value && value.TryGetValue(out string text) && string.Equals(text, "NA", StringComparison.Ordinal))
                {
                    missing.Add(section.Key);
                }
            }

            foreach(string key in missing)
            {
                root.Remove(key);
            }

            return root.Deserialize<FundamentalData>(DeserializeOptions);
        }
    }
}
