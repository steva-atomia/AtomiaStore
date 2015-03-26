using System;

namespace Atomia.Store.Core
{
    /// <summary>
    /// Renewal period of a <see cref="Product"/>
    /// </summary>
    public sealed class RenewalPeriod
    {
        public const string MONTH = "MONTH";
        public const string YEAR = "YEAR";

        private readonly int period;
        private readonly string unit;

        /// <summary>
        /// Renewal period constructor
        /// </summary>
        /// <param name="period">Length of the renewal period</param>
        /// <param name="unit">Unit of the renewal period: <see cref="MONTH" or <see cref="YEAR"/>/></param>
        public RenewalPeriod(int period, string unit)
        {
            if (period <= 0)
            {
                throw new ArgumentException("period must be greater than 0");
            }

            if (!(unit.ToUpper() == YEAR || unit.ToUpper() == MONTH))
            {
                throw new ArgumentException("unit should be YEAR or MONTH (case-insensitive)");
            }

            this.period = period;
            this.unit = unit;
        }

        /// <summary>
        /// Length of the renewal period
        /// </summary>
        public int Period
        {
            get
            {
                return period;
            }
        }

        /// <summary>
        /// <see cref="MONTH"/> or <see cref="YEAR"/>
        /// </summary>
        public string Unit 
        {
            get
            {
                return unit.ToUpper();
            }
        }
    }
}
