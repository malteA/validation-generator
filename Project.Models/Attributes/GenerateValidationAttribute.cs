using System.ComponentModel.DataAnnotations;

namespace Project.Models.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class GenerateValidationAttribute : ValidationAttribute
    {
    }
}
