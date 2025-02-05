using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipmentStoresTests.Models.Data.DTO
{
    public class AdminContainerDTO
    {
        public Dictionary<string, AdminDTO> Admins { get; set; } = new Dictionary<string, AdminDTO>();
    }

    public class AdminDTO
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string AuthKey { get; set; } = string.Empty;
    }
}
