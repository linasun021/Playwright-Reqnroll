using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace EquipmentStoresTests.Models.Pages.StorePages
{
    public class CreatePage : BasePage<CreatePage>
    {
        public CreatePage(IPage page) : base(page) { }

        // Define locators using ILocator
        private ILocator FirstNameInput => page.Locator("#firstname");
        private ILocator LastNameInput => page.Locator("#lastname");
        private ILocator EmailInput => page.Locator("#email_address");
        private ILocator PasswordInput => page.Locator("#password");
        private ILocator ConfirmPasswordInput => page.Locator("#password-confirmation");
        private ILocator CreateAccountButton => page.Locator("button[title='Create an Account']");

        // Method to fill out the form
        public async Task FillFormAsync(string firstName, string lastName, string email, string password)
        {
            await FirstNameInput.FillAsync(firstName);
            await LastNameInput.FillAsync(lastName);
            await EmailInput.FillAsync(email);
            await PasswordInput.FillAsync(password);
            await ConfirmPasswordInput.FillAsync(password);
        }

        // Method to submit the form
        public async Task SubmitFormAsync()
        {
            await CreateAccountButton.ClickAsync();
        }
    }
}
