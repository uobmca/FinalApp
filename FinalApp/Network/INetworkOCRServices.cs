using System;
using System.Threading.Tasks;
using FinalApp.Models;

namespace FinalApp.Network {
    public interface INetworkOCRServices {
        Task<CognitiveServicesResponse> GetCognitiveServicesResponse(string operationId);
        Task<string> SendCognitiveServicesRequest(byte[] byteData, bool isHandWritten = true);
    }
}
