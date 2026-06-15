namespace EODHD.CSharpApiClient
{
    /// <summary>
    /// Well-known string constants returned by the EODHD API — equity instrument types as reported in
    /// the <c>Type</c> field of exchange symbol and fundamentals responses.
    /// </summary>
    public static class EodhdConstants
    {
        /// <summary>
        /// Common stock.
        /// </summary>
        public const string EquityTypeCommonStock = "Common Stock";

        /// <summary>
        /// Exchange-traded fund.
        /// </summary>
        public const string EquityTypeETF = "ETF";

        /// <summary>
        /// Preferred stock.
        /// </summary>
        public const string EquityTypePreferredStock = "Preferred Stock";

        /// <summary>
        /// Subscription right.
        /// </summary>
        public const string EquityTypeRight = "Right";

        /// <summary>
        /// Rolling instrument.
        /// </summary>
        public const string EquityTypeRolling = "Rolling";

        /// <summary>
        /// Unit.
        /// </summary>
        public const string EquityTypeUnit = "Unit";

        /// <summary>
        /// Voting share class.
        /// </summary>
        public const string EquityTypeVoting = "Voting";

        /// <summary>
        /// Warrant.
        /// </summary>
        public const string EquityTypeWarrant = "Warrant";
    }
}
