using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EquipmentStoresTests.Models.Pages.AdminPages;
using Microsoft.Playwright;

namespace EquipmentStoresTests.Models.Components.Dialogs.AdminDialogs
{
    public class AdminLoginDialogs<T>
    {
        private readonly IPage page;
        private readonly T parent;

        public AdminLoginDialogs(IPage page, T parent)
        {
            this.page = page;
            this.parent = parent;
        }

        public ILocator UsernameInput => page.Locator("#username");
        public ILocator PasswordInput => page.Locator("#login");
        public ILocator SignInButton => page.Locator(".action-login");

        public ILocator AuthenticatorCodeInput => page.Locator("#tfa_code");
        public ILocator ConfirmButton => page.Locator(".action-login");

        // Method to enter the authenticator code
        public async Task ConfirmAuthenticatorCodeAsync(string code)
        {
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await AuthenticatorCodeInput.FillAsync(code);
            await ConfirmButton.ClickAsync();
        }

        public async Task LoginAsync(string username, string password)
        {
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await UsernameInput.FillAsync(username);
            await PasswordInput.FillAsync(password);
            await SignInButton.ClickAsync();
        }
    }
}
