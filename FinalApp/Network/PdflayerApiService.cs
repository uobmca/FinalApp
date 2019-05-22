

using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace FinalApp.Network {
    public class PdflayerApiService : DocumentsUtilsApiService {

        private const string kUriBase = "http://api.pdflayer.com/api/convert";
        private const string kAccessKey = "dfc14e4d4ee416ab0a5db23458d57e54";
        private const bool kIsTestMode = true;

        public async Task<DocumentsApiServiceResponse> ConvertHtmlToPdf(string htmlContent) {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request parameters
            queryString["access_key"] = kAccessKey;
            if (kIsTestMode) {
                queryString["test"] = "1";
            }

            var uri = string.Format("{0}?{1}", kUriBase, queryString);

            var parameters = new List<KeyValuePair<string, string>> {
                new KeyValuePair<string, string>("document_html", htmlContent)
            };

            HttpContent content = new FormUrlEncodedContent(parameters);
            HttpResponseMessage response;

            response = await client.PostAsync(uri,content);

            if(response.IsSuccessStatusCode) {
                var bodyData = await response.Content.ReadAsByteArrayAsync();
                if (bodyData != null) {
                    return new DocumentsApiServiceResponse { FileBytesData = bodyData };
                }
            }

            return DocumentsApiServiceResponse.Failure();

        }
    }
}
