using System;
using Reqnroll.BoDi;
using NUnit.Framework;
using EquipmentStoresTests.Models.Components.Dialogs.StoreDialogs;
using Reqnroll.Infrastructure;
using Microsoft.Playwright;
using System.Threading.Tasks;
using EquipmentStoresTests.Models.Pages;
using EquipmentStoresTests.Models.Data.DTO;
using EquipmentStoresTests.Models.Data;
using EquipmentStoresTests.Support;
using EquipmentStoresTests.Models.Pages.StorePages;
using EquipmentStoresTests.Configuration;
using EquipmentStoresTests.Drivers;
using System.Linq;
using EquipmentStoresTests.Models.Constants;

namespace EquipmentStoresTests.StepDefinitions
{
    [Binding]
    public class EQ_FE_EndtoEndStepDefinitions : BaseSteps
    {
        IPage _mailPage;
        IPage _LesmillPlusRigesterPage;
        Settings _settings;
        public EQ_FE_EndtoEndStepDefinitions(IObjectContainer objectContainer, ScenarioContext scenarioContext) : base(objectContainer, scenarioContext)
        {
            _settings = UIDriverFactory.GetSettings();
        }

        [Given(@"I am located on home page of equipment store")]
        public async Task GivenIAmLocatedOnHomePage()
        {
            await page.GotoAsync(_settings.UIStoreBaseUrl);
            await page.BringToFrontAsync();
            try
            {
                var homepage = new HomePage(page);
                await homepage.HoverSearchToggle();
                var regionSelectorPopup = new RegionSelectorDialogs<HomePage>(page, homepage);
                await regionSelectorPopup.WaitForPopupToAppear();
                await regionSelectorPopup.Close();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

        [When(@"I create a new customer account (.*)")]
        public async Task WhenICreateANewCustomerAccountCustomerxx(string customerName)
        {
            CustomerDTO currentCustomer = TestDataLoader.LoadCustomer(customerName);
            var homepage = new HomePage(page);
            await homepage.ClickCreateAccountAsync();
            var createPage = new CreatePage(page);
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            string emailAddress = timestamp + currentCustomer.Contact.EmailAddress;
            Console.WriteLine($"New customer email address is {emailAddress}");
            scenarioContext[SCKeys.EmailAddress] = emailAddress;
            scenarioContext[SCKeys.CustomerName] = customerName;
            await createPage.FillFormAsync(currentCustomer.FirstName, currentCustomer.LastName, emailAddress, currentCustomer.Contact.Password);
            await createPage.SubmitFormAsync();
        }

        [When(@"I active my new account")]
        public async Task WhenIActiveMyNewAccount()
        {
            try
            {
                _mailPage = await MailinatorController.GetMailinatorPageAsync(page);
                var mailinatorPage = new MailinatorPage(_mailPage);
                string emailAddress = scenarioContext[SCKeys.EmailAddress].ToString();
                await mailinatorPage.OpenInbox(emailAddress);
                string emailTitle = "Please confirm your Les Mills Equipment account - UAT";
                await mailinatorPage.OpenEmail(emailTitle);

                // Listen for new pages
                IPage newTab = null;
                page.Context.Page += (_, tab) =>
                {
                    newTab = tab;
                };
                await mailinatorPage.ClickConfirmYourAccountButton();

                // Wait for the new tab to open
                await Task.Delay(5000);

                if (newTab != null)
                {
                    await newTab.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
                    Console.WriteLine("New tab title: " + await newTab.TitleAsync());
                    await newTab.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                await MailinatorController.ClosePage();
                await page.BringToFrontAsync();
            }
        }

        [When(@"I login using this new account")]
        public async Task WhenILoginUsingThisNewAccount()
        {
            CustomerDTO currentCustomer = TestDataLoader.LoadCustomer(scenarioContext[SCKeys.CustomerName].ToString());
            var loginPage = new LoginPage(page);
            await loginPage.FillLoginFormAsync(scenarioContext[SCKeys.EmailAddress].ToString(), currentCustomer.Contact.Password);
            await loginPage.SubmitLoginFormAsync();
        }

        [When(@"I add shipping address information")]
        public async Task WhenIAddShippingAddressInformation()
        {
            CustomerDTO currentCustomer = TestDataLoader.LoadCustomer(scenarioContext[SCKeys.CustomerName].ToString());
            var accountPage = new AccountPage(page);
            await accountPage.ClickEditAddressButtonAsync();
            var newAddressPage = new NewAddressPage(page);
            await newAddressPage.FillAddressFormAsync(currentCustomer.FirstName, currentCustomer.LastName, currentCustomer.Address.Street, currentCustomer.Address.City, currentCustomer.Address.State, currentCustomer.Address.ZipCode, currentCustomer.Address.Country, currentCustomer.Contact.Phone);
            await newAddressPage.SubmitAddressFormAsync();
        }


        [When(@"I navigate to Equipment Page via (.*)")]
        public async Task GivenINavigateToTheEquipmentPage(string subTitleLink)
        {
            var homepage = new HomePage(page);
            await homepage.ClickEquipmentSetLinkAsync(subTitleLink);
        }

        [When(@"I click equipment (.*) on Equipment page")]
        public async Task WhenIClickEquipmentBarbellSetOnEquipmentPage(string equipmentName)
        {
            var equipmentPage = new EquipmentPage(page);
            scenarioContext[SCKeys.EquipmentName] = equipmentName;
            scenarioContext[SCKeys.EquipmentPrice] = await equipmentPage.GetProductPriceAsync(equipmentName);
            await equipmentPage.ClickProductImageAsync(equipmentName);
        }


        [When(@"I add (.*) to cart on Equipment page")]
        public async Task WhenIAddLesMillsSmartbarSingleBarToCartOnEquipmentPage(string equipmentName)
        {

            var equipmentPage = new EquipmentPage(page);
            scenarioContext[SCKeys.EquipmentPrice] = await equipmentPage.GetProductPriceAsync(equipmentName);
            await equipmentPage.ClickAddToCartButton(equipmentName);
        }

        [When(@"I click Add to cart on Product Details page")]
        public async Task WhenIClickAddToCartOnProductDetailsPage()
        {
            var productDetialsPage = new ProductDetailsPage(page, scenarioContext[SCKeys.EquipmentName].ToString());
            await productDetialsPage.ClickAddToCartButtonAsync();
        }


        [Then(@"I should see equipment (.*) be added into Cart Dialogs")]
        public async Task ThenIShouldSeeinCartDialogs(string equipmentName)
        {
            var cartDialogs = new CartDialogs<EquipmentPage>(page, new EquipmentPage(page));
            scenarioContext[SCKeys.EquipmentName] = equipmentName;
            Assert.IsTrue(await cartDialogs.CheckProductVisibleInCart(equipmentName), $"The equipment {equipmentName} should has been added into cart.");
        }

        [Then(@"I should see free subscription (.*) be added into Cart Dialogs")]
        public async Task ThenIShouldSeesubscriptioninCartDialogs(string name)
        {
            var cartDialogs = new CartDialogs<EquipmentPage>(page, new EquipmentPage(page));
            Assert.IsTrue(await cartDialogs.CheckFreeSubscriptionVisibleInCart(name), $"The free subscription {name} should has been added into cart.");
        }

        [Then(@"I should see the correct price and Subtotal in Cart Dialogs")]
        public async Task ThenIShouldSeeTheCorrectPriceAndSubtotalInCartDialogs()
        {
            string equipmentName = scenarioContext[SCKeys.EquipmentName].ToString();
            var cartDialogs = new CartDialogs<EquipmentPage>(page, new EquipmentPage(page));
            string equipmentPrice = await cartDialogs.GetItemPriceByName(equipmentName);
            decimal equipmentPriceDec = decimal.Parse(equipmentPrice.TrimStart('$'));
            string expectedPrice = scenarioContext[SCKeys.EquipmentPrice].ToString();
            Assert.AreEqual(expectedPrice, equipmentPrice, $"The price of {equipmentName} should be {expectedPrice}.");
            int equipmentQuantity = await cartDialogs.GetItemQuantityByName(equipmentName);
            decimal expectledSubTotalDec = equipmentQuantity * equipmentPriceDec;
            string expectedSubTotal = "$" + expectledSubTotalDec.ToString();
            string actualSubTotal = await cartDialogs.GetSubtotal();
            Assert.AreEqual(expectedSubTotal, actualSubTotal, $"The subtoal should be {expectedSubTotal}.");
        }


        [When(@"I click CHECKOUT button on Cart Dialogs")]
        public async Task WhenIClickCHECKOUTButtonOnCartDialogs()
        {
            var cartDialogs = new CartDialogs<EquipmentPage>(page, new EquipmentPage(page));
            await cartDialogs.ClickCheckoutButton();
        }

        [When(@"I close the Free Gift Popup")]
        public async Task WhenICloseTheFreeGiftPopup()
        {
            try
            {
                var freeGiftDialogs = new FreeGiftDialogs<ShippingPage>(page, new ShippingPage(page));
                await freeGiftDialogs.ClosePromoPopup();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }


        [When(@"I enter (.*) customer (.*) info on Shipping Page")]
        public async Task WhenIEnterGuestCustomerCustomerInfoOnShippingPage(string customerType, string customerName)
        {

            switch (customerType)
            {
                case "guest":
                    {
                        CustomerDTO currentCustomer = TestDataLoader.LoadCustomer(customerName);
                        Console.WriteLine($"Current customer is {currentCustomer.Contact.EmailAddress}");
                        var shippingPage = new ShippingPage(page);
                        await shippingPage.EnterEmailAsync(currentCustomer.Contact.EmailAddress);
                        await shippingPage.EnterShippingAddress(currentCustomer);
                        scenarioContext[SCKeys.CustomerName] = customerName;
                        break;
                    }
                case "existing":
                    {
                        CustomerDTO currentCustomer = TestDataLoader.LoadCustomer(customerName);
                        Console.WriteLine($"Current customer is {currentCustomer.Contact.EmailAddress}");
                        var shippingPage = new ShippingPage(page);
                        await shippingPage.ExistingCustomerLogin(currentCustomer);
                        scenarioContext[SCKeys.CustomerName] = customerName;
                        break;
                    }

                default:
                    throw new Exception("Please provide correct customer type, guest or existing.");
            }

        }


        [Then(@"I should see the shipping info be entered automaticlly")]
        public async Task WhenIShouldSeeTheShippingInfoBeEnteredAutomaticlly()
        {
            CustomerDTO currentCustomer = TestDataLoader.LoadCustomer(scenarioContext[SCKeys.CustomerName].ToString());
            var shippingPage = new ShippingPage(page);
            await Task.Delay(10000);
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            Assert.AreEqual(currentCustomer.FirstName, (await shippingPage.FirstNameInput.InputValueAsync()));
            Assert.AreEqual(currentCustomer.LastName, (await shippingPage.LastNameInput.InputValueAsync()));
            Assert.AreEqual(currentCustomer.Address.Street, (await shippingPage.StreetAddressInput.InputValueAsync()));
            Assert.AreEqual(currentCustomer.Address.City, (await shippingPage.CityInput.InputValueAsync()));
            Assert.AreEqual(currentCustomer.Address.ZipCode, (await shippingPage.ZipCodeInput.InputValueAsync()));
            Assert.AreEqual(currentCustomer.Address.Country, (await shippingPage.GetSelectCountryTextAsync()));
            Assert.AreEqual(currentCustomer.Contact.Phone, (await shippingPage.PhoneInput.InputValueAsync()));

        }

        [When(@"I enter discount code (.*)")]
        public async Task WhenIEnterDiscountCodeTestAutomation(string discountCodeName)
        {
            var shippingPage = new ShippingPage(page);
            await shippingPage.ApplyDiscountAsync(discountCodeName);
            scenarioContext[SCKeys.DiscountCode] = discountCodeName;
        }

        [Then(@"I should see the Order Total include the calculated Tax on Shipping Page")]
        public async Task ThenIShouldSeeTheOrderTotalIncludeTheCalculatedTaxOnShippingPage()
        {
            var shippingPage = new ShippingPage(page);
            decimal subtotal = decimal.Parse((await shippingPage.GetSubtotalAsync()).TrimStart('$'));
            decimal tax = decimal.Parse((await shippingPage.GetTaxValueAsync()).TrimStart('$'));
            string orderTotal = await shippingPage.GetOrderTotalAsync();
            scenarioContext[SCKeys.OrderTotal] = orderTotal;
            decimal orderTotaldec = decimal.Parse(orderTotal.TrimStart('$'));
            Assert.AreEqual(subtotal + tax, orderTotaldec, $"Order total value should be ${subtotal + tax}.");
        }

        [Then(@"I should see the Order Total include the calculated Tax and exclude the discount on Shipping Page")]
        public async Task ThenIShouldSeeTheOrderTotalIncludeTheCalculatedTaxAndExcludeTheDiscountOnShippingPage()
        {
            var shippingPage = new ShippingPage(page);
            decimal subtotal = decimal.Parse((await shippingPage.GetSubtotalAsync()).TrimStart('$'));
            decimal tax = decimal.Parse((await shippingPage.GetTaxValueAsync()).TrimStart('$'));
            string orderTotal = await shippingPage.GetOrderTotalAsync();
            scenarioContext[SCKeys.OrderTotal] = orderTotal;
            decimal orderTotaldec = decimal.Parse(orderTotal.TrimStart('$'));
            decimal discountDec = decimal.Parse((await shippingPage.GetDiscountAmountByName(scenarioContext[SCKeys.DiscountCode].ToString())).TrimStart('-').TrimStart('$'));

            Assert.AreEqual(subtotal + tax - discountDec, orderTotaldec, $"Order total value should be ${subtotal + tax - discountDec}.");
        }


        [Then(@"I should see (.*)% discount is applied")]
        public async Task ThenIShouldSeeDiscountIsApplied(int discount)
        {
            var shippingPage = new ShippingPage(page);
            decimal discountDec = decimal.Parse((await shippingPage.GetDiscountAmountByName(scenarioContext[SCKeys.DiscountCode].ToString())).TrimStart('-').TrimStart('$'));
            decimal subtotal = decimal.Parse((await shippingPage.GetSubtotalAsync()).TrimStart('$'));
            Assert.AreEqual(subtotal * discount * 0.01m, discountDec, $"The ammount of Discount {scenarioContext[SCKeys.DiscountCode].ToString()} should be -${(subtotal * discount * 0.01m).ToString()}.");

        }


        [Then(@"I should see free shipping is displayed on Shipping Page")]
        public async Task ThenIShouldSeeFreeShippingIsDisplayedOnShippingPage()
        {
            var shippingPage = new ShippingPage(page);
            Assert.IsTrue(await shippingPage.IsOFreeShippingInfoBlockVisibleAsync(), $"Free shipping should be displayed.");
        }



        [When(@"I click Continue to payment on Shipping Page")]
        public async Task WhenIClickContinueToPaymentOnShippingPage()
        {
            var shippingPage = new ShippingPage(page);
            await shippingPage.ClickContinueToPaymentAsync();
        }


        [When(@"I enter (.*) payment info of (.*) on Payment Page")]
        public async Task WhenIEnterPaymentInfoOfCustomerOnPaymentPage(string paymentOptions, string customerName)
        {
            switch (paymentOptions)
            {
                case "credit card":
                    {
                        CustomerDTO currentCustomer = TestDataLoader.LoadCustomer(customerName);
                        var paymentPage = new PaymentPage(page);
                        await paymentPage.SelectCreditCardOptionAsync();
                        await paymentPage.EnterCreditCardDetailsAsync(currentCustomer.CreditCard.CardNumber, currentCustomer.CreditCard.Expiry, currentCustomer.CreditCard.Cvv);
                        await paymentPage.PlaceOrderAsync();
                        break;
                    }
                case "PayPal":
                    {
                        CustomerDTO currentCustomer = TestDataLoader.LoadCustomer(customerName);
                        var paymentPage = new PaymentPage(page);
                        await paymentPage.SelectPaypalOptionAsync();
                        var popupTask = page.WaitForPopupAsync();
                        await paymentPage.ClickPaywithPaypalButton();
                        var popup = await popupTask;
                        await popup.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
                        await popup.FillAsync("#email", currentCustomer.Paypal.PaypalEmail);
                        await popup.ClickAsync("#btnNext");
                        await popup.FillAsync("#password", currentCustomer.Paypal.PaypalPassword);
                        await popup.ClickAsync("#btnLogin");
                        await popup.ClickAsync("#payment-submit-btn");
                        break;
                    }
                default:
                    throw new Exception("Please provide correct payment options.");

            }

        }

        [Then(@"I should see order success message (.*) on Order Success page")]
        public async Task ThenIShouldSeeOrderSuccessMessageOnOrderSuccessPage(string expectSuccessMsg)
        {
            var successPage = new OrderSuccessPage(page);
            string successMessage = await successPage.GetSuccessMessageAsync();
            Assert.AreEqual(expectSuccessMsg, successMessage, $"The success message should be {expectSuccessMsg}");
        }
        [Then(@"I should see the order number is presented on Order Success page")]
        public async Task ThenIShouldSeeTheOrderNumberIsPresentedOnOrderSuccessPage()
        {
            var successPage = new OrderSuccessPage(page);
            string orderNumber = await successPage.GetOrderNumberAsync();
            orderNumber = orderNumber.Split(':')[1].TrimStart().TrimEnd('.').TrimEnd();
            scenarioContext[SCKeys.OrderNumber] = orderNumber;
            Assert.IsTrue(await successPage.CheckOrderNumberVisibleAsync(), $"Order number should be visible on the success page.");
        }

        [Then(@"I should see the Create your account here link on Order Success page")]
        public async Task ThenIShouldSeeTheCreateYourAccountHereLinkOnOrderSuccessPage()
        {
            var successPage = new OrderSuccessPage(page);
            Assert.IsTrue(await successPage.CheckCreateAccountLinkVisibleAsync(), $"Create Account Link should be visible on the success page.");
        }

        [Then(@"I should see the email address of (.*) is displayed on OrderSuccess page")]
        public async Task ThenIShouldSeeTheEmailAddressOfCustomerIsDisplayedOnOrderSuccessPage(string customerName)
        {
            CustomerDTO currentCustomer = TestDataLoader.LoadCustomer(customerName);
            string expectedEmail = currentCustomer.CustomerType == "new" ? scenarioContext[SCKeys.EmailAddress].ToString() : currentCustomer.Contact.EmailAddress;
            var successPage = new OrderSuccessPage(page);
            Assert.AreEqual("Your email: " + expectedEmail, await successPage.GetEmailAsync(), $"Email address should be {expectedEmail}.");
        }

        [Then(@"I should see the order confirm email has been send to customer (.*) with correct info")]
        public async Task ThenIShouldSeeTheOrderConfirmEmailHasBeenSendToCustomer(string customerName)
        {
            CustomerDTO currentCustomer = TestDataLoader.LoadCustomer(customerName);
            string expectedEmail = currentCustomer.CustomerType == "new" ? scenarioContext[SCKeys.EmailAddress].ToString() : currentCustomer.Contact.EmailAddress;
            if (scenarioContext.ScenarioInfo.Tags.Contains("store"))
            {
                _mailPage = await MailinatorController.GetMailinatorPageAsync(page);
            }
            else
            {
                _mailPage = await MailinatorController.GetMailinatorPageAsync(adminPage);
            }
            var mailinatorPage = new MailinatorPage(_mailPage);
            await mailinatorPage.OpenInbox(expectedEmail);
            string orderNumber = scenarioContext[SCKeys.OrderNumber].ToString();
            string emailSubject = $"Les Mills Equipment : New order #{orderNumber} - UAT";
            await mailinatorPage.OpenEmail(emailSubject);
            string expectedtotalPrice = scenarioContext[SCKeys.OrderTotal].ToString();
            Assert.AreEqual(expectedtotalPrice, await mailinatorPage.GetTotalPriceAsync(), $"The Total charge should be {expectedtotalPrice}");
            ScreenShotsHelper.TakeScreenshot(_mailPage);
            await mailinatorPage.ClickBackToInbox();
        }

        [When(@"I click Active now in customer email for LesMILLs+Account")]
        public async Task WhenIClickActiveNowInCustomerEmailForLesMILLsAccount()
        {
            string emailSubject = $"Activate Your LesMIlls+ Account";
            var mailinatorPage = new MailinatorPage(_mailPage);
            await mailinatorPage.OpenEmail(emailSubject);
            // Listen for new pages
            page.Context.Page += (_, tab) =>
            {
                _LesmillPlusRigesterPage = tab;
            };
            await mailinatorPage.ClickActiveLinkAsync();

        }

        [Then(@"I should see the LesMILLs+Account register page be opened")]
        public async Task ThenIShouldSeeTheLesMILLsAccountRegisterPageBeOpened()
        {
            await Task.Delay(12000);

            if (_LesmillPlusRigesterPage != null)
            {
                await _LesmillPlusRigesterPage.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
                string expectTitle = $"Let's start with your email";
                ILocator pageTitle = _LesmillPlusRigesterPage.Locator("#confirm-email-title");
                await pageTitle.WaitForAsync(new LocatorWaitForOptions { Timeout = 60000 });
                Assert.AreEqual(expectTitle, await pageTitle.InnerTextAsync(), $"Page title should be {expectTitle}");
                ScreenShotsHelper.TakeScreenshot(_LesmillPlusRigesterPage);
                await _LesmillPlusRigesterPage.CloseAsync();
            }
            else
            {
                Assert.Fail($"LessMILLS plus register page not open.");
            }
        }
    }
}
