using Newtonsoft.Json;
using Project.Models.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using ValidationResponse = Project.Models.Attributes.ValidationResponse;

namespace Project.Controllers
{
    public class ValidationController
    {

        /// <summary>
        /// Removes the Decorator at the end from a string
        /// </summary>
        /// <param name="name">Full String</param>
        /// <param name="decorator">Decorator wich will be removed</param>
        /// <returns>String without the Decorator</returns>
        private static string RemoveDecorator(string name, string decorator)
        {
            return name.Substring(name.Length - decorator.Length, decorator.Length).ToLower() == decorator
                ? name.Substring(0, name.Length - decorator.Length).ToLower()
                : name.ToLower();
        }

        /// <summary>
        /// Gets the Property Validators for a Model
        /// </summary>
        /// <param name="properties">Model Properties</param>
        /// <returns>Dictionary with Property Name and the applied attributes</returns>
        public static Dictionary<string, List<ValidationResponse>> GetModelPropertyValidators(
            PropertyInfo[] properties)
        {
            if (properties == null) throw new Exception("Parameter missing");
            var propertyDictionary = new Dictionary<string, List<ValidationResponse>>();
            foreach (var property in properties)
            {
                var attributes = new List<ValidationResponse>();
                var name = property.Name;

                foreach (var untypedAttribute in Attribute.GetCustomAttributes(property))
                {
                    var mapAttributeName =
                        new Func<string, string>(input => RemoveDecorator(input, "attribute"));
                    ValidationResponse validationAttribute = new()
                    {
                        Type = mapAttributeName.Invoke(untypedAttribute.GetType().Name)
                    };

                    var found = true;
                    switch (untypedAttribute)
                    {
                        case JsonPropertyAttribute attribute:
                            // Change C# property name to JSON property name
                            name = attribute.PropertyName;
                            found = false;
                            break;
                        case MinLengthAttribute attribute:
                            validationAttribute.Value = attribute.Length.ToString();
                            validationAttribute.ErrorMessage = attribute.ErrorMessage;
                            break;
                        case MaxLengthAttribute attribute:
                            validationAttribute.Value = attribute.Length.ToString();
                            validationAttribute.ErrorMessage = attribute.ErrorMessage;
                            break;
                        case RegularExpressionAttribute attribute:
                            // Some types, for example ZipCodeValidator, extend RegularExpressionAttribute
                            // Those types also get caught by this case, but the name does not match, thus:
                            // Types extending RegularExpressionAttribute shall be RegularExpressionAttribute to the client
                            validationAttribute.Type = mapAttributeName.Invoke(nameof(RegularExpressionAttribute));

                            validationAttribute.Value = attribute.Pattern;
                            validationAttribute.ErrorMessage = attribute.ErrorMessage;
                            break;
                        case RangeAttribute attribute:
                            validationAttribute.Value = $"{attribute.Minimum}:{attribute.Maximum}";
                            validationAttribute.ErrorMessage = attribute.ErrorMessage;
                            break;
                        case RequiredAttribute attribute:
                            validationAttribute.Value = true.ToString();
                            validationAttribute.ErrorMessage = attribute.ErrorMessage;
                            break;
                        case EmailAddressAttribute attribute:
                            validationAttribute.ErrorMessage = attribute.ErrorMessage;
                            break;
                        case CompareAttribute attribute:
                            validationAttribute.Value = attribute.OtherProperty;
                            validationAttribute.ErrorMessage = attribute.ErrorMessage;
                            break;
                        case FirstDayOfMonthValidation attribute:
                            validationAttribute.ErrorMessage = attribute.ErrorMessage;
                            break;
                        default:
                            found = false;
                            break;
                    }

                    if (found)
                        attributes.Add(validationAttribute);
                }

                propertyDictionary.Add(name, attributes);
            }

            return propertyDictionary;
        }
    }
}