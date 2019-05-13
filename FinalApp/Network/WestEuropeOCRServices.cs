using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using FinalApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FinalApp.Network {
    public class WestEuropeOCRServices : INetworkOCRServices {

        const string subscriptionKey = "d52564bdb77443deacdb5dba486a471a";
        const string uriBase = "https://westeurope.api.cognitive.microsoft.com/vision/v1.0";

        public async Task<CognitiveServicesRequestResponse> SendCognitiveServicesRequest(byte[] byteData, bool isHandWritten = true) {

            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            // Request parameters
            queryString["handwriting"] = isHandWritten ? "true" : "false";
            var uri = string.Format("{0}/recognizeText?{1}", uriBase, queryString);

            HttpResponseMessage response;
            Metadata metadata = new Metadata();
            using (var content = new ByteArrayContent(byteData)) {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(uri, content);

            }

            using (var content = new ByteArrayContent(byteData)) {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                HttpResponseMessage metadataResponse = await client.PostAsync("https://westeurope.api.cognitive.microsoft.com/vision/v2.0/tag?language=en", content);
                if (metadataResponse.IsSuccessStatusCode) {
                    var metadataStr = await metadataResponse.Content.ReadAsStringAsync();
                    var dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(metadataStr);
                    JObject metadataDict = dictionary["metadata"] as JObject;
                    metadata.Width = (int)metadataDict["width"];
                    metadata.Height = (int)metadataDict["height"];
                }
            }

            if (response.Headers.TryGetValues("Operation-Location", out IEnumerable<string> values)) {
                if(values.First() is string operationId) {
                    return new CognitiveServicesRequestResponse { 
                        ImageMetadata = metadata,
                        OperationId = operationId
                    };
                }
            }

            return null;
        }

        public async Task<CognitiveServicesResponse> GetCognitiveServicesResponse(string operationId) {
            var client = new HttpClient();

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            var uri = operationId;
            var response = await client.GetAsync(uri);
            var contentString = await response.Content.ReadAsStringAsync();

            if (contentString == null) {
                return null;
            }

            return CognitiveServicesResponse.FromJson(contentString);
        }
    }
}
