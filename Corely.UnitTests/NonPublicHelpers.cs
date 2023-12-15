namespace Corely.UnitTests
{
    using System;
    using System.Linq;
    using System.Reflection;

    namespace AB.TestBase
    {
        public static class NonPublicHelpers
        {
            private const BindingFlags BINDING_FLAGS = BindingFlags.Instance
                    | BindingFlags.NonPublic
                    | BindingFlags.FlattenHierarchy
                    | BindingFlags.Static;
            public static T? InvokeNonPublicMethod<T>(object classInstance, string methodName)
            {
                var methodInfo = GetNonPublicMethod(classInstance, methodName)
                    ?? throw new NullReferenceException($"Method {methodName} not found in type {classInstance.GetType().Name}");
                return (T?)methodInfo.Invoke(classInstance, null);
            }

            public static T? InvokeNonPublicMethod<T>(object classInstance, string methodName, params object[] args)
            {
                var methodInfo = GetNonPublicMethod(classInstance, methodName)
                    ?? throw new NullReferenceException($"Method {methodName} not found in type {classInstance.GetType().Name}");
                return (T?)methodInfo.Invoke(classInstance, args);
            }

            /// <summary>
            /// Use this when method is overloaded
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="classInstance"></param>
            /// <param name="methodName"></param>
            /// <param name="args"></param>
            /// <returns></returns>
            public static T? InvokeNonPublicMethod<T>(object classInstance, string methodName, params (Type, object)[] args)
            {
                var methodInfo = GetNonPublicMethod(classInstance, methodName, args.Select(a => a.Item1).ToArray())
                    ?? throw new NullReferenceException($"Method {methodName} not found in type {classInstance.GetType().Name}");
                return (T?)methodInfo.Invoke(classInstance, args.Select(a => a.Item2).ToArray());
            }

            private static MethodInfo? GetNonPublicMethod(object classInstance, string methodName)
            {
                var methodInfo = classInstance.GetType().GetMethod(methodName, BINDING_FLAGS)
                    // This is mostly for cases where class is wrapped for unit testing
                    ?? classInstance.GetType().BaseType?.GetMethod(methodName, BINDING_FLAGS);

                return methodInfo;
            }

            private static MethodInfo? GetNonPublicMethod(object classInstance, string methodName, params Type[] paramTypes)
            {
                var methodInfo = classInstance.GetType().GetMethod(methodName, BINDING_FLAGS, paramTypes)
                    ?? classInstance.GetType().BaseType?.GetMethod(methodName, BINDING_FLAGS, paramTypes);

                return methodInfo;
            }

            public static void SetNonPublicProperty(object instance, string propName, object value, bool setBackingFieldFallback = true)
            {
                var instanceType = instance.GetType();
                var prop = instanceType.GetProperty(propName, BINDING_FLAGS);

                while (prop == null && instanceType.BaseType != null)
                {
                    instanceType = instanceType.BaseType;
                    prop = instanceType.GetProperty(propName, BINDING_FLAGS);
                }

                try
                {
                    if (prop == null)
                    {
                        throw new NullReferenceException($"Property {propName} not found in type {instanceType.Name}");
                    }
                    prop.SetValue(instance, value);
                }
                catch (ArgumentException)
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
                var field = instanceType.GetField(fieldName, BINDING_FLAGS);

                while (field == null && instanceType.BaseType != null)
                {
                    instanceType = instanceType.BaseType;
                    field = instanceType.GetField(fieldName, BINDING_FLAGS);
                }

                if (field == null)
                {
                    throw new NullReferenceException($"Field {fieldName} not found in type {instanceType.Name}");
                }
                field.SetValue(instance, value);
            }
        }
    }
}
