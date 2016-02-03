using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;
using Atomia.Web.Plugin.ProductsProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Atomia.Store.PublicBillingApi.Test
{
    [TestClass]
    public class PriceCalculatorTest
    {
        [TestMethod]
        public void TestPriceExcludingVat()
        {
            var calculator = new PriceCalculator(false, false);
            var priceExcludingTax = 100m;
            var taxes = new List<PublicOrderTax> 
            {
                new PublicOrderTax(){Name = "Tax1", Percent = 25, ApplyToAmountOnly = true }
            };

            var result = calculator.CalculatePrice(priceExcludingTax, taxes);

            Assert.AreEqual(100, result);
        }

        [TestMethod]
        public void TestPriceExcludingVatCumulative()
        {
            var calculator = new PriceCalculator(false, false);
            var priceExcludingTax = 100m;
            var taxes = new List<PublicOrderTax> 
            {
                new PublicOrderTax(){Name = "Tax1", Percent = 25, ApplyToAmountOnly = false },
                new PublicOrderTax(){Name = "Tax2", Percent = 25, ApplyToAmountOnly = false }
            };

            var result = calculator.CalculatePrice(priceExcludingTax, taxes);

            Assert.AreEqual(100m, result);
        }

        [TestMethod]
        public void TestPriceIncludingVat()
        {
            var calculator = new PriceCalculator(true, false);
            var priceExcludingTax = 100;
            var taxes = new List<PublicOrderTax> 
            {
                new PublicOrderTax(){Name = "Tax1", Percent = 25, ApplyToAmountOnly = true }
            };

            var result = calculator.CalculatePrice(priceExcludingTax, taxes);

            Assert.AreEqual(125, result);
        }

        [TestMethod]
        public void TestPriceIncludingVatMultipleTaxes()
        {
            var calculator = new PriceCalculator(true, false);
            var priceExcludingTax = 100m;
            var taxes = new List<PublicOrderTax> 
            {
                new PublicOrderTax(){Name = "Tax1", Percent = 25, ApplyToAmountOnly = true },
                new PublicOrderTax(){Name = "Tax2", Percent = 25, ApplyToAmountOnly = true }
            };

            var result = calculator.CalculatePrice(priceExcludingTax, taxes);

            Assert.AreEqual(150m, result);
        }

        [TestMethod]
        public void TestPriceIncludingVatCumulativeTaxes()
        {
            var calculator = new PriceCalculator(true, false);
            var priceExcludingTax = 100m;
            var taxes = new List<PublicOrderTax> 
            {
                new PublicOrderTax(){Name = "Tax1", Percent = 25, ApplyToAmountOnly = false },
                new PublicOrderTax(){Name = "Tax2", Percent = 25, ApplyToAmountOnly = false }
            };

            var result = calculator.CalculatePrice(priceExcludingTax, taxes);

            Assert.AreEqual(156.25m, result);
        }

        [TestMethod]
        public void TestPriceIncludingVatWithProductTaxes()
        {
            var calculator = new PriceCalculator(true, false);
            var priceExcludingTax = 100;
            var taxes = new List<ProductTax> 
            {
                new ProductTax(new PublicOrderTax(){Name = "Tax1", Percent = 25, ApplyToAmountOnly = true })
            };

            var result = calculator.CalculatePrice(priceExcludingTax, taxes);

            Assert.AreEqual(125, result);
        }

        [TestMethod]
        public void TestPriceIncludingVatNoTaxes()
        {
            var calculator = new PriceCalculator(true, false);
            var priceExcludingTax = 100;
            var taxes = new List<PublicOrderTax> {};

            var result = calculator.CalculatePrice(priceExcludingTax, taxes);

            Assert.AreEqual(100, result);
        }

        [TestMethod]
        public void TestPriceIncludingVatNullPublicOrderTaxes()
        {
            var calculator = new PriceCalculator(true, false);
            var priceExcludingTax = 100;
            List<PublicOrderTax> taxes = null;

            var result = calculator.CalculatePrice(priceExcludingTax, taxes);

            Assert.AreEqual(100, result);
        }

        [TestMethod]
        public void TestPriceIncludingVatNullProductTaxes()
        {
            var calculator = new PriceCalculator(true, false);
            var priceExcludingTax = 100;
            List<ProductTax> taxes = null;

            var result = calculator.CalculatePrice(priceExcludingTax, taxes);

            Assert.AreEqual(100, result);
        }

        [TestMethod]
        public void TestPriceIncludingVatAndResellerHasInclusiveTaxCalculation()
        {
            var calculator = new PriceCalculator(true, true);
            var priceExcludingTax = 100;
            var taxes = new List<PublicOrderTax>
            {
                new PublicOrderTax(){Name = "Tax1", Percent = 25, ApplyToAmountOnly = true }
            };

            var result = calculator.CalculatePrice(priceExcludingTax, taxes);

            Assert.AreEqual(100, result);
        }
    }
}
