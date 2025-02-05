using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace EquipmentStoresTests.Models.Pages.StorePages
{
    public class LoginPage : BasePage<LoginPage>
    {

        public LoginPage(IPage page) : base(page) { }

        private ILocator EmailInput => page.Locator("#email");
        private ILocator PasswordInput => page.Locator("#pass").First;
        private ILocator RememberMeCheckbox => page.Locator("#remember_menE5ZTRfdU8");
        private ILocator LoginButton => page.Locator("button#send2").First;
        private ILocator ForgotPasswordLink => page.Locator("a.action.remind");

        // Method to fill out the login form
        public async Task FillLoginFormAsync(string email, string password)
        {
            await EmailInput.FillAsync(email);
            await PasswordInput.FillAsync(password);
        }

        // Method to toggle the "Remember Me" checkbox
        public async Task ToggleRememberMeAsync(bool shouldCheck)
        {
            bool isChecked = await RememberMeCheckbox.IsCheckedAsync();
            if (isChecked != shouldCheck)
            {
                await RememberMeCheckbox.ClickAsync();
            }
        }

        // Method to submit the login form
        public async Task SubmitLoginFormAsync()
        {
            await LoginButton.ClickAsync();
        }

        // Method to click the "Forgot Password?" link
        public async Task ClickForgotPasswordAsync()
        {
            await ForgotPasswordLink.ClickAsync();
        }
    }
}
