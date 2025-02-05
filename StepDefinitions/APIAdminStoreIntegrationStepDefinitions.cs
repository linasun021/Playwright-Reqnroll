using System;
using Reqnroll.BoDi;
using EquipmentStoresTests.Configuration;
using EquipmentStoresTests.Drivers;
using EquipmentStoresTests.Models.Data.DTO;
using EquipmentStoresTests.Models.Data;
using Reqnroll.Infrastructure;
using System.Threading.Tasks;
using NUnit.Framework;
using EquipmentStoresTests.Models.API;
using Microsoft.Playwright;
using EquipmentStoresTests.Models.Pages;
using EquipmentStoresTests.Support;
using EquipmentStoresTests.Models.Constants;

namespace EquipmentStoresTests.StepDefinitions
{
    [Binding]
    public class APIAdminStoreIntegrationStepDefinitions : BaseSteps
    {
        Settings _settings;
        IAPIResponse aPIResponse;
        IPage _mailPage;
        public APIAdminStoreIntegrationStepDefinitions(IObjectContainer objectContainer, ScenarioContext scenarioContext) : base(objectContainer, scenarioContext)
        {
            _settings = UIDriverFactory.GetSettings();
        }

        

        [When(@"I ship the order using MF API")]
        public async Task WhenIShipTheOrderUsingMFAPI()
        {
            await Task.Delay(10000);
            var mFAPI = new MFAPI(apiRestquestContext);
            scenarioContext[SCKeys.TracingNumber] = "FD000" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            aPIResponse = await mFAPI.PostIncomingWebHook(scenarioContext[SCKeys.OrderNumber].ToString(), scenarioContext[SCKeys.TracingNumber].ToString());
        }

        [Then(@"I should see the status code is (.*)")]
        public void ThenIShouldSeeTheStatusCodeIs(int statusCode)
        {
            Assert.AreEqual(statusCode, aPIResponse.Status, $"Status code should be {statusCode}");
        }

        [Then(@"I should see the shipped confirm email has beed send to customer (.*) with correct info")]
        public async Task ThenIShouldSeeTheShippedConfirmEmailHasBeedSendToCustomerCustomerWithCorrectInfo(String customerName)
        {
            CustomerDTO currentCustomer = TestDataLoader.LoadCustomer(customerName);
            string expectedEmail = currentCustomer.CustomerType == "new" ? scenarioContext[SCKeys.EmailAddress].ToString() : currentCustomer.Contact.EmailAddress;
            _mailPage = await MailinatorController.GetMailinatorPageAsync(page);
            var mailinatorPage = new MailinatorPage(_mailPage);
            await mailinatorPage.OpenInbox(expectedEmail);
            string orderNumber = scenarioContext[SCKeys.OrderNumber].ToString();
            string emailSubject = $"Here it comes - Les Mills Equipment: Order # {orderNumber} - UAT";
            await mailinatorPage.OpenEmail(emailSubject);
            string expectedTracingNumber = scenarioContext[SCKeys.TracingNumber].ToString();
            Assert.AreEqual(expectedTracingNumber, await mailinatorPage.GetTrackingNumberAsync(), $"The tracing number should be {expectedTracingNumber}");
            await mailinatorPage.ClickBackToInbox();
        }
    }
}
