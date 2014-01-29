using System;
using System.Runtime.Serialization;
using System.Reflection;

namespace GpodderLib.Utils
{
    internal static class EnumUtils
    {
        public static string GetValueName(this Enum enumValue)
        {
            var enumType = enumValue.GetType();
            var name = Enum.GetName(enumType, enumValue);
            var enumMemberAttribute = enumType.GetRuntimeField(name).GetCustomAttribute<EnumMemberAttribute>();
            return enumMemberAttribute.Value ?? name;
        }

        public static T GetEnumValue<T>(this string name)
        {
            return (T)Enum.Parse(typeof(T), name, true);
        }
    }
}
