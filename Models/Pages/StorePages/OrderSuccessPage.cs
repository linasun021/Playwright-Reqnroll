using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Playwright;
using static System.Net.Mime.MediaTypeNames;

namespace EquipmentStoresTests.Models.Pages.StorePages
{
    public class OrderSuccessPage : BasePage<OrderSuccessPage>
    {
        public OrderSuccessPage(IPage page) : base(page) { }
        public ILocator SuccessMessage => page.Locator(".success-info h1");
        public ILocator OrderNumber => page.Locator(".success-info p:has-text('Your order number is:')");
        public ILocator Email => page.Locator(".success-info p:has-text('Your email:')");
        public ILocator CreateAccountLink => page.Locator("a:has-text('Create your account here')");

        // Methods
        public async Task<string> GetSuccessMessageAsync()
        {
            await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            return await SuccessMessage.InnerTextAsync();
        }

        public async Task<string> GetOrderNumberAsync()
        {
            return await OrderNumber.InnerTextAsync();
        }

        public async Task<bool> CheckOrderNumberVisibleAsync()
        {
            return await OrderNumber.IsVisibleAsync();
        }
        public async Task<string> GetEmailAsync()
        {
            return await Email.InnerTextAsync();
        }

        public async Task<OrderSuccessPage> ClickCreateAccountLinkAsync()
        {
            await CreateAccountLink.ClickAsync();
            return this;
        }

        public async Task<bool> CheckCreateAccountLinkVisibleAsync()
        {
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            return await CreateAccountLink.IsVisibleAsync();
        }
    }
}
