using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EquipmentStoresTests.Support;
using Microsoft.Playwright;
using Reqnroll.Infrastructure;
using static System.Net.Mime.MediaTypeNames;

namespace EquipmentStoresTests.Models.Pages.StorePages
{
    public class PaymentPage : BasePage<PaymentPage>
    {

        public PaymentPage(IPage page) : base(page) { }

        public ILocator CreditCardOptionLabel => page.Locator("label[for='braintree']");
        public ILocator PayPalOptionLabel => page.Locator("label[for='braintree_paypal']");
        public ILocator GooglePayOptionLabel => page.Locator("label[for='braintree_googlepay']");
        public ILocator KlarnaOptionLabel => page.Locator("label[for='klarna_pay_over_time']");
        public ILocator PaymentMethods => page.Locator("#checkout-payment-method-load .payment-methods");
        public ILocator CreditCardOption => page.Locator("#braintree");
        public ILocator PaywithPaypalButton => page.FrameLocator("#braintree_paypal_placeholder iframe.component-frame").Locator("#buttons-container");

        public ILocator PayPalOption => page.Locator("#braintree_paypal");
        public ILocator GooglePayOption => page.Locator("#braintree_googlepay");
        public ILocator CreditCardNumber => page.FrameLocator("iframe[name='braintree-hosted-field-number']").Locator("input[data-braintree-name='number']");
        public ILocator ExpirationDate => page.FrameLocator("iframe[name='braintree-hosted-field-expirationDate']").Locator("input[data-braintree-name='expirationDate']");
        public ILocator CVV => page.FrameLocator("iframe[name='braintree-hosted-field-cvv']").Locator("input[data-braintree-name='cvv']");
        public ILocator PlaceOrderButton => page.Locator("#place-order-trigger");

        // Methods
        public async Task<PaymentPage> SelectCreditCardOptionAsync()
        {
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await CreditCardOptionLabel.ClickAsync();
            return this;
        }

        public async Task<PaymentPage> EnterCreditCardDetailsAsync(string cardNumber, string expirationDate, string cvv)
        {
            await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded, new PageWaitForLoadStateOptions { Timeout = 12000 });
            await CreditCardNumber.FillAsync(cardNumber);
            await ExpirationDate.FillAsync(expirationDate);
            await CVV.FillAsync(cvv);
            return this;
        }

        public async Task<PaymentPage> SelectPaypalOptionAsync()
        {
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await PayPalOptionLabel.ClickAsync();
            return this;
        }

        public async Task<PaymentPage> ClickPaywithPaypalButton()
        {
            await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded, new PageWaitForLoadStateOptions { Timeout = 12000 });
            await PaywithPaypalButton.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 12000 });
            await PaywithPaypalButton.ClickAsync();
            return this;
        }
        public async Task<PaymentPage> PlaceOrderAsync()
        {
            await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded, new PageWaitForLoadStateOptions { Timeout = 12000 });
            await PlaceOrderButton.ClickAsync();
            return this;
        }
    }
}
