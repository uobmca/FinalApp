using System;
using System.Threading.Tasks;

namespace FinalApp.Network {

    public class DocumentsApiServiceResponse { 
        public byte[] FileBytesData { get; set; }
        public bool IsSuccess { get; set; } = true;

        public static DocumentsApiServiceResponse Failure() {
            return new DocumentsApiServiceResponse { IsSuccess = false };
        }
    }

    public interface DocumentsUtilsApiService {
        Task<DocumentsApiServiceResponse> ConvertHtmlToPdf(string htmlContent);
    }
}
