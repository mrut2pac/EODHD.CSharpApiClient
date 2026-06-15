namespace EODHD.CSharpApiClient.DataModel.Macro
{
    /// <summary>
    /// A macroeconomic indicator requested from the EODHD macro-indicators endpoint. Each value maps to
    /// the API's <c>indicator</c> query parameter.
    /// </summary>
    public enum MacroIndicator
    {
        /// <summary>
        /// Gross domestic product, current US dollars (<c>gdp_current_usd</c>). The API default.
        /// </summary>
        GdpCurrentUsd,

        /// <summary>
        /// GDP per capita, current US dollars (<c>gdp_per_capita_usd</c>).
        /// </summary>
        GdpPerCapitaUsd,

        /// <summary>
        /// GDP growth, annual percent (<c>gdp_growth_annual</c>).
        /// </summary>
        GdpGrowthAnnual,

        /// <summary>
        /// Gross national income, current US dollars (<c>gni_usd</c>).
        /// </summary>
        GniUsd,

        /// <summary>
        /// GNI per capita, current US dollars (<c>gni_per_capita_usd</c>).
        /// </summary>
        GniPerCapitaUsd,

        /// <summary>
        /// GNI, purchasing-power-parity US dollars (<c>gni_ppp_usd</c>).
        /// </summary>
        GniPppUsd,

        /// <summary>
        /// GNI per capita, purchasing-power-parity US dollars (<c>gni_per_capita_ppp_usd</c>).
        /// </summary>
        GniPerCapitaPppUsd,

        /// <summary>
        /// Gross capital formation, percent of GDP (<c>gross_capital_formation_percent_gdp</c>).
        /// </summary>
        GrossCapitalFormationPercentGdp,

        /// <summary>
        /// Agriculture value added, percent of GDP (<c>agriculture_value_added_percent_gdp</c>).
        /// </summary>
        AgricultureValueAddedPercentGdp,

        /// <summary>
        /// Industry value added, percent of GDP (<c>industry_value_added_percent_gdp</c>).
        /// </summary>
        IndustryValueAddedPercentGdp,

        /// <summary>
        /// Services value added, percent of GDP (<c>services_value_added_percent_gdp</c>).
        /// </summary>
        ServicesValueAddedPercentGdp,

        /// <summary>
        /// Inflation, consumer prices, annual percent (<c>inflation_consumer_prices_annual</c>).
        /// </summary>
        InflationConsumerPricesAnnual,

        /// <summary>
        /// Consumer price index (<c>consumer_price_index</c>).
        /// </summary>
        ConsumerPriceIndex,

        /// <summary>
        /// Inflation, GDP deflator, annual percent (<c>inflation_gdp_deflator_annual</c>).
        /// </summary>
        InflationGdpDeflatorAnnual,

        /// <summary>
        /// Real interest rate, percent (<c>real_interest_rate</c>).
        /// </summary>
        RealInterestRate,

        /// <summary>
        /// Net trades in goods and services (<c>net_trades_goods_services</c>).
        /// </summary>
        NetTradesGoodsServices,

        /// <summary>
        /// Exports of goods and services, percent of GDP (<c>exports_of_goods_services_percent_gdp</c>).
        /// </summary>
        ExportsOfGoodsServicesPercentGdp,

        /// <summary>
        /// Imports of goods and services, percent of GDP (<c>imports_of_goods_services_percent_gdp</c>).
        /// </summary>
        ImportsOfGoodsServicesPercentGdp,

        /// <summary>
        /// Merchandise trade, percent of GDP (<c>merchandise_trade_percent_gdp</c>).
        /// </summary>
        MerchandiseTradePercentGdp,

        /// <summary>
        /// High-technology exports, percent of total exports (<c>high_technology_exports_percent_total</c>).
        /// </summary>
        HighTechnologyExportsPercentTotal,

        /// <summary>
        /// Central government debt, percent of GDP (<c>debt_percent_gdp</c>).
        /// </summary>
        DebtPercentGdp,

        /// <summary>
        /// Revenue excluding grants, percent of GDP (<c>revenue_excluding_grants_percent_gdp</c>).
        /// </summary>
        RevenueExcludingGrantsPercentGdp,

        /// <summary>
        /// Cash surplus/deficit, percent of GDP (<c>cash_surplus_deficit_percent_gdp</c>).
        /// </summary>
        CashSurplusDeficitPercentGdp,

        /// <summary>
        /// Total debt service, percent of GNI (<c>total_debt_service_percent_gni</c>).
        /// </summary>
        TotalDebtServicePercentGni,

        /// <summary>
        /// Total population (<c>population_total</c>).
        /// </summary>
        PopulationTotal,

        /// <summary>
        /// Population growth, annual percent (<c>population_growth_annual</c>).
        /// </summary>
        PopulationGrowthAnnual,

        /// <summary>
        /// Net migration (<c>net_migration</c>).
        /// </summary>
        NetMigration,

        /// <summary>
        /// Life expectancy at birth, years (<c>life_expectancy</c>).
        /// </summary>
        LifeExpectancy,

        /// <summary>
        /// Fertility rate, births per woman (<c>fertility_rate</c>).
        /// </summary>
        FertilityRate,

        /// <summary>
        /// Prevalence of HIV, total (<c>prevalence_hiv_total</c>).
        /// </summary>
        PrevalenceHivTotal,

        /// <summary>
        /// Unemployment, total percent of labour force (<c>unemployment_total_percent</c>).
        /// </summary>
        UnemploymentTotalPercent,

        /// <summary>
        /// Income share held by the lowest 20 percent (<c>income_share_lowest_twenty</c>).
        /// </summary>
        IncomeShareLowestTwenty,

        /// <summary>
        /// Poverty headcount at the poverty lines, percent of population (<c>poverty_poverty_lines_percent_population</c>).
        /// </summary>
        PovertyPovertyLinesPercentPopulation,

        /// <summary>
        /// Market capitalisation of domestic companies, percent of GDP (<c>market_cap_domestic_companies_percent_gdp</c>).
        /// </summary>
        MarketCapDomesticCompaniesPercentGdp,

        /// <summary>
        /// Mobile cellular subscriptions per 100 people (<c>mobile_subscriptions_per_hundred</c>).
        /// </summary>
        MobileSubscriptionsPerHundred,

        /// <summary>
        /// Individuals using the internet per 100 people (<c>internet_users_per_hundred</c>).
        /// </summary>
        InternetUsersPerHundred,

        /// <summary>
        /// Start-up procedures to register a business (<c>startup_procedures_register</c>).
        /// </summary>
        StartupProceduresRegister,

        /// <summary>
        /// CO2 emissions, metric tons per capita (<c>co2_emissions_tons_per_capita</c>).
        /// </summary>
        Co2EmissionsTonsPerCapita,

        /// <summary>
        /// Surface area, square kilometres (<c>surface_area_km</c>).
        /// </summary>
        SurfaceAreaKm,
    }
}
