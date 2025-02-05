using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace EquipmentStoresTests.Models.Pages.StorePages
{
    public class AccountPage : BasePage<AccountPage>
    {
        public AccountPage(IPage page) : base(page) { }

        private ILocator EditAddressButton => page.Locator("a[data-ui-id='default-shipping-edit-link']");

        public async Task ClickEditAddressButtonAsync()
        {
            await EditAddressButton.ClickAsync();
        }
    }
}
