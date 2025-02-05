using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EquipmentStoresTests.Models.Data.DTO;
using Microsoft.Playwright;
using static System.Net.Mime.MediaTypeNames;

namespace EquipmentStoresTests.Models.Pages.StorePages
{
    public class ShippingPage : BasePage<ShippingPage>
    {
        public ShippingPage(IPage page) : base(page) { }

        public ILocator EmailInput => page.GetByLabel("Email address").First;
        public ILocator PasswordInput => page.GetByLabel("Password").First;
        public ILocator LoginButton => page.Locator("button.action.login.primary[data-action='checkout-method-login']");
        public ILocator FirstNameInput => page.GetByLabel("First name").First;
        public ILocator LastNameInput => page.GetByLabel("Last name").First;
        public ILocator StreetAddressInput => page.GetByLabel("Street address").First;
        public ILocator ApartmentInput => page.GetByLabel("Apartment, suite, etc. (optional)").First;
        public ILocator CityInput => page.GetByLabel("City").First;
        public ILocator ZipCodeInput => page.GetByLabel("Zip code").First;
        public ILocator StateSelect => page.GetByLabel("State").First;
        public ILocator CountrySelect => page.GetByLabel("Country").First;
        public ILocator PhoneInput => page.GetByLabel("Phone").First;
        public ILocator CompanyNameInput => page.GetByLabel("Company name (optional)").First;
        private ILocator ContinueToPaymentButton => page.Locator("button[data-role='opc-continue']");
        private ILocator PlaceOrderButton => page.Locator("button#place-order-trigger");

        private ILocator DiscountToggle => page.Locator("strong.action.action-toggle#block-discount-heading");
        private ILocator DiscountCodeInput => page.Locator("input#discount-code");
        private ILocator ApplyButton => page.Locator("button.action.primary.action-apply");
        private ILocator Subtotal => page.Locator(".totals.sub .price");
        private ILocator TaxValue => page.Locator("tr.totals-tax-summary td.amount .price");

        private ILocator GetDiscountAmount(string discountCode) => page.Locator($"tr.total-rules:has(.rule-name:text('{discountCode}'))").Locator(".rule-amount");
        private ILocator OrderTotal => page.Locator(".grand.totals .price");
        private ILocator FreeShippingInfoBlock => page.Locator("div.order-summary-info-block .title:has-text('FREE shipping')");


