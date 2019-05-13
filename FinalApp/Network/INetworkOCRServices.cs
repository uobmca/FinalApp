using System;
using System.Threading.Tasks;
using FinalApp.Models;

namespace FinalApp.Network {

    public interface INetworkOCRServices {
        Task<CognitiveServicesResponse> GetCognitiveServicesResponse(string operationId);
        Task<CognitiveServicesRequestResponse> SendCognitiveServicesRequest(byte[] byteData, bool isHandWritten = true);
    }

    public class CognitiveServicesRequestResponse {
        public string OperationId { get; set; }
        public Metadata ImageMetadata { get; set; }
    }
}
