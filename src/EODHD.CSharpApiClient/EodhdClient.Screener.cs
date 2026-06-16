using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

using EODHD.CSharpApiClient.DataModel;
using EODHD.CSharpApiClient.DataModel.Bonds;
using EODHD.CSharpApiClient.DataModel.BulkFundamental;
using EODHD.CSharpApiClient.DataModel.Cboe;
using EODHD.CSharpApiClient.DataModel.Commodities;
using EODHD.CSharpApiClient.DataModel.EarningsTrends;
using EODHD.CSharpApiClient.DataModel.EconomicEvents;
using EODHD.CSharpApiClient.DataModel.ExchangeInfo;
using EODHD.CSharpApiClient.DataModel.Fundamental;
using EODHD.CSharpApiClient.DataModel.IdMappings;
using EODHD.CSharpApiClient.DataModel.InsiderTransactions;
using EODHD.CSharpApiClient.DataModel.Macro;
using EODHD.CSharpApiClient.DataModel.MarketCap;
using EODHD.CSharpApiClient.DataModel.News;
using EODHD.CSharpApiClient.DataModel.Options;
using EODHD.CSharpApiClient.DataModel.Quotes;
using EODHD.CSharpApiClient.DataModel.Screener;
using EODHD.CSharpApiClient.DataModel.Sentiment;
using EODHD.CSharpApiClient.DataModel.TechnicalIndicators;
using EODHD.CSharpApiClient.DataModel.Treasury;
using EODHD.CSharpApiClient.DataModel.UpcomingDividends;
using EODHD.CSharpApiClient.DataModel.UpcomingEarnings;
using EODHD.CSharpApiClient.DataModel.UpcomingIpos;
using EODHD.CSharpApiClient.DataModel.UpcomingSplits;
using EODHD.CSharpApiClient.Exceptions;
using EODHD.CSharpApiClient.Transport;

namespace EODHD.CSharpApiClient
{
    public sealed partial class EodhdClient
    {
        // ================================================================
        // Stock Market Screener API
        // ================================================================

        /// <summary>
        /// Screens instruments by filter conditions and/or pre-defined signals.
        /// </summary>
        /// <param name="filters">Optional filter conditions, combined with logical AND.</param>
        /// <param name="signals">
        /// Optional pre-defined signals to require (e.g. <c>200d_new_hi</c>, <c>200d_new_lo</c>,
        /// <c>bookvalue_neg</c>, <c>bookvalue_pos</c>, <c>wallstreet_lo</c>, <c>wallstreet_hi</c>).
        /// </param>
        /// <param name="sort">Optional sort over a numeric field, formatted <c>field.asc</c> or <c>field.desc</c>.</param>
        /// <param name="limit">Optional results per request (1–100, default 50).</param>
        /// <param name="offset">Optional pagination offset (0–999, default 0).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The matching instruments.</returns>
        public async Task<ScreenerResult[]> GetScreenerAsync(IReadOnlyList<ScreenerFilter> filters = null, IReadOnlyList<string> signals = null, string sort = null, int? limit = null, int? offset = null, CancellationToken ct = default)
        {
            ScreenerResponse response = await this.GetJsonAsync<ScreenerResponse>(
                "screener",
                ct,
                ("filters", SerializeScreenerFilters(filters)),
                ("signals", JoinSymbols(signals)),
                ("sort", sort),
                ("limit", FormatInt(limit)),
                ("offset", FormatInt(offset))).ConfigureAwait(false);

            return response?.Data ?? Array.Empty<ScreenerResult>();
        }

        /// <summary>
        /// Screens instruments by filter conditions and/or pre-defined signals.
        /// </summary>
        /// <param name="filters">Optional filter conditions, combined with logical AND.</param>
        /// <param name="signals">Optional pre-defined signals to require.</param>
        /// <param name="sort">Optional sort over a numeric field, formatted <c>field.asc</c> or <c>field.desc</c>.</param>
        /// <param name="limit">Optional results per request (1–100, default 50).</param>
        /// <param name="offset">Optional pagination offset (0–999, default 0).</param>
        /// <returns>The matching instruments.</returns>
        public ScreenerResult[] GetScreener(IReadOnlyList<ScreenerFilter> filters = null, IReadOnlyList<string> signals = null, string sort = null, int? limit = null, int? offset = null)
        {
            return this.GetScreenerAsync(filters, signals, sort, limit, offset).GetAwaiter().GetResult();
        }

        // ================================================================
        // Macroeconomic Indicators API
        // ================================================================

