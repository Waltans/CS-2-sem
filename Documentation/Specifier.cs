using System;
using System.Linq;
using System.Reflection;

namespace Documentation;

public class Specifier<T> : ISpecifier
{
    private readonly Type _type;

    public Specifier()
    {
        _type = typeof(T);
    }

    public string GetApiDescription()
    {
        var attribute = _type.GetCustomAttributes<ApiDescriptionAttribute>()
            .FirstOrDefault();

        return attribute?.Description;
    }

    public string[] GetApiMethodNames()
    {
        return _type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Where(m => m.GetCustomAttributes<ApiMethodAttribute>().Any())
            .Select(m => m.Name)
            .ToArray();
    }

    public string GetApiMethodDescription(string methodName)
    {
        var method = _type.GetMethod(methodName);
        if (method == null || !method.GetCustomAttributes<ApiMethodAttribute>().Any())
            return null;

        var attribute = method.GetCustomAttributes<ApiDescriptionAttribute>()
            .FirstOrDefault();

        return attribute?.Description;
    }

    public string[] GetApiMethodParamNames(string methodName)
    {
        var method = _type.GetMethod(methodName);
        if (method == null || !method.GetCustomAttributes<ApiMethodAttribute>().Any())
            return null;

        return method.GetParameters()
            .Select(p => p.Name)
            .ToArray();
    }

    public string GetApiMethodParamDescription(string methodName, string paramName)
    {
        var method = _type.GetMethod(methodName);
        if (method == null || !method.GetCustomAttributes<ApiMethodAttribute>().Any())
            return null;

        var parameter = method.GetParameters()
            .SingleOrDefault(p => p.Name == paramName && p.GetCustomAttributes<ApiDescriptionAttribute>().Any());

        return parameter?.GetCustomAttributes<ApiDescriptionAttribute>().FirstOrDefault()?.Description;
    }

    public ApiParamDescription GetApiMethodParamFullDescription(string methodName, string paramName)
    {
        var result = new ApiParamDescription { ParamDescription = new CommonDescription(paramName) };
        var method = _type.GetMethod(methodName);
        if (method == null || !method.GetCustomAttributes<ApiMethodAttribute>().Any())
            return result;

        var parameter = method.GetParameters().SingleOrDefault(p => p.Name == paramName);
        if (parameter == null)
            return result;

        ChangeResult(result, parameter);

        return result;
    }

    private bool ChangeResult(ApiParamDescription result, ParameterInfo param)
    {
        var change = false;

        var attributes = param.GetCustomAttributes();
        foreach (var attribute in attributes)
            if (attribute is ApiDescriptionAttribute descriptionAttribute)
            {
                result.ParamDescription.Description = descriptionAttribute.Description;
                change = true;
            }
            else if (attribute is ApiIntValidationAttribute validationAttribute)
            {
                result.MaxValue = validationAttribute.MaxValue;
                result.MinValue = validationAttribute.MinValue;
                change = true;
            }
            else if (attribute is ApiRequiredAttribute requiredAttribute)
            {
                result.Required = requiredAttribute.Required;
                change = true;
            }

        return change;
    }

    public ApiMethodDescription GetApiMethodFullDescription(string methodName)
    {
        var method = _type.GetMethod(methodName);
        if (method == null || !method.GetCustomAttributes().Any(g => g is ApiMethodAttribute)) return null;

        var result = new ApiMethodDescription();
        result.MethodDescription = new CommonDescription(methodName, GetApiMethodDescription(methodName));
        result.ParamDescriptions = GetApiMethodParamNames(methodName)
            .Select(n => GetApiMethodParamFullDescription(methodName, n)).ToArray();

        var param = method.ReturnParameter;

        var returnDescription = new ApiParamDescription();
        returnDescription.ParamDescription = new CommonDescription();

        if (param == null) return result;
        if (ChangeResult(returnDescription, param))
            result.ReturnDescription = returnDescription;
        return result;
    }
}