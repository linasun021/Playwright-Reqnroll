using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using EquipmentStoresTests.Models.Pages;
using Microsoft.Playwright;
using NUnit.Framework;

namespace EquipmentStoresTests.Support
{
    public class MailinatorController
    {
        private static IPage mailinatorPage;
        public static async Task<IPage> GetMailinatorPageAsync(IPage originalPage)
        {
            mailinatorPage = await originalPage.Context.NewPageAsync();

            // Navigate to Mailinator's public inbox page
            await mailinatorPage.GotoAsync("https://www.mailinator.com/v4/public/inboxes.jsp");

            return mailinatorPage;
        }

        public static async Task ClosePage()
        {
            if (mailinatorPage != null) { await mailinatorPage.CloseAsync(); }
        }
    }
}
