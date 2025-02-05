using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipmentStoresTests.Models.Data.DTO
{
    public class CustomerContainerDTO
    {
        public Dictionary<string, CustomerDTO> Customers { get; set; } = new Dictionary<string, CustomerDTO>();
    }

    public class CustomerDTO
    {
        public string CustomerType { get; set; } = "guest";
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public AddressDTO Address { get; set; } = new AddressDTO();
        public ContactDTO Contact { get; set; } = new ContactDTO();
        public CreditCardDTO CreditCard { get; set; } = new CreditCardDTO();
        public PaypalDTO Paypal { get; set; } = new PaypalDTO();
    }

    public class AddressDTO
    {
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
    }

    public class ContactDTO
    {
        public string Phone { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class CreditCardDTO
    {
        public string CardNumber { get; set; } = string.Empty;
        public string Expiry { get; set; } = string.Empty;
        public string Cvv { get; set; } = string.Empty;
    }

    public class PaypalDTO
    {
        public string PaypalEmail { get; set; } = string.Empty;
        public string PaypalPassword { get; set; } = string.Empty;
    }
}
