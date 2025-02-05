using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using EquipmentStoresTests.Drivers;
using EquipmentStoresTests.Models.Data.DTO;

namespace EquipmentStoresTests.Models.Data
{
    public class TestDataLoader
    {
        public static CustomerDTO LoadCustomer(string userKey)
        {
            string environment = UIDriverFactory.GetSettings().Environment ?? "UAT";

            string fileName = $@"./Models/Data/JSON/CustomerDetails{environment}.json";
            string filePath = Path.Combine(AppContext.BaseDirectory, fileName);
            string jsonData = File.ReadAllText(filePath);

            Dictionary<string, CustomerDTO> customers = JsonSerializer.Deserialize<Dictionary<string, CustomerDTO>>(jsonData);

            if (customers != null && customers.TryGetValue(userKey, out CustomerDTO customer))
            {
                return customer;
            }
            else
            {
                throw new KeyNotFoundException($"User with key '{userKey}' not found.");
            }
        }

        public static AdminDTO LoadAdmin(string adminKey)
        {
            string environment = UIDriverFactory.GetSettings().Environment ?? "UAT";

            string fileName = $@"./Models/Data/JSON/AdminDetails{environment}.json";
            string filePath = Path.Combine(AppContext.BaseDirectory, fileName);
            string jsonData = File.ReadAllText(filePath);

            Dictionary<string, AdminDTO> admins = JsonSerializer.Deserialize<Dictionary<string, AdminDTO>>(jsonData);

            if (admins != null && admins.TryGetValue(adminKey, out AdminDTO admin))
            {
                return admin;
            }
            else
            {
                throw new KeyNotFoundException($"User with key '{adminKey}' not found.");
            }
        }
    }
}
