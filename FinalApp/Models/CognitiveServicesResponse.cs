
namespace FinalApp.Models {
    using Newtonsoft.Json;
    using J = Newtonsoft.Json.JsonPropertyAttribute;
    using N = Newtonsoft.Json.NullValueHandling;

    public partial class CognitiveServicesResponse {
        [J("status")] public string Status { get; set; }
        [J("recognitionResult", NullValueHandling = N.Ignore)] public RecognitionResult RecognitionResult { get; set; }
    }

    public partial class RecognitionResult {
        [J("lines")] public Line[] Lines { get; set; }
    }

    public partial class Line {
        [J("boundingBox")] public long[] BoundingBox { get; set; }
        [J("text")] public string Text { get; set; }
        [J("words")] public Word[] Words { get; set; }
    }

    public partial class Word {
        [J("boundingBox")] public long[] BoundingBox { get; set; }
        [J("text")] public string Text { get; set; }
        [J("confidence", NullValueHandling = N.Ignore)] public string Confidence { get; set; }
    }

    public partial class CognitiveServicesResponse {
        public static CognitiveServicesResponse FromJson(string json) => JsonConvert.DeserializeObject<CognitiveServicesResponse>(json, Commons.StandardJsonConverter.Settings);
    }

    public static class Serialize {
        public static string ToJson(this CognitiveServicesResponse self) => JsonConvert.SerializeObject(self, Commons.StandardJsonConverter.Settings);
    }


}