        /// <summary>
        /// Returns a macroeconomic indicator time series for a country.
        /// </summary>
        /// <param name="countryCode">
        /// The ISO 3166-1 alpha-3 country code (e.g. <c>"USA"</c>) or a World Bank aggregate code
        /// (e.g. <c>"WLD"</c>, <c>"EUU"</c>).
        /// </param>
        /// <param name="indicator">The indicator to fetch. Defaults to <see cref="MacroIndicator.GdpCurrentUsd"/>.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The indicator observations, most recent first.</returns>
        public Task<MacroIndicatorValue[]> GetMacroIndicatorAsync(string countryCode, MacroIndicator indicator = MacroIndicator.GdpCurrentUsd, CancellationToken ct = default)
        {
            if(string.IsNullOrWhiteSpace(countryCode))
            {
                throw new ArgumentNullException(nameof(countryCode));
            }

            return this.GetJsonAsync<MacroIndicatorValue[]>(
                "macro-indicator/" + Uri.EscapeDataString(countryCode),
                ct,
                ("indicator", MacroIndicatorToString(indicator)));
        }

        /// <summary>
        /// Returns a macroeconomic indicator time series for a country.
        /// </summary>
        /// <param name="countryCode">
        /// The ISO 3166-1 alpha-3 country code (e.g. <c>"USA"</c>) or a World Bank aggregate code.
        /// </param>
        /// <param name="indicator">The indicator to fetch. Defaults to <see cref="MacroIndicator.GdpCurrentUsd"/>.</param>
        /// <returns>The indicator observations, most recent first.</returns>
        public MacroIndicatorValue[] GetMacroIndicator(string countryCode, MacroIndicator indicator = MacroIndicator.GdpCurrentUsd)
        {
            return this.GetMacroIndicatorAsync(countryCode, indicator).GetAwaiter().GetResult();
        }

        // ================================================================
        // Historical Market Capitalization API
        // ================================================================

        /// <summary>
        /// Returns the weekly historical market capitalisation for a symbol, ordered oldest-first.
        /// </summary>
        /// <param name="symbol">The full EODHD symbol (e.g. <c>"AAPL.US"</c>).</param>
        /// <param name="from">Optional inclusive start date.</param>
        /// <param name="to">Optional inclusive end date.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The weekly market-cap data points.</returns>
        public async Task<HistoricalMarketCap[]> GetHistoricalMarketCapAsync(string symbol, DateTime? from = null, DateTime? to = null, CancellationToken ct = default)
        {
            if(string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentNullException(nameof(symbol));
            }

            // The endpoint returns a JSON object keyed by sequential index ("0", "1", ...); project the
            // values into a date-ordered array so callers get a stable, idiomatic sequence.
            Dictionary<string, HistoricalMarketCap> map = await this.GetJsonAsync<Dictionary<string, HistoricalMarketCap>>(
                "historical-market-cap/" + Uri.EscapeDataString(symbol),
                ct,
                ("from", FormatDate(from)),
                ("to", FormatDate(to))).ConfigureAwait(false);

            if(map == null || map.Count == 0)
            {
                return Array.Empty<HistoricalMarketCap>();
            }

            HistoricalMarketCap[] points = new HistoricalMarketCap[map.Count];
            map.Values.CopyTo(points, 0);
            Array.Sort(points, (left, right) => left.Date.CompareTo(right.Date));
            return points;
        }

        /// <summary>
        /// Returns the weekly historical market capitalisation for a symbol, ordered oldest-first.
        /// </summary>
        /// <param name="symbol">The full EODHD symbol (e.g. <c>"AAPL.US"</c>).</param>
        /// <param name="from">Optional inclusive start date.</param>
        /// <param name="to">Optional inclusive end date.</param>
        /// <returns>The weekly market-cap data points.</returns>
        public HistoricalMarketCap[] GetHistoricalMarketCap(string symbol, DateTime? from = null, DateTime? to = null)
        {
            return this.GetHistoricalMarketCapAsync(symbol, from, to).GetAwaiter().GetResult();
        }

        private static string SerializeScreenerFilters(IReadOnlyList<ScreenerFilter> filters)
        {
            if(filters == null || filters.Count == 0)
            {
                return null;
            }

            // EODHD expects a JSON array of [field, operation, value] triples, e.g.
            // [["market_capitalization",">",1000000000000],["sector","=","Technology"]].
            object[][] triples = new object[filters.Count][];
            for(int i = 0; i < filters.Count; i++)
            {
                ScreenerFilter filter = filters[i];
                triples[i] = new object[] { filter.Field, filter.Operation, filter.Value };
            }

            return JsonSerializer.Serialize(triples, FilterSerializeOptions);
        }

