using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EquipmentStoresTests.Models.API
{
    public abstract class BaseAPI<T> where T : BaseAPI<T>
    {
        IAPIRequestContext _aPIRequestContext;
        public BaseAPI(IAPIRequestContext aPIRequestContext)
        {
            _aPIRequestContext = aPIRequestContext;
        }

        private string resourceUrl;

        public async Task<IAPIResponse> Post(Dictionary<string, string> header, string payload)
        {
            // Create the request options with the serialized payload
            var options = new APIRequestContextOptions
            {
                Headers = header,
                DataObject = JsonDocument.Parse(payload).RootElement
            };

            // Execute the POST request with the options
            return await _aPIRequestContext.PostAsync(resourceUrl, options);
        }
        protected void SetResourceUrl(string resourceUrl)
        {
            //Remove leading slash as not needed for RestSharp implementation
            if (resourceUrl.StartsWith('/') || resourceUrl.StartsWith('\\'))
            {
                resourceUrl = resourceUrl[1..];
            }
            this.resourceUrl = resourceUrl;
        }
    }
}
