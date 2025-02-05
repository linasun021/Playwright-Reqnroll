using System;
using System.Threading.Tasks;
using Reqnroll.BoDi;
using EquipmentStoresTests.Configuration;
using EquipmentStoresTests.Drivers;
using EquipmentStoresTests.Models.Data.DTO;
using EquipmentStoresTests.Models.Data;
using EquipmentStoresTests.Models.Pages.AdminPages;
using Microsoft.Playwright;
using Reqnroll.Infrastructure;
using NUnit.Framework;
using System.Linq;
using EquipmentStoresTests.Models.Pages;
using EquipmentStoresTests.Support;
using EquipmentStoresTests.Models.Constants;
using EquipmentStoresTests.Models.Components.Dialogs.AdminDialogs;
using OtpNet;

namespace EquipmentStoresTests.StepDefinitions
{
    [Binding]
    public class CreateOrderStepDefinitions : BaseSteps
    {
        Settings _settings;
        IPage _mailPage;
        public CreateOrderStepDefinitions(IObjectContainer objectContainer, ScenarioContext scenarioContext) : base(objectContainer, scenarioContext)
        {
            _settings = UIDriverFactory.GetSettings();
        }
        [When(@"I am located on equipment admin using (.*)")]
        [Given(@"I am located on equipment admin using (.*)")]
        public async Task GivenIAmLocatedOnDashboardPageOfEquipmentAdmin(string adminName)
        {
            if (scenarioContext.ScenarioInfo.Tags.Contains("store"))
            {
                adminPage = await page.Context.NewPageAsync();
            }
            await adminPage.GotoAsync(_settings.UIAdminBaseUrl);
            await adminPage.BringToFrontAsync();

            var currentAdmin = TestDataLoader.LoadAdmin(adminName);
            var homePage = new HomePage(adminPage);
            var loginDialogs = new AdminLoginDialogs<HomePage>(adminPage, homePage);
            await loginDialogs.LoginAsync(currentAdmin.UserName, currentAdmin.Password);
            ScreenShotsHelper.TakeScreenshot(adminPage);
            var totp = new Totp(Base32Encoding.ToBytes(currentAdmin.AuthKey));
            var result = totp.ComputeTotp();
            await loginDialogs.ConfirmAuthenticatorCodeAsync(result);
        }

        [When(@"I create New Order on product (.*) for existing customer (.*) in store (.*)")]
        public async Task WhenICreateNewOrderOnOrderPage(string productName, string customerName, string storeName)
        {
            CustomerDTO currentCustomer = TestDataLoader.LoadCustomer(customerName);
            var dashboardPage = new HomePage(adminPage);
            await dashboardPage.OpenSalesMenuAsync();
            await dashboardPage.NavigateToOrdersAsync();
            ScreenShotsHelper.TakeScreenshot(adminPage);
            var orderPage = new OrdersPage(adminPage);
            await orderPage.ClickCreateNewOrderAsync();
            var orderCreatePage = new OrderCreatePage(adminPage);
            await orderCreatePage.InputEmailAsync(currentCustomer.Contact.EmailAddress);
            await orderCreatePage.ClickSearchCustomerButtonAsync();
            ScreenShotsHelper.TakeScreenshot(adminPage);
            await orderCreatePage.ClickToSelectCustomer(currentCustomer.Contact.EmailAddress);
            await orderCreatePage.ClickStoreByName(storeName);
            await orderCreatePage.ClickAddProductsAsync();
            await orderCreatePage.InputProductNameAsync(productName);
            await orderCreatePage.ClickSearchProductButtonAsync();
            ScreenShotsHelper.TakeScreenshot(adminPage);
            await orderCreatePage.CheckProductCheckboxAsync();
            await orderCreatePage.ClickAddSelectedProductsAsync();
            await orderCreatePage.ClickGetShippingMethodsAsync();
            await orderCreatePage.ClickFreeShippingLabelAsync();
            await orderCreatePage.ClickCreditCardLabelAsync();
            await orderCreatePage.InputCreditCardDetailsAsync(currentCustomer.CreditCard.CardNumber, currentCustomer.CreditCard.Expiry.Split('/')[0], currentCustomer.CreditCard.Expiry.Split('/')[1], currentCustomer.CreditCard.Cvv);
            ScreenShotsHelper.TakeScreenshot(adminPage);
            await orderCreatePage.ClickSubmitOrderButtonAsync();
        }

        [Then(@"I should see the success message (.*)")]
        public async Task ThenIShouldSeeTheSuccessMessageYouCreatedTheOrder_(string successMsg)
        {
            var orderCreatePage = new OrderCreatePage(adminPage);
            scenarioContext[SCKeys.OrderNumber] = await orderCreatePage.GetOrderNumberAsync();
            scenarioContext[SCKeys.OrderTotal] = await orderCreatePage.GetTotalPaidPriceAsync();
            Assert.AreEqual(successMsg, await orderCreatePage.GetSuccessMessageAsync(), $"Success message should be {successMsg}");
            ScreenShotsHelper.TakeScreenshot(adminPage);
        }

        [Then(@"I should see the order status is (.*) on equipment admin")]
        public async Task ThenIShouldSeeTheOrderStatusIsProcessingOnAdmin(string orderStatus)
        {
            await Task.Delay(10000);
            string customerName = scenarioContext[SCKeys.CustomerName].ToString();
            string orderNumber = scenarioContext[SCKeys.OrderNumber].ToString();
            CustomerDTO currentCustomer = TestDataLoader.LoadCustomer(customerName);
            var dashboardPage = new HomePage(adminPage);
            await dashboardPage.OpenSalesMenuAsync();
            await dashboardPage.NavigateToOrdersAsync();
            var orderPage = new OrdersPage(adminPage);
            await orderPage.SearchForOrder(orderNumber);
            await orderPage.ClickViewLinkByOrderNumber(orderNumber);
            Assert.AreEqual(orderStatus, await orderPage.GetOrderStatusText(), $"Order status should be {orderStatus}");
            ScreenShotsHelper.TakeScreenshot(adminPage);
        }

        [Then(@"I should see the admin created order confirm email has been send to customer (.*) with correct info")]
        public async Task ThenIShouldSeeTheOrderConfirmEmailHasBeenSendToCustomer(string customerName)
        {
            CustomerDTO currentCustomer = TestDataLoader.LoadCustomer(customerName);
            string expectedEmail = currentCustomer.CustomerType == "new" ? scenarioContext[SCKeys.EmailAddress].ToString() : currentCustomer.Contact.EmailAddress;
            _mailPage = await MailinatorController.GetMailinatorPageAsync(adminPage);
            var mailinatorPage = new MailinatorPage(_mailPage);
            await mailinatorPage.OpenInbox(expectedEmail);
            string orderNumber = scenarioContext[SCKeys.OrderNumber].ToString();
            string emailSubject = $"Les Mills Equipment: Order # {orderNumber} - UAT";
            await mailinatorPage.OpenEmail(emailSubject);
            string expectedtotalPrice = scenarioContext[SCKeys.OrderTotal].ToString();
            Assert.AreEqual(expectedtotalPrice, await mailinatorPage.GetTotalPriceAsync(), $"The Total charge should be {expectedtotalPrice}");
            ScreenShotsHelper.TakeScreenshot(_mailPage);
            await mailinatorPage.ClickBackToInbox();
        }

    }
}
