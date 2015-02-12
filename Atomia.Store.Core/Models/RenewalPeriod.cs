using System;

namespace Atomia.Store.Core
{
    public sealed class RenewalPeriod
    {
        public const string MONTH = "MONTH";
        public const string YEAR = "YEAR";

        private readonly int period;
        private readonly string unit;

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

        public int Period
        {
            get
            {
                return period;
            }
        }

        public string Unit 
        {
            get
            {
                return unit.ToUpper();
            }
        }
    }
}
