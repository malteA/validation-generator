using Newtonsoft.Json;
using Project.Models.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    [GenerateValidation]
    public class ShouldBeParsedToValidation
    {
        [JsonProperty("name")]
        [Required(ErrorMessage = "Name is required")]
        [RegularExpression(@"([A-Z])\w+", ErrorMessage = "Name can only contain letters")]
        public string Name { get; set; }

        [JsonProperty("date")]
        [FirstDayOfMonthValidation]
        public DateTime Date { get; set; }
    }
}