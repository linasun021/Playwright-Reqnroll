using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace EquipmentStoresTests.Models.Pages.StorePages
{
    public class NewAddressPage : BasePage<NewAddressPage>
    {
        public NewAddressPage(IPage page) : base(page) { }
        private ILocator FirstNameInput => page.Locator("#firstname");
        private ILocator LastNameInput => page.Locator("#lastname");
        private ILocator StreetInput => page.Locator("#street_1");
        private ILocator CityInput => page.Locator("#city");
        private ILocator StateSelect => page.Locator("#region_id");
        private ILocator ZipInput => page.Locator("#zip");
        private ILocator CountrySelect => page.Locator("#country");
        private ILocator TelephoneInput => page.Locator("#telephone");
        private ILocator SaveAddressButton => page.Locator("button[data-action='save-address']");

        // Method to fill out the address form
        public async Task FillAddressFormAsync(string firstName, string lastName, string street, string city, string state, string zip, string country, string telephone)
        {
            await FirstNameInput.FillAsync(firstName);
            await LastNameInput.FillAsync(lastName);
            await StreetInput.FillAsync(street);
            await CityInput.FillAsync(city);
            await StateSelect.SelectOptionAsync(new SelectOptionValue { Label = state });
            await ZipInput.FillAsync(zip);
            await CountrySelect.SelectOptionAsync(new SelectOptionValue { Label = country });
            await TelephoneInput.FillAsync(telephone);
        }

        // Method to submit the address form
        public async Task SubmitAddressFormAsync()
        {
            await SaveAddressButton.ClickAsync();
        }
    }
}
