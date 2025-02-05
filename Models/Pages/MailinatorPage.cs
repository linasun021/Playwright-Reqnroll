using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Playwright;
using NUnit.Framework;

namespace EquipmentStoresTests.Models.Pages
{
    public class MailinatorPage
    {
        private IPage page;
        public  MailinatorPage(IPage page)
        {
            this.page = page;
        }
        public ILocator EmailInput => page.Locator("input#inbox_field");
        public ILocator GoButton => page.Locator("button.primary-btn");
        public ILocator ConfirmAccountButton => page.FrameLocator("#html_msg_body").Locator("a:has-text('Confirm Your Account')");
        public ILocator TotalPrice => page.FrameLocator("#html_msg_body").Locator("tfoot.order-totals tr.grand_total td strong span.price");
        public ILocator ActivateLink => page.FrameLocator("#html_msg_body").Locator("a:has-text('Activate now')");

        public ILocator BackToInboxLink => page.Locator("a:has-text('Back to Inbox')");

        public ILocator TracingNumberLink => page.FrameLocator("#html_msg_body").Locator("td p.tracking-info a.tracking-links");

        public async Task<string> GetTrackingNumberAsync()
        {
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            return await TracingNumberLink.InnerTextAsync();
        }
        public async Task EnterEmailAddress(string emailAddress)
        {
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await EmailInput.FillAsync(emailAddress);
        }

        public async Task ClickGoButton()
        {
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await GoButton.ClickAsync();
        }

        public async Task OpenInbox(string emailAddress)
        {
            await EnterEmailAddress(emailAddress);
            await ClickGoButton();
        }

        public async Task<bool> isOrderConfirmEmailVisible(string orderNumber)
        {
            return await (await GetEmailItem(orderNumber)).IsVisibleAsync();
        }

        public async Task OpenEmail(string emailTitle)
        {
            var emailSelector = $"//tr[td[normalize-space(text()) = '{emailTitle}']]";
            var emailItem = page.Locator(emailSelector);
            var emailElement = await page.WaitForSelectorAsync(emailSelector, new PageWaitForSelectorOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 300000 // Timeout in milliseconds
            });
            await emailItem.ClickAsync();
        }

        public async Task ClickConfirmYourAccountButton()
        {
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await ConfirmAccountButton.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 12000 });
            await ConfirmAccountButton.ClickAsync();
        }

        public async Task ClickActiveLinkAsync()
        {
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await ActivateLink.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 12000 });
            await ActivateLink.ClickAsync();
        }

        private async Task<ILocator> GetEmailItem(string orderNumber)
        {
            // Look for an email with a specific subject or content
            var emailSubject = $"Les Mills Equipment : New order #{orderNumber} - UAT";
            var emailSelector = $"//tr[contains(., '{emailSubject}')]";
            var emailItem = page.Locator(emailSelector);
            var emailElement = await page.WaitForSelectorAsync(emailSelector, new PageWaitForSelectorOptions
            {
                Timeout = 60000 // Timeout in milliseconds
            });
            return emailItem;
        }

        public async Task<string> GetTotalPriceAsync()
        {
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            return await TotalPrice.InnerTextAsync();
        }


        public async Task<string> GetOrderContent(string orderNumber)
        {
            await (await GetEmailItem(orderNumber)).ClickAsync();
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await page.WaitForSelectorAsync("div#msgpane");
            var emailContent = await page.InnerTextAsync("div#msgpane");
            return emailContent;
        }

        public async Task ClickBackToInbox()
        {
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await BackToInboxLink.ClickAsync();
        }

    }
}
