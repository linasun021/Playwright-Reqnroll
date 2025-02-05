using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Reqnroll.Infrastructure;
using static System.Net.Mime.MediaTypeNames;

namespace EquipmentStoresTests.Models.Pages.StorePages
{
    public class EquipmentPage : BasePage<EquipmentPage>
    {
        public EquipmentPage(IPage page) : base(page) { }

        protected ILocator GetProductItem(string productName) => page.Locator($".product-item:has-text('{productName}')");
        protected ILocator GetAddToCartButton(string title) => page.Locator($"//a[@title='{title}']/ancestor::div[contains(@class, 'product-item')]//button[@title='Add to cart']");
        protected ILocator GetHoverImage(string productName) => page.Locator($".hover-image img[alt='{productName}']");
        protected ILocator GetProductPrice(string productName) => GetProductItem(productName).Locator(".price-wrapper .price");

        public async Task ClickAddToCartButton(string title)
        {
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            var addToCartButton = GetAddToCartButton(title).First;
            await addToCartButton.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible, // Wait until the element is visible
                Timeout = 5000, // Timeout in milliseconds (optional)
            });
            // Click the "Add to cart" button
            await addToCartButton.ClickAsync();
        }


        // Product item action methods
        public async Task<ProductDetailsPage> ClickProductImageAsync(string productName)
        {
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await GetHoverImage(productName).ClickAsync(new LocatorClickOptions() { Timeout = 12000 });
            return new ProductDetailsPage(page, productName);
        }

        public async Task<string> GetProductPriceAsync(string productName)
        {
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            return await GetProductPrice(productName).First.InnerTextAsync();
        }

    }
}
