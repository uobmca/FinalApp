using System;

namespace FinalApp.Models {
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using J = Newtonsoft.Json.JsonPropertyAttribute;
    using R = Newtonsoft.Json.Required;
    using N = Newtonsoft.Json.NullValueHandling;

    public partial class OcrApiResponse {
        [J("language")] public string Language { get; set; }
        [J("textAngle")] public double TextAngle { get; set; }
        [J("orientation")] public string Orientation { get; set; }
        [J("regions")] public Region[] Regions { get; set; }
    }

    public partial class Region {
        [J("boundingBox")] public string BoundingBox { get; set; }
        [J("lines")] public Line[] Lines { get; set; }
    }

    public partial class Line {
        [J("boundingBox")] public string BoundingBox { get; set; }
        [J("words")] public Word[] Words { get; set; }
    }

    public partial class Word {
        [J("boundingBox")] public string BoundingBox { get; set; }
        [J("text")] public string Text { get; set; }
    }

    public partial class OcrApiResponse {
        public static OcrApiResponse FromJson(string json) => JsonConvert.DeserializeObject<OcrApiResponse>(json, FinalApp.Models.Converter.Settings);
    }

    public static class Serialize {
        public static string ToJson(this OcrApiResponse self) => JsonConvert.SerializeObject(self, FinalApp.Models.Converter.Settings);
    }

    internal static class Converter {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
