using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Reqnroll.Infrastructure;
using static System.Net.Mime.MediaTypeNames;

namespace EquipmentStoresTests.Models.Pages.StorePages
{
    public abstract class BasePage<T> where T : BasePage<T>
    {
        protected readonly IPage page;
        protected readonly ReqnrollOutputHelper specFlowOutputHelper;

        protected BasePage(IPage page)
        {
            this.page = page;
        }


        public ILocator searchToggle => page.Locator("span.search-toggle");
        public ILocator SearchInput => page.Locator("#search");
        public ILocator SearchButton => page.Locator("button[type='submit']");
        private ILocator BackButton => page.GetByText("Back");
        private ILocator EquipmentLink => page.Locator("span.level-top:has-text('Equipment')");
        private ILocator GetLinkByName(string name) => page.Locator($"div.sub-level a:text('{name}')");

        private ILocator MenuTrigger => page.Locator(".customer-actions-trigger");
        private ILocator CreateAccountLink => page.Locator("a:has-text('Create Account')");
        private ILocator LoginLink => page.Locator("a:has-text('Log In')");


        // Method to expand the customer menu
        private async Task ExpandMenuAsync()
        {
            await MenuTrigger.ClickAsync();
        }

        // Method to click the "Create Account" link
        public async Task ClickCreateAccountAsync()
        {
            await ExpandMenuAsync();
            await CreateAccountLink.ClickAsync();
        }

        // Method to click the "Log In" link
        public async Task ClickLoginAsync()
        {
            await ExpandMenuAsync();
            await LoginLink.ClickAsync();
        }

        // Action methods
        public async Task<BasePage<T>> ClickBackButtonAsync()
        {
            await BackButton.ClickAsync();
            return this;
        }

        public async Task<BasePage<T>> ClickEquipmentLinkAsync()
        {
            await EquipmentLink.ClickAsync();
            return this;
        }

        public async Task<EquipmentPage> ClickEquipmentSetLinkAsync(string name)
        {
            await ClickEquipmentLinkAsync();
            await GetLinkByName(name).ClickAsync();
            return new EquipmentPage(page);
        }

        public BasePage<T> ClicksearchToggle()
        {
            searchToggle.ClickAsync().Wait();
            return this;
        }

        public async Task<BasePage<T>> HoverSearchToggle()
        {
            await searchToggle.HoverAsync();
            return this;
        }
        public BasePage<T> EnterSearchQuery(string query)
        {
            SearchInput.FillAsync(query).Wait();
            return this;
        }
    }
}

