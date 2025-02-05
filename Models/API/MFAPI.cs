using EquipmentStoresTests.Models.APIDTO.Requests;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Reqnroll;

namespace EquipmentStoresTests.Models.API
{
    public class MFAPI : BaseAPI<MFAPI>
    {
        public MFAPI(IAPIRequestContext aPIRequestContext) : base(aPIRequestContext) { }

        public async Task<IAPIResponse> PostIncomingWebHook(string orderNumber, string tracingNumber)
        {
            SetResourceUrl($"/lm_mainfreightordersapi/webhook/index");
            var builder = await IncomingWebHookRequestBuilder.LoadFromJsonFileAsync();
            var requestDtoJson = builder.UpdateOurReference($"{orderNumber}")
                                    .UpdateYourReference($"{orderNumber}")
                                    .UpdateCarrierReference($"{tracingNumber}")
                                    .SerializeToJson();
            Dictionary<string, string> headers = new Dictionary<string, string>
            {
                { "header-lm-key", "FMfcgxwKjdzpqrVpQJfStRxxDNqcQKTk" },
                { "Content-Type", "application/json" }
            };
            return await Post(headers, requestDtoJson);
        }
    }
}
