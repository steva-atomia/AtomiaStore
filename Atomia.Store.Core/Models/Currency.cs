using System;

namespace Atomia.Store.Core
{
    /// <summary>
    /// Representation of a currency
    /// </summary>
    public sealed class Currency
    {
        private readonly string code;

        /// <summary>
        /// Currency constructor
        /// </summary>
        /// <param name="currencyCode">Three-letter code, <see cref="Code"/></param>
        public Currency(string currencyCode)
        {
            if (String.IsNullOrEmpty(currencyCode))
            {
                throw new ArgumentException("currencyCode");
            }

            this.code = currencyCode;
        }

        /// <summary>
        /// Three-letter ISO 4217 currency code.
        /// </summary>
        /// <example>SEK, USD, GBP, EUR</example>
        public string Code
        {
            get
            {
                return this.code;
            }
        }
    }
}
