using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atomia.Store.Core.Test
{
    internal class FakeCartRepository: ICartProvider
    {
        public int SaveCartCount = 0;

        public Cart GetCart()
        {
            throw new NotImplementedException();
        }

        public void SaveCart(Cart cart)
        {
            SaveCartCount++;
        }
    }

    internal class FakeCarPricingProvider : ICartPricingService
    {
        public int CalculatePriceCount = 0;

        public Cart CalculatePricing(Cart cart)
        {
            this.CalculatePriceCount++;
            return cart;
        }
    }

    internal class FakeItemDisplayProvider : IItemDisplayProvider
    {
        public string GetName(Item item)
        {
            throw new NotImplementedException();
        }

        public string GetDescription(Item item)
        {
            throw new NotImplementedException();
        }
    }

    internal class FakeCurrencyProvider : ICurrencyProvider
    {
        public string GetCurrencyCode()
        {
 	        throw new NotImplementedException();
        }

        public string FormatAmount(decimal amount)
        {
 	        throw new NotImplementedException();
        }

        public void SetCurrencyCode(string currencyCode)
        {
            throw new NotImplementedException();
        }
    }

    [TestClass]
    public class CartTest
    {
        private FakeCartRepository cartRepository;
        private FakeCarPricingProvider cartPricingProvider;
        private FakeItemDisplayProvider itemDisplayProvider;
        private FakeCurrencyProvider currencyProvider;
        private Cart cart;

        [TestInitialize]
        public void Setup()
        {
            cartRepository = new FakeCartRepository(); 
            cartPricingProvider = new FakeCarPricingProvider();
            itemDisplayProvider = new FakeItemDisplayProvider();
            currencyProvider = new FakeCurrencyProvider();
            cart = new Cart(cartRepository, cartPricingProvider);
        }

        [TestMethod]
        public void SetPricingTest()
        {
            Assert.AreEqual(0m, cart.SubTotal, "Expected SubTotal to be 0 in new cart.");
            Assert.AreEqual(0m, cart.Tax, "Expected Tax to be 0 in new cart.");
            Assert.AreEqual(0m, cart.Total, "Expected Total to be 0 in new cart.");

            cart.SetPricing(1m, 1m, 1m);

            Assert.AreEqual(1m, cart.SubTotal, "Expected SubTotal to be 1.");
            Assert.AreEqual(1m, cart.Tax, "Expected Tax to be 1.");
            Assert.AreEqual(1m, cart.Total, "Expected Total to be 1.");

            Assert.AreEqual(0, cartRepository.SaveCartCount, "Did not expect cart to be saved.");
            Assert.AreEqual(0, cartPricingProvider.CalculatePriceCount, "Did not expect prices to be calculated.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "Did not expect to be able to add negative subTotal.")]
        public void SetPricingSubTotalNegativeFailsTest()
        {
            cart.SetPricing(-1m, 1m, 1m);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "Did not expect to be able to add negative tax.")]
        public void SetPricingTaxNegativeFailsTest()
        {
            cart.SetPricing(1m, -1m, 1m);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "Did not expect to be able to add negative total.")]
        public void SetPricingTotalNegativeFailsTest()
        {
            cart.SetPricing(1m, 1m, -1m);
        }

        [TestMethod]
        public void IsEmptyTest()
        {
            var cartItem = new CartItem("ART1", 1, itemDisplayProvider, currencyProvider);

            Assert.IsTrue(cart.IsEmpty(), "Expected cart to be empty.");
        }

        [TestMethod]
        public void AddItemTest()
        {
            var cartItem = new CartItem("ART1", 1, itemDisplayProvider, currencyProvider);

            Assert.AreEqual(0, cartItem.Id, "Did not expect Id to be set on new cart item.");

            cart.AddItem(cartItem);

            Assert.IsFalse(cart.IsEmpty(), "Did not expect cart to be empty.");
            Assert.AreEqual(1, cart.CartItems.Count, "Expected 1 item in cart.");
            Assert.AreEqual(1, cartItem.Id, "Expected cart item to have Id set.");

            Assert.AreEqual(1, cartRepository.SaveCartCount, "Expect cart to be saved once.");
            Assert.AreEqual(1, cartPricingProvider.CalculatePriceCount, "Expected prices to be calculated once.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Did not expect to be able to add null to cart items.")]
        public void AddItemNullTest()
        {
            cart.AddItem(null);
        }

        [TestMethod]
        public void RemoveItemTest()
        {
            var cartItem1 = new CartItem("ART1", 1, itemDisplayProvider, currencyProvider);
            var cartItem2 = new CartItem("ART2", 1, itemDisplayProvider, currencyProvider);

            cart.AddItem(cartItem1);
            cart.AddItem(cartItem2);

            Assert.AreEqual(2, cart.CartItems.Count, "Expected 2 items in cart.");
            Assert.IsTrue(cart.CartItems.Contains(cartItem1), "Expected item ART1 in cart.");
            Assert.IsTrue(cart.CartItems.Contains(cartItem2), "Expected item ART1 in cart.");

            cart.RemoveItem(cartItem1.Id);

            Assert.AreEqual(1, cart.CartItems.Count, "Expected 1 item in cart.");
            Assert.IsTrue(cart.CartItems.Contains(cartItem2), "Expected item ART1 in cart.");

            Assert.AreEqual(3, cartRepository.SaveCartCount, "Expect cart to be saved 3 times.");
            Assert.AreEqual(3, cartPricingProvider.CalculatePriceCount, "Expected prices to be calculated 3 times.");
        }

        [TestMethod]
        public void SetCampaignCodeTest()
        {
            Assert.AreEqual(String.Empty, cart.CampaignCode);

            cart.SetCampaignCode("FOO");
            Assert.AreEqual("FOO", cart.CampaignCode);

            cart.SetCampaignCode("BAR");
            Assert.AreEqual("BAR", cart.CampaignCode);

            Assert.AreEqual(2, cartRepository.SaveCartCount, "Expect cart to be saved twice.");
            Assert.AreEqual(2, cartPricingProvider.CalculatePriceCount, "Expected prices to be calculated twice.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Did not expected to be able to null as campaign code.")]
        public void SetCampaignCodeNullTest()
        {
            Assert.AreEqual(String.Empty, cart.CampaignCode);

            cart.SetCampaignCode(null);
        }

        [TestMethod]
        public void RemoveCampaignCodeTest()
        {
            Assert.AreEqual(String.Empty, cart.CampaignCode);

            cart.SetCampaignCode("FOO");
            Assert.AreEqual("FOO", cart.CampaignCode);

            cart.RemoveCampaignCode();
            Assert.AreEqual(string.Empty, cart.CampaignCode);

            Assert.AreEqual(2, cartRepository.SaveCartCount, "Expect cart to be saved twice.");
            Assert.AreEqual(2, cartPricingProvider.CalculatePriceCount, "Expected prices to be calculated twice.");
        }

        [TestMethod]
        public void ChangeQuantityTest()
        {
            var cartItem = new CartItem("ART1", 1, itemDisplayProvider, currencyProvider);

            cart.AddItem(cartItem);
            cart.ChangeQuantity(cartItem.Id, 2m);

            Assert.AreEqual(2, cartItem.Quantity);
            Assert.AreEqual(2, cartRepository.SaveCartCount, "Expect cart to be saved twice.");
            Assert.AreEqual(2, cartPricingProvider.CalculatePriceCount, "Expected prices to be calculated twice.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "Did not expect to be able to add negative quantity.")]
        public void ChangeQuantityNegativeFailsTest()
        {
            var cartItem = new CartItem("ART1", 1, itemDisplayProvider, currencyProvider);

            cart.AddItem(cartItem);
            cart.ChangeQuantity(cartItem.Id, -2m);
        }
    }
}
