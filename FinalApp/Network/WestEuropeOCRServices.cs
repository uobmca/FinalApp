using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using FinalApp.Models;

namespace FinalApp.Network {
    public class WestEuropeOCRServices : INetworkOCRServices {

        const string subscriptionKey = "d52564bdb77443deacdb5dba486a471a";
        const string uriBase = "https://westeurope.api.cognitive.microsoft.com/vision/v1.0";

        public async Task<string> SendCognitiveServicesRequest(byte[] byteData, bool isHandWritten = true) {

            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            // Request parameters
            queryString["handwriting"] = isHandWritten ? "true" : "false";
            var uri = string.Format("{0}/recognizeText?{1}", uriBase, queryString);

            HttpResponseMessage response;

            using (var content = new ByteArrayContent(byteData)) {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(uri, content);
            }

            response.EnsureSuccessStatusCode();

            if (response.Headers.TryGetValues("Operation-Location", out IEnumerable<string> values)) {
                if(values.First() is string operationId) {
                    return operationId;
                }
            }

            return null;
        }

        public async Task<CognitiveServicesResponse> GetCognitiveServicesResponse(string operationId) {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            var uri = operationId;
            var response = await client.GetAsync(uri);

            response.EnsureSuccessStatusCode();

            var contentString = await response.Content.ReadAsStringAsync();
            if (contentString == null) {
                return null;
            }

            return CognitiveServicesResponse.FromJson(contentString);
        }
    }
}
