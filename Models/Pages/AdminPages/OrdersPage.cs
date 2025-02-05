using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace EquipmentStoresTests.Models.Pages.AdminPages
{
    public class OrdersPage : BasePage<OrdersPage>
    {
        public OrdersPage(IPage page) : base(page) { }

        public ILocator CreateNewOrderButton => page.Locator("button[title='Create New Order']").First;
        private ILocator SearchInput => page.Locator("input.admin__control-text.data-grid-search-control[id='fulltext'][type='text']").First;

        // Locator for the search button using combined attributes
        private ILocator SearchButton => page.Locator("button.action-submit[type='button'][aria-label='Search']").First;

        public ILocator GetViewLinkByOrderNumber(string orderNumber) => page.Locator($"//tr[td/div[text()='{orderNumber}']]//a[contains(@class, 'action-menu-item') and text()='View']").First;

        private ILocator OrderStatus => page.Locator("//span[@id='order_status']");

        // Method to get the order status text
        public async Task<string> GetOrderStatusText()
        {
            return await OrderStatus.InnerTextAsync();
        }

        // Example action to click the "View" link
        public async Task ClickViewLinkByOrderNumber(string orderNumber)
        {
            var viewLink = GetViewLinkByOrderNumber(orderNumber);
            await viewLink.ClickAsync();
        }

        // Method to click the "Create New Order" button
        public async Task ClickCreateNewOrderAsync()
        {
            await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            await CreateNewOrderButton.WaitForAsync(new LocatorWaitForOptions { Timeout = 12000});
            await CreateNewOrderButton.ClickAsync();
        }
        private async Task EnterSearchOrder(string searchTerm)
        {
            await SearchInput.FillAsync(searchTerm);
        }

        private async Task ClickSearchButton()
        {
            await SearchButton.ClickAsync();
        }

        public async Task SearchForOrder(string orderNumber)
        {
            await EnterSearchOrder(orderNumber);
            await ClickSearchButton();
        }
    }
}
