using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace EquipmentStoresTests.Models.Components.Dialogs.StoreDialogs
{
    public class FreeGiftDialogs<T>
    {
        private readonly IPage page;
        private readonly T parent;

        public FreeGiftDialogs(IPage page, T parent)
        {
            this.page = page;
            this.parent = parent;
        }
        private ILocator CloseButton => page.Locator("div.ampromo-close[data-role='ampromo-popup-hide']");

        public async Task ClosePromoPopup()
        {

            // Wait for the button to be visible and enabled
            await CloseButton.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });

            // Click the button
            await CloseButton.ClickAsync();
        }
    }
}
