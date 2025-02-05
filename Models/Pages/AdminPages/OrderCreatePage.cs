using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace EquipmentStoresTests.Models.Pages.AdminPages
{
    public class OrderCreatePage : BasePage<OrderCreatePage>
    {
        public OrderCreatePage(IPage page) : base(page) { }

        public ILocator EmailInput => page.Locator("#sales_order_create_customer_grid_filter_email");
        public ILocator SearchCustomerButton => page.Locator("button[title='Search'][type='button'][data-action='grid-filter-apply']").First;
        public ILocator EmailCell(string emailAddress) => page.Locator($"td:has-text('{emailAddress}')").First;
        public ILocator StoreLabel(string storeName) => page.Locator($"label:has-text('{storeName}')");

        public ILocator GetShippingMethodsLink => page.Locator("#order-shipping-method-summary a.action-default");

        public ILocator AddProductsButton => page.Locator("#add_products");

        public ILocator ProductNameInput => page.Locator("#sales_order_create_search_grid_filter_name");

        public ILocator SearchProductButton => page.Locator("button[title='Search'][type='button'][data-action='grid-filter-apply']").Last;

        public ILocator ProductCheckbox => page.Locator("input.checkbox.admin__control-checkbox").First;

        public ILocator AddSelectedProductsButton => page.Locator("button[title='Add Selected Product(s) to Order']");
        public ILocator FreeShippingLabel => page.Locator("label[for='s_method_freeshippingadmin_freeshippingadmin']");

        public ILocator CreditCardLabel => page.Locator("label[for='p_method_braintree']");

        public ILocator CardNumberInput => page.FrameLocator("iframe[name='braintree-hosted-field-number']").Locator("input[data-braintree-name='number']");
        public ILocator ExpMonthInput => page.FrameLocator("iframe[name='braintree-hosted-field-expirationMonth']").Locator("input[data-braintree-name='expirationMonth']");
        public ILocator ExpYearInput => page.FrameLocator("iframe[name='braintree-hosted-field-expirationYear']").Locator("input[data-braintree-name='expirationYear']");
        public ILocator CVVInput => page.FrameLocator("iframe[name='braintree-hosted-field-cvv']").Locator("input[data-braintree-name='cvv']");

        public ILocator SubmitOrderButton => page.Locator("#submit_order_top_button").First;

        public ILocator OrderNumberContainer => page.Locator("div.admin__page-section-item-title .title").First;

        public ILocator SuccessMessage => page.Locator("div[data-ui-id='messages-message-success']").First;
        public ILocator TotalPaidPrice => page.Locator("tr:has(td.label:has-text('Total Paid')) td:nth-child(2) .price").First;


        // Method to get the total paid price
        public async Task<string> GetTotalPaidPriceAsync()
        {
            return await TotalPaidPrice.InnerTextAsync();
        }


        // Method to get the success message text
        public async Task<string> GetSuccessMessageAsync()
        {
            return await SuccessMessage.InnerTextAsync();
        }


        // Method to get the order number
        public async Task<string> GetOrderNumberAsync()
        {
            await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            // Get the full text content of the order number container
            var fullText = await OrderNumberContainer.InnerTextAsync();

            // Extract the order number using string manipulation
            var orderNumber = fullText.Split('(')[0].Split('#')[1].TrimEnd().TrimStart(); // Assuming the format is "Order # 93300009961"

            return orderNumber;
        }

        // Method to click the "Submit Order" button
        public async Task ClickSubmitOrderButtonAsync()
        {
            await SubmitOrderButton.ClickAsync();
        }

        public async Task InputCreditCardDetailsAsync(string cardNumber, string expMonth, string expYear, string cvv)
        {
            // Input credit card number
            await CardNumberInput.FillAsync(cardNumber);

            // Input expiration month
            await ExpMonthInput.FillAsync(expMonth);

            // Input expiration year
            await ExpYearInput.FillAsync(expYear);

            // Input CVV
            await CVVInput.FillAsync(cvv);
        }

        // Method to click the label
        public async Task ClickCreditCardLabelAsync()
        {
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await CreditCardLabel.ClickAsync();
        }

        // Method to click the label
        public async Task ClickFreeShippingLabelAsync()
        {
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await FreeShippingLabel.ClickAsync();
        }

        // Method to click the button
        public async Task ClickAddSelectedProductsAsync()
        {
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await AddSelectedProductsButton.ClickAsync();
        }

        // Method to check the checkbox
        public async Task CheckProductCheckboxAsync()
        {
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await Task.Delay(5000);
            if (!await ProductCheckbox.IsCheckedAsync())
            {
                await ProductCheckbox.CheckAsync();
            }
        }

        // Method to click the "Search" button
        public async Task ClickSearchProductButtonAsync()
        {
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await SearchProductButton.ClickAsync();
        }

        // Method to input text into the name field
        public async Task InputProductNameAsync(string name)
        {
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await ProductNameInput.FillAsync(name);
        }

        // Method to click the "Add Products" button
        public async Task ClickAddProductsAsync()
        {
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await AddProductsButton.ClickAsync();
        }

        // Method to click the link
        public async Task ClickGetShippingMethodsAsync()
        {
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await GetShippingMethodsLink.ClickAsync();
        }


        public async Task ClickStoreByName(string storeName)
        {
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await StoreLabel(storeName).ClickAsync();
        }
        // Method to click the Search button
        public async Task ClickSearchCustomerButtonAsync()
        {
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await SearchCustomerButton.ClickAsync();
        }

        // Method to input text into the email field
        public async Task InputEmailAsync(string email)
        {
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await EmailInput.FillAsync(email);
        }

        public async Task ClickToSelectCustomer(string emailAddress)
        {
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await EmailCell(emailAddress).ClickAsync();
        }
    }
}