        public async Task<ShippingPage> ExpandDiscountSection()
        {
            await DiscountToggle.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });
            await DiscountToggle.ClickAsync();
            return this;
        }

        // Method to enter a discount code
        public async Task<ShippingPage> EnterDiscountCode(string code)
        {
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            // Ensure the input is visible and enabled
            await DiscountCodeInput.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });

            // Enter the discount code
            await DiscountCodeInput.FillAsync(code);
            return this;
        }

        // Method to click the "Apply" button
        public async Task<ShippingPage> ClickApplyButton()
        {
            // Ensure the button is visible and enabled
            await ApplyButton.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });

            // Click the button
            await ApplyButton.ClickAsync();
            return this;
        }

        public async Task<ShippingPage> ApplyDiscountAsync(string discountCode)
        {
            if (!await IsDiscountHasBeenApplied(discountCode))
            {
                await ExpandDiscountSection();
                await EnterDiscountCode(discountCode);
                await ApplyButton.ClickAsync();
            }
            return this;

        }

        public async Task<bool> IsDiscountHasBeenApplied(string discountCode)
        {
            try
            {
                return await GetDiscountAmount(discountCode).IsVisibleAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<string> GetDiscountAmountByName(string discountCode)
        {
            var discountItem = GetDiscountAmount(discountCode);
            await discountItem.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });
            return await discountItem.InnerTextAsync();
        }

        public async Task<string> GetSubtotalAsync()
        {
            await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded, new PageWaitForLoadStateOptions { Timeout = 60000});
            return await Subtotal.InnerTextAsync();
        }

        public async Task<string> GetOrderTotalAsync()
        {
            return await OrderTotal.InnerTextAsync();
        }
        public async Task<string> GetTaxValueAsync()
        {
            return await TaxValue.InnerTextAsync();
        }
        public async Task<bool> IsOFreeShippingInfoBlockVisibleAsync()
        {
            return await FreeShippingInfoBlock.IsVisibleAsync();
        }
        public async Task<ShippingPage> EnterEmailAsync(string email)
        {
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await EmailInput.FillAsync(email);
            return this;
        }

        public async Task<ShippingPage> EnterPasswordAsync(string password)
        {
            await PasswordInput.FillAsync(password);
            return this;
        }

        public async Task<ShippingPage> ClickLoginButton()
        {
            await LoginButton.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });
            await LoginButton.ClickAsync();
            return this;
        }

        public async Task<ShippingPage> EnterFirstNameAsync(string firstName)
        {
            await FirstNameInput.FillAsync(firstName);
            return this;
        }

        public async Task<ShippingPage> EnterLastNameAsync(string lastName)
        {
            await LastNameInput.FillAsync(lastName);
            return this;
        }

        public async Task<ShippingPage> EnterStreetAddressAsync(string streetAddress)
        {
            await StreetAddressInput.FillAsync(streetAddress);
            return this;
        }

        public async Task<ShippingPage> EnterApartmentAsync(string apartment)
        {
            await ApartmentInput.FillAsync(apartment);
            return this;
        }

        public async Task<ShippingPage> EnterCityAsync(string city)
        {
            await CityInput.FillAsync(city);
            return this;
        }

        public async Task<ShippingPage> EnterZipCodeAsync(string zipCode)
        {
            await ZipCodeInput.FillAsync(zipCode);
            return this;
        }

        public async Task<ShippingPage> SelectStateAsync(string state)
        {
            await StateSelect.SelectOptionAsync(new SelectOptionValue { Label = state });
            return this;
        }

        public async Task<ShippingPage> SelectCountryAsync(string country)
        {
            await CountrySelect.SelectOptionAsync(new SelectOptionValue { Label = country });
            return this;
        }

        public async Task<string> GetSelectCountryTextAsync()
        {
            return await CountrySelect.EvaluateAsync<string>("element => element.options[element.selectedIndex].text");
        }


        public async Task<ShippingPage> EnterPhoneAsync(string phone)
        {
            await PhoneInput.FillAsync(phone);
            return this;
        }

        public async Task<ShippingPage> EnterCompanyNameAsync(string companyName)
        {
            await CompanyNameInput.FillAsync(companyName);
            return this;
        }

        public async Task<ShippingPage> ClickContinueToPaymentAsync()
        {
            await ContinueToPaymentButton.ClickAsync();
            return this;
        }

        public async Task<ShippingPage> ClickPlaceOrderAsync()
        {
            await PlaceOrderButton.ClickAsync();
            return this;
        }

        public async Task<ShippingPage> EnterShippingAddress(CustomerDTO currentCustomer)
        {
            await EnterFirstNameAsync(currentCustomer.FirstName);
            await EnterLastNameAsync(currentCustomer.LastName);
            await EnterStreetAddressAsync(currentCustomer.Address.Street);
            await EnterCityAsync(currentCustomer.Address.City);
            await EnterZipCodeAsync(currentCustomer.Address.ZipCode);
            await SelectStateAsync(currentCustomer.Address.State);
            await SelectCountryAsync(currentCustomer.Address.Country);
            await EnterPhoneAsync(currentCustomer.Contact.Phone);
            return this;
        }

        public async Task<ShippingPage> ExistingCustomerLogin(CustomerDTO currentCustomer)
        {
            await EnterEmailAsync(currentCustomer.Contact.EmailAddress);
            await EnterPasswordAsync(currentCustomer.Contact.Password);
            await ClickLoginButton();
            return this;
        }
    }
}
