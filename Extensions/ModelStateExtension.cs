using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BlogVisualStudio.Extensions;

public static class ModelStateExtension
{
    public static List<string> GetErrors(this ModelStateDictionary modelState)
    {
        List<string> errorList = new();
        foreach (var item in modelState.Values)
            errorList.AddRange(item.Errors.Select(error => error.ErrorMessage));
        return errorList;
    }
}