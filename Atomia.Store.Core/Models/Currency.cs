// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Currency.cs" company="Atomia AB">
//   Copyright (C) 2010 Atomia AB. All rights reserved
// </copyright>
// <summary>
//   Representation of a currency
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace Atomia.Store.Core
{
    /// <summary>
    /// Representation of a currency
    /// </summary>
    public sealed class Currency
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Currency"/> class.
        /// </summary>
        /// <param name="currencyCode">The currency code.</param>
        /// <exception cref="System.ArgumentException">currencyCode</exception>
        public Currency(string currencyCode)
        {
            if (string.IsNullOrEmpty(currencyCode))
            {
                throw new ArgumentException("currencyCode");
            }

            this.Code = currencyCode;
        }

        /// <summary>
        /// Three-letter ISO 4217 currency code.
        /// </summary>
        /// <example>SEK, USD, GBP, EUR</example>
        public string Code { get; }

        /// <summary>
        /// Gets the name of the currency.
        /// </summary>
        /// <value>
        /// The name of the currency.
        /// </value>
        public string Name { get; internal set; }

        /// <summary>
        /// Creates the currency.
        /// </summary>
        /// <param name="resourceProvider">The resource provider.</param>
        /// <param name="currencyCode">The currency code.</param>
        /// <returns></returns>
        public static Currency CreateCurrency(IResourceProvider resourceProvider, string currencyCode)
        {
            var name = resourceProvider.GetResource(currencyCode.ToUpper());

            if (string.IsNullOrEmpty(name))
            {
                name = currencyCode;
            }

            return new Currency(currencyCode)
            {
                Name = name
            };
        }
    }
}
