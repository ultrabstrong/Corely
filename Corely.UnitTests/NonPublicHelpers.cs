using System.Reflection;

namespace Corely.UnitTests
{
    internal static class NonPublicHelpers
    {
        public static T? InvokeNonPublicMethod<T>(object classInstance, string methodName, params object[] args)
        {
            var result = classInstance
                ?.GetType()
                ?.GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic)
                ?.Invoke(classInstance, args);

            return (T?)result;
        }

        public static void SetNonPublicProperty(object instance, string propName, object value,
            bool setBackingFieldFallback = true)
        {
            var instanceType = instance.GetType();
            var prop = instanceType
                .GetProperty(propName, BindingFlags.Instance | BindingFlags.NonPublic);

            while (prop == null && instanceType.BaseType != null)
            {
                instanceType = instanceType.BaseType;
                prop = instanceType
                    .GetProperty(propName, BindingFlags.Instance | BindingFlags.NonPublic);
            }

            try
            {
                ArgumentNullException.ThrowIfNull(prop, nameof(prop));
                prop.SetValue(instance, value);
            }
            catch (ArgumentNullException)
            {
                if (setBackingFieldFallback)
                {
                    string fieldname = $"<{propName}>k__BackingField";
                    SetNonPublicField(instance, fieldname, value);
                }
                else
                {
                    throw;
                }
            }
        }

        public static void SetNonPublicField(object instance, string fieldName, object value)
        {
            var instanceType = instance.GetType();
            var field = instanceType
                .GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);

            while (field == null && instanceType.BaseType != null)
            {
                instanceType = instanceType.BaseType;
                field = instanceType
                    .GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            }

            ArgumentNullException.ThrowIfNull(field, nameof(field));
            field.SetValue(instance, value);
        }
    }
}
