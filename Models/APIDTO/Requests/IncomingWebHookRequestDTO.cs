using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EquipmentStoresTests.Models.APIDTO.Requests
{
    public class IncomingWebHookRequestDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("metadata")]
        public Metadata Metadata { get; set; }

        [JsonPropertyName("content")]
        public Content Content { get; set; }
    }

    public class Metadata
    {
        [JsonPropertyName("subscriptionId")]
        public string SubscriptionId { get; set; }

        [JsonPropertyName("subscriptionTransportEventId")]
        public string SubscriptionTransportEventId { get; set; }

        [JsonPropertyName("eventTypeCode")]
        public string EventTypeCode { get; set; }

        [JsonPropertyName("serviceTypeCode")]
        public string ServiceTypeCode { get; set; }

        [JsonPropertyName("referenceTypeCode")]
        public string ReferenceTypeCode { get; set; }

        [JsonPropertyName("eventCode")]
        public string EventCode { get; set; }
    }

    public class Content
    {
        [JsonPropertyName("reference")]
        public Reference Reference { get; set; }
    }

    public class Reference
    {
        [JsonPropertyName("ourReference")]
        public string OurReference { get; set; }

        [JsonPropertyName("yourReference")]
        public string YourReference { get; set; }

        [JsonPropertyName("serviceType")]
        public string ServiceType { get; set; }

        [JsonPropertyName("trackingUrl")]
        public string TrackingUrl { get; set; }

        [JsonPropertyName("relatedItems")]
        public List<object> RelatedItems { get; set; }

        [JsonPropertyName("carrierReferences")]
        public List<CarrierReference> CarrierReferences { get; set; }

        [JsonPropertyName("events")]
        public List<Event> Events { get; set; }

        [JsonPropertyName("relatedReferences")]
        public object RelatedReferences { get; set; }
    }

    public class CarrierReference
    {
        [JsonPropertyName("reference")]
        public string Reference { get; set; }

        [JsonPropertyName("carrierName")]
        public string CarrierName { get; set; }

        [JsonPropertyName("trackingUrl")]
        public string TrackingUrl { get; set; }
    }

    public class Event
    {
        [JsonPropertyName("sequence")]
        public int Sequence { get; set; }

        [JsonPropertyName("eventDateTime")]
        public DateTime EventDateTime { get; set; }

        [JsonPropertyName("groupingLevel1Code")]
        public string GroupingLevel1Code { get; set; }

        [JsonPropertyName("groupingLevel2Code")]
        public string GroupingLevel2Code { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }

        [JsonPropertyName("location")]
        public string Location { get; set; }

        [JsonPropertyName("isEstimate")]
        public bool IsEstimate { get; set; }
    }
}
