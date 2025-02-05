using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Reqnroll.Infrastructure;

namespace EquipmentStoresTests.Models.Pages.AdminPages
{
    public abstract class BasePage<T> where T : BasePage<T>
    {
        protected readonly IPage page;
        protected readonly ReqnrollOutputHelper specFlowOutputHelper;

        protected BasePage(IPage page)
        {
            this.page = page;
        }

        // Define locators using the Locator method
        public ILocator SalesMenu => page.Locator("#menu-magento-sales-sales");
        public ILocator QuotesLink => page.Locator("#menu-magento-negotiablequote-quote-index");
        public ILocator OrdersLink => page.Locator("li[data-ui-id='menu-magento-sales-sales-order']");
        public ILocator InvoicesLink => page.Locator("li[data-ui-id='menu-magento-sales-sales-invoice']");
        public ILocator ShipmentsLink => page.Locator("[data-ui-id='menu-magento-sales-sales-shipment']");
        public ILocator CreditMemosLink => page.Locator("[data-ui-id='menu-magento-sales-sales-creditmemo']");
        public ILocator ReturnsLink => page.Locator("[data-ui-id='menu-magento-rma-sales-magento-rma-rma']");
        public ILocator BillingAgreementsLink => page.Locator("[data-ui-id='menu-magento-paypal-paypal-billing-agreement']");
        public ILocator TransactionsLink => page.Locator("[data-ui-id='menu-magento-sales-sales-transactions']");
        public ILocator BraintreeVirtualTerminalLink => page.Locator("[data-ui-id='menu-paypal-braintree-virtual-terminal']");

        // Methods to interact with the elements
        public async Task OpenSalesMenuAsync()
        {
            var emailSelector = "#menu-magento-sales-sales";
            await page.WaitForSelectorAsync(emailSelector, new PageWaitForSelectorOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 300000 // Timeout in milliseconds
            });
            await SalesMenu.ClickAsync();
        }

        public async Task NavigateToQuotesAsync()
        {
            await QuotesLink.ClickAsync();
        }

        public async Task NavigateToOrdersAsync()
        {
            await OrdersLink.ClickAsync();
        }

        public async Task NavigateToInvoicesAsync()
        {
            await InvoicesLink.ClickAsync();
        }

        public async Task NavigateToShipmentsAsync()
        {
            await ShipmentsLink.ClickAsync();
        }

        public async Task NavigateToCreditMemosAsync()
        {
            await CreditMemosLink.ClickAsync();
        }

        public async Task NavigateToReturnsAsync()
        {
            await ReturnsLink.ClickAsync();
        }

        public async Task NavigateToBillingAgreementsAsync()
        {
            await BillingAgreementsLink.ClickAsync();
        }

        public async Task NavigateToTransactionsAsync()
        {
            await TransactionsLink.ClickAsync();
        }

        public async Task NavigateToBraintreeVirtualTerminalAsync()
        {
            await BraintreeVirtualTerminalLink.ClickAsync();
        }
    }
}
