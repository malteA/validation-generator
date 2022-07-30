using Newtonsoft.Json;

namespace Project.Models.Attributes
{
    public class ValidationResponse
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("errorMessage")]
        public string ErrorMessage { get; set; }
    }
}
