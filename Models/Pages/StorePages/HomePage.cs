using Microsoft.Playwright;
using static System.Net.Mime.MediaTypeNames;

namespace EquipmentStoresTests.Models.Pages.StorePages
{
    public class HomePage : BasePage<HomePage>
    {
        public HomePage(IPage page) : base(page) { }
    }
}
