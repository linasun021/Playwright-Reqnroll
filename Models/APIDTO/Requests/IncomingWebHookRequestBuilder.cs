using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;

namespace EquipmentStoresTests.Models.APIDTO.Requests
{
    public class IncomingWebHookRequestBuilder
    {
        private IncomingWebHookRequestDto _requestDto;

        public static async Task<IncomingWebHookRequestBuilder> LoadFromJsonFileAsync()
        {
            string fileName = $@"./Models/APIDTO/Requests/IncomingWebHookRequest.json";
            string filePath = Path.Combine(AppContext.BaseDirectory, fileName);
            string jsonData = File.ReadAllText(filePath);
            var jsonString = await File.ReadAllTextAsync(filePath);
            var requestDto = JsonSerializer.Deserialize<IncomingWebHookRequestDto>(jsonString);
            return new IncomingWebHookRequestBuilder(requestDto);
        }

        private IncomingWebHookRequestBuilder(IncomingWebHookRequestDto requestDto)
        {
            _requestDto = requestDto;
        }

        public IncomingWebHookRequestBuilder UpdateOurReference(string newReference)
        {
            _requestDto.Content.Reference.OurReference = newReference;
            return this;
        }

        public IncomingWebHookRequestBuilder UpdateYourReference(string newReference)
        {
            _requestDto.Content.Reference.YourReference = newReference;
            return this;
        }

        public IncomingWebHookRequestBuilder UpdateCarrierReference(string newCarrierReference)
        {
            if (_requestDto.Content.Reference.CarrierReferences != null && _requestDto.Content.Reference.CarrierReferences.Count > 0)
            {
                _requestDto.Content.Reference.CarrierReferences[0].Reference = newCarrierReference;
            }
            return this;
        }

        public string SerializeToJson()
        {
            return JsonSerializer.Serialize(_requestDto, new JsonSerializerOptions { WriteIndented = true });
        }

        public IncomingWebHookRequestDto Build()
        {
            return _requestDto;
        }
    }
}
