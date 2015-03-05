using System;

namespace Atomia.Store.Core
{
    public class Currency
    {
        private readonly string code;

        public Currency(string currencyCode)
        {
            if (String.IsNullOrEmpty(currencyCode))
            {
                throw new ArgumentException("currencyCode");
            }

            this.code = currencyCode;
        }

        public string Code
        {
            get
            {
                return this.code;
            }
        }
    }
}
