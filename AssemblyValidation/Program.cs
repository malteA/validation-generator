using Project.Models.Attributes;
using System.Reflection;
using System.Text.Json;

try
{
    const string modelPath = "Project.Models.ShouldBeParsedToValidation";
    const string validationControllerPath = "Project.Controllers.ValidationController";
    var modelsAssembly = Assembly.LoadFrom("Project.Models.dll");
    var controllerAssembly = Assembly.LoadFrom("Project.Controllers.dll");

    IEnumerable<Type>? types = from assembly in AppDomain.CurrentDomain.GetAssemblies()
                            from type in assembly.GetTypes()
                            where !type.IsDefined(typeof(GenerateValidationAttribute))
                            select type;

    PropertyInfo[] properties = GetModelAsProperties(modelsAssembly, modelPath);
    if (!properties.Any())
        return;

    Type? controllerType = controllerAssembly.GetType(validationControllerPath);

    MethodInfo? methodInfo = controllerType?.GetMethod("GetModelPropertyValidators");
    object? validationControllerInstance = Activator.CreateInstance(controllerType);

    object? result = methodInfo?.Invoke(validationControllerInstance, new object[] { properties });
    Console.WriteLine(JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));

}
catch (Exception ex)
{
    Console.WriteLine(ex.Message, "Exception");
}

static PropertyInfo[] GetModelAsProperties(Assembly assembly, string pathToModel)
{
    Type? type = assembly.GetType(pathToModel, true);

    return type?.GetProperties() ?? Array.Empty<PropertyInfo>();
}
