using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Playwright;
using static System.Net.Mime.MediaTypeNames;

namespace EquipmentStoresTests.Models.Pages.StorePages
{
    public class ProductDetailsPage : BasePage<HomePage>
    {
        private readonly string productName;
        public ProductDetailsPage(IPage page, string productName) : base(page)
        {
            this.productName = productName;
        }

        // Parent locator for the product section
        private ILocator ProductSection => page.Locator($".product-info-inner:has-text('{productName}')");

        // Locators derived from the parent locator
        private ILocator Breadcrumbs => ProductSection.Locator(".breadcrumbs");
        private ILocator BreadcrumbShopAll => Breadcrumbs.GetByRole(AriaRole.Link, new() { Name = "Shop all" });
        private ILocator BreadcrumbProduct => Breadcrumbs.Locator($"li.item.product:has-text('{productName}')");
        private ILocator PageTitle => ProductSection.Locator("h1.page-title span.base:has-text('{_productName}')");
        private ILocator ProductPrice => ProductSection.Locator(".price-box .price");
        private ILocator AddToCartButton => ProductSection.Locator("button.action.primary.buy-button.green.tocart");
        private ILocator QuantityInput => ProductSection.Locator("#qty");
        private ILocator ProductDescription => ProductSection.Locator(".product-description");
        private ILocator ProductSpecificationsLink => ProductSection.GetByRole(AriaRole.Link, new() { Name = "Weight and dimensions" });
        private ILocator ProductCareInformationLink => ProductSection.GetByRole(AriaRole.Link, new() { Name = "Care Information" });
        private ILocator ProductShippingAndReturnsLink => ProductSection.GetByRole(AriaRole.Link, new() { Name = "Shipping and returns" });

        // Action methods
        public async Task<ProductDetailsPage> ClickBreadcrumbShopAllAsync()
        {
            await BreadcrumbShopAll.ClickAsync();
            return this;
        }

        public async Task<ProductDetailsPage> ClickBreadcrumbProductAsync()
        {
            await BreadcrumbProduct.ClickAsync();
            return this;
        }

        public async Task<string> GetPageTitleAsync()
        {
            return await PageTitle.InnerTextAsync();
        }

        public async Task<string> GetProductPriceAsync()
        {
            return await ProductPrice.InnerTextAsync();
        }

        public async Task<ProductDetailsPage> SetQuantityAsync(int quantity)
        {
            await QuantityInput.FillAsync(quantity.ToString());
            return this;
        }

        public async Task<ProductDetailsPage> ClickAddToCartButtonAsync()
        {
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await AddToCartButton.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 12000 });
            await AddToCartButton.ClickAsync();
            return this;
        }

        public async Task<string> GetProductDescriptionAsync()
        {
            return await ProductDescription.InnerTextAsync();
        }

        public async Task<ProductDetailsPage> ClickProductSpecificationsLinkAsync()
        {
            await ProductSpecificationsLink.ClickAsync();
            return this;
        }

        public async Task<ProductDetailsPage> ClickProductCareInformationLinkAsync()
        {
            await ProductCareInformationLink.ClickAsync();
            return this;
        }

        public async Task<ProductDetailsPage> ClickProductShippingAndReturnsLinkAsync()
        {
            await ProductShippingAndReturnsLink.ClickAsync();
            return this;
        }
    }
}
