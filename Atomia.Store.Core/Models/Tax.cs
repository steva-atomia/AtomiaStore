using System;

namespace Atomia.Store.Core
{
    public class Tax
    {
        public Tax(string name, decimal amount, decimal percentage)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException("amount", "amount cannot be less than 0.");
            }

            if (percentage < 0)
            {
                throw new ArgumentOutOfRangeException("percentage", "percentage cannot be less than 0.");
            }

            this.Name = name;
            this.Amount = amount;
            this.Percentage = percentage;
        }

        public string Name { get; private set; }

        public decimal Amount { get; private set; }

        public decimal Percentage { get; private set; }
    }
}
