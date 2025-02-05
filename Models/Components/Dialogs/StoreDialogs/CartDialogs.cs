using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using EquipmentStoresTests.Models.Pages.StorePages;
using Microsoft.Playwright;
using static System.Net.Mime.MediaTypeNames;

namespace EquipmentStoresTests.Models.Components.Dialogs.StoreDialogs
{
    public class CartDialogs<T>
    {
        private readonly IPage page;
        private readonly T parent;

        public CartDialogs(IPage page, T parent)
        {
            this.page = page;
            this.parent = parent;
        }

        // Locator for items by title
        public ILocator GetItemByTitle(string title) => page.Locator($"//a[@title='{title}']/ancestor::li[contains(@class, 'item')]");
        private ILocator GetFreeSubscriptionByName(string name) => page.Locator($".product-item-name:has-text('{name}')");
        private ILocator GetQuantityInputByTitle(string title) => page.Locator($"//a[@title='{title}']/ancestor::li//input[contains(@class, 'item-qty')]");
        private ILocator GetPriceByTitle(string title) => page.Locator($"//a[@title='{title}']/ancestor::li//span[@class='price']");

        public ILocator GetRemoveButtonByName(string name) => page.Locator($"//a[@title='{name}']/ancestor::li[contains(@class, 'item')]//a[@title='Remove item']");
        public ILocator GetUpdateButtonByName(string name) => page.Locator($"//a[@title='{name}']/ancestor::li[contains(@class, 'item')]//button[@title='Update']");
        public ILocator DiscountCodeInput => page.Locator("#minicart-coupon-code");
        public ILocator ApplyDiscountButton => page.Locator("button.action.apply.primary");
        public ILocator SubtotalAmount => page.Locator(".subtotal .price-wrapper .price");
        public ILocator CheckoutButton => page.Locator("#top-cart-btn-checkout");



        public async Task<bool> CheckProductVisibleInCart(string equipmentName)
        {
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle,new PageWaitForLoadStateOptions { Timeout = 60000});
            ILocator productItem = GetItemByTitle(equipmentName);
            await productItem.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 12000 });
            return await productItem.IsVisibleAsync();
        }

        public async Task<bool> CheckFreeSubscriptionVisibleInCart(string name)
        {
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            ILocator productItem = GetFreeSubscriptionByName(name);
            await productItem.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 12000 });
            return await productItem.IsVisibleAsync();
        }

        // Method to update the quantity of an item by title
        public async Task UpdateQuantityByTitle(string title, int quantity)
        {
            var quantityInput = GetQuantityInputByTitle(title);
            await quantityInput.FillAsync(quantity.ToString());
            var updateButton = quantityInput.Locator("following-sibling::button[contains(@class, 'update-cart-item')]");
            await updateButton.ClickAsync();
        }

        // Method to apply a discount code
        public async Task ApplyDiscountCode(string discountCode)
        {
            await DiscountCodeInput.FillAsync(discountCode);
            await ApplyDiscountButton.ClickAsync();
        }

        public async Task<string> GetItemPriceByName(string equipmentName)
        {
            return await GetPriceByTitle(equipmentName).InnerTextAsync();
        }

        public async Task<int> GetItemQuantityByName(string equipmentName)
        {
            return int.Parse(await GetQuantityInputByTitle(equipmentName).InputValueAsync());
        }

        public async Task<string> GetSubtotal()
        {
            return await SubtotalAmount.InnerTextAsync();
        }

        // Method to click the checkout button
        public async Task<ShippingPage> ClickCheckoutButton()
        {
            await CheckoutButton.ClickAsync();
            return new ShippingPage(page);
        }
    }
}
