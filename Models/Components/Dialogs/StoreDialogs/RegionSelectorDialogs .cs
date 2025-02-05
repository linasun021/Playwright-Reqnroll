using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EquipmentStoresTests.Models.Pages.StorePages;
using Microsoft.Playwright;
using static System.Net.Mime.MediaTypeNames;

namespace EquipmentStoresTests.Models.Components.Dialogs.StoreDialogs
{
    public class RegionSelectorDialogs<T>
    {
        private readonly IPage page;
        private readonly T parent;

        public RegionSelectorDialogs(IPage page, T parent)
        {
            this.page = page;
            this.parent = parent;
        }
        private ILocator PopupContainer => page.Locator(".Popup__WisepopContent-sc-1vpebv6-3");
        private ILocator CloseButton => page.Locator("button.PopupCloseButton__InnerPopupCloseButton-sc-srj7me-0");

        public async Task<HomePage> Close()
        {
            await CloseButton.ClickAsync();
            return new HomePage(page);
        }
        public async Task<RegionSelectorDialogs<T>> WaitForPopupToAppear()
        {
            await PopupContainer.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });
            return this;
        }
    }
}