        private static string MacroIndicatorToString(MacroIndicator indicator)
        {
            switch(indicator)
            {
                case MacroIndicator.GdpCurrentUsd:
                    return "gdp_current_usd";
                case MacroIndicator.GdpPerCapitaUsd:
                    return "gdp_per_capita_usd";
                case MacroIndicator.GdpGrowthAnnual:
                    return "gdp_growth_annual";
                case MacroIndicator.GniUsd:
                    return "gni_usd";
                case MacroIndicator.GniPerCapitaUsd:
                    return "gni_per_capita_usd";
                case MacroIndicator.GniPppUsd:
                    return "gni_ppp_usd";
                case MacroIndicator.GniPerCapitaPppUsd:
                    return "gni_per_capita_ppp_usd";
                case MacroIndicator.GrossCapitalFormationPercentGdp:
                    return "gross_capital_formation_percent_gdp";
                case MacroIndicator.AgricultureValueAddedPercentGdp:
                    return "agriculture_value_added_percent_gdp";
                case MacroIndicator.IndustryValueAddedPercentGdp:
                    return "industry_value_added_percent_gdp";
                case MacroIndicator.ServicesValueAddedPercentGdp:
                    return "services_value_added_percent_gdp";
                case MacroIndicator.InflationConsumerPricesAnnual:
                    return "inflation_consumer_prices_annual";
                case MacroIndicator.ConsumerPriceIndex:
                    return "consumer_price_index";
                case MacroIndicator.InflationGdpDeflatorAnnual:
                    return "inflation_gdp_deflator_annual";
                case MacroIndicator.RealInterestRate:
                    return "real_interest_rate";
                case MacroIndicator.NetTradesGoodsServices:
                    return "net_trades_goods_services";
                case MacroIndicator.ExportsOfGoodsServicesPercentGdp:
                    return "exports_of_goods_services_percent_gdp";
                case MacroIndicator.ImportsOfGoodsServicesPercentGdp:
                    return "imports_of_goods_services_percent_gdp";
                case MacroIndicator.MerchandiseTradePercentGdp:
                    return "merchandise_trade_percent_gdp";
                case MacroIndicator.HighTechnologyExportsPercentTotal:
                    return "high_technology_exports_percent_total";
                case MacroIndicator.DebtPercentGdp:
                    return "debt_percent_gdp";
                case MacroIndicator.RevenueExcludingGrantsPercentGdp:
                    return "revenue_excluding_grants_percent_gdp";
                case MacroIndicator.CashSurplusDeficitPercentGdp:
                    return "cash_surplus_deficit_percent_gdp";
                case MacroIndicator.TotalDebtServicePercentGni:
                    return "total_debt_service_percent_gni";
                case MacroIndicator.PopulationTotal:
                    return "population_total";
                case MacroIndicator.PopulationGrowthAnnual:
                    return "population_growth_annual";
                case MacroIndicator.NetMigration:
                    return "net_migration";
                case MacroIndicator.LifeExpectancy:
                    return "life_expectancy";
                case MacroIndicator.FertilityRate:
                    return "fertility_rate";
                case MacroIndicator.PrevalenceHivTotal:
                    return "prevalence_hiv_total";
                case MacroIndicator.UnemploymentTotalPercent:
                    return "unemployment_total_percent";
                case MacroIndicator.IncomeShareLowestTwenty:
                    return "income_share_lowest_twenty";
                case MacroIndicator.PovertyPovertyLinesPercentPopulation:
                    return "poverty_poverty_lines_percent_population";
                case MacroIndicator.MarketCapDomesticCompaniesPercentGdp:
                    return "market_cap_domestic_companies_percent_gdp";
                case MacroIndicator.MobileSubscriptionsPerHundred:
                    return "mobile_subscriptions_per_hundred";
                case MacroIndicator.InternetUsersPerHundred:
                    return "internet_users_per_hundred";
                case MacroIndicator.StartupProceduresRegister:
                    return "startup_procedures_register";
                case MacroIndicator.Co2EmissionsTonsPerCapita:
                    return "co2_emissions_tons_per_capita";
                case MacroIndicator.SurfaceAreaKm:
                    return "surface_area_km";
                default:
                    throw new ArgumentOutOfRangeException(nameof(indicator), indicator, "Unknown macro indicator.");
            }
        }
    }
}
