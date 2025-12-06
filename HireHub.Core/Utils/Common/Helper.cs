using System.Globalization;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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

        foreach (var fromProperty in fromType.GetProperties())
        {
            var toProperty = toType.GetProperty(fromProperty.Name);
            if (toProperty == null) continue;
            if (!toProperty.CanWrite) continue;

            if ( fromProperty.PropertyType.IsValueType && (fromProperty.PropertyType == toProperty.PropertyType) )
                toProperty.SetValue(toData, fromProperty.GetValue(fromData));

            if (fromProperty.PropertyType.IsClass)
            {
                var fromPropertyData = fromProperty.GetValue(fromData);
                var toPropertyData = toProperty.GetValue(toData) ?? Activator.CreateInstance(toProperty.PropertyType);

                var value = (toPropertyData != null && fromPropertyData != null) ? Map(fromPropertyData, toPropertyData) : null;

                toProperty.SetValue(toData, value);
            }
        }

        return toData;
    }

    public static readonly ValueConverter<TimeOnly, string> TimeConverter = new
    (
        v => v.ToString("hh:mm tt", CultureInfo.InvariantCulture), // TimeOnly → string
        v => TimeOnly.ParseExact(v, "hh:mm tt") // string → TimeOnly
    );
}
