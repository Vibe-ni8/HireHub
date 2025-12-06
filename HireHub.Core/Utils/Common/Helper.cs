using System.Globalization;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json.Linq;

namespace HireHub.Core.Utils.Common;

public static class Helper
{
    public static T Map<F, T>(F fromData) where F : class where T : class, new()
    {
        return Map(fromData, new T());
    }

    public static T Map<F, T>(F fromData, T toData) where F : class where T : class
    {
        Type fromType = typeof(F);
        Type toType = typeof(T);

        return Map(fromData, fromType, toData, toType);
    }

    private static T Map<F, T>(F fromData, Type fromType, T toData, Type toType) where F : class where T : class
    {
        foreach (var fromProperty in fromType.GetProperties())
        {
            var toProperty = toType.GetProperty(fromProperty.Name);
            if (toProperty == null) continue;
            if (!toProperty.CanWrite) continue;

            object? value = fromProperty.GetValue(fromData);

            if (fromProperty.PropertyType.IsClass && fromProperty.PropertyType != typeof(string))
            {
                var fromPropertyData = fromProperty.GetValue(fromData);
                var toPropertyData = toProperty.GetValue(toData) ?? Activator.CreateInstance(toProperty.PropertyType);

                value = (toPropertyData != null && fromPropertyData != null) ? 
                    Map(fromPropertyData, fromProperty.PropertyType, toPropertyData, toProperty.PropertyType) : 
                    null;
            }

            toProperty.SetValue(toData, value);
        }

        return toData;
    }

    public static readonly ValueConverter<TimeOnly, string> TimeConverter = new
    (
        v => v.ToString("HH:mm", CultureInfo.InvariantCulture), // TimeOnly → string
        v => TimeOnly.ParseExact(v, new string[] { "HH:mm", "hh:mm tt" }) // string → TimeOnly
    );
}
