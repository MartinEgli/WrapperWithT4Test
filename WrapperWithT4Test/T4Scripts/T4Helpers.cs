using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using static System.String;

namespace WrapperWithT4Test.T4Scripts
{
    public static class T4Helpers
    {
        private static readonly Dictionary<Type, string> typeMapDictionary = new Dictionary<Type, string>();

        public static Regex CanonicalNamePattern =>
            new Regex(@"^([^`]+)`?.*$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /* ---------------------------------- HELPERS ---------------------------------- */

        public static void AddTypesToMap(Type[] sourceTypes, string proxySuffix)
        {
            foreach (var targetType in sourceTypes)
                if (!typeMapDictionary.ContainsKey(targetType))
                    typeMapDictionary.Add(targetType, GetProxyTypeShortName(targetType, proxySuffix));
        }

        public static IList<CustomAttributeData> FilterAttributes(IList<CustomAttributeData> attributes)
        {
            var list = attributes
                .Where(a => !(a.AttributeType == typeof(OptionalAttribute)))
                .Where(a => !(a.AttributeType == typeof(OutAttribute)))
                .Where(a => !(a.AttributeType == typeof(_Attribute)))
                .ToList();
            return list;
        }

        public static Type[] FilterInterfaces(Type[] attributes)
        {
            var list = attributes
                .Where(a => !(a == typeof(_Attribute)))
                .ToArray();
            return list;
        }

        public static string GetArgPrefix(ParameterInfo parameterInfo)
        {
            if (parameterInfo.ParameterType.IsByRef && parameterInfo.IsOut)
                return "out ";
            if (parameterInfo.ParameterType.IsByRef)
                return "ref ";
            return Empty;
        }

        public static string GetCanonicalName(Type type)
        {
            return CanonicalNamePattern.Match(type.FullName).Groups[1].Value;
        }

        public static ConstructorInfo[] GetConstructorInfos(Type i)
        {
            return i.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
        }

        public static EventInfo[] GetEventInfos(Type i)
        {
            return i.GetEvents(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
        }

        public static EventInfo[] GetInterfaceEventInfos(Type i)
        {
            return i.GetEvents(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
        }

        public static MethodInfo[] GetInterfaceMethodInfos(Type i)
        {
            return i.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Where(m => !m.IsSpecialName).ToArray();
        }

        public static PropertyInfo[] GetInterfacePropertyInfos(Type i)
        {
            return i.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
        }

        public static MethodInfo[] GetMethodInfos(Type i)
        {
            return i.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Where(m => !m.IsSpecialName).ToArray();
        }

        public static PropertyInfo[] GetPropertyInfos(Type i)
        {
            return i.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
        }

        public static string GetProxyTypeShortName(Type type, string proxySuffix)
        {
            return GetCanonicalName(type).Split('.').Last().Split('+').Last() + proxySuffix;
        }

        public static EventInfo[] GetStaticEventInfos(Type i)
        {
            return i.GetEvents(BindingFlags.Static | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Where(m => !m.IsSpecialName).ToArray();
        }

        public static MethodInfo[] GetStaticMethodInfos(Type i)
        {
            return i.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Where(m => !m.IsSpecialName).ToArray();
        }

        public static PropertyInfo[] GetStaticPropertyInfos(Type i)
        {
            return i.GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Where(m => !m.IsSpecialName).ToArray();
        }

        public static bool HasFieldAttributes(FieldInfo fieldInfo)
        {
            var attributes = CustomAttributeData.GetCustomAttributes(fieldInfo);
            var list = FilterAttributes(attributes);
            return list.Any();
        }

        public static bool HasPropertyGetter(PropertyInfo propInfo)
        {
            return propInfo.CanRead;
        }

        public static bool HasPropertySetter(PropertyInfo propInfo)
        {
            var m = propInfo.GetSetMethod();
            return m != null && m.IsPublic;
        }

        public static bool HasTypeAttributes(Type type)
        {
            var attributes = CustomAttributeData.GetCustomAttributes(type);
            var list = FilterAttributes(attributes);
            return list.Any();
        }

        public static bool IsVoid(Type type)
        {
            return ReferenceEquals(type, typeof(void));
        }

        public static string WriteArray(Type type)
        {
            var str = new StringBuilder();
            var t = type;
            var depth = 1;
            while ((t = t.GetElementType()) != null && t.IsArray) depth++;

            str.Append(WriteType(t));

            while (depth-- > 0) str.Append("[]");
            return str.ToString();
        }

        public static string WriteConstructorSignature(ConstructorInfo constructorInfo, string name)
        {
            var parameters = constructorInfo.GetParameters();
            var str = new StringBuilder();
            str.Append(WriteUnsafe(constructorInfo));
            str.Append(name);
            str.Append("(");
            str.Append(WriteMethodParameters(parameters));
            str.Append(")");
            return str.ToString();
        }

        public static string WriteDelegate(Type type, string proxySuffix)
        {
            var str = new StringBuilder();
            var method = type.GetMethod("Invoke");
            str.Append("delegate ");
            str.Append(WriteWrapperType(method.ReturnType));
            str.Append(" ");
            str.Append(GetProxyTypeShortName(type, proxySuffix));
            str.Append(" (");
            var parameters = method.GetParameters();
            str.Append(Join(", ",
                parameters.Select(param => $"{WriteWrapperType(param.ParameterType)} {param.Name}")));
            str.Append(");");
            return str.ToString();
        }

        public static string WriteEventSignature(EventInfo eventInfo)
        {
            var str = new StringBuilder();
            str.Append("event ");
            str.Append(WriteWrapperType(eventInfo.EventHandlerType));
            str.Append(" ");
            str.Append(eventInfo.Name);
            return str.ToString();
        }

        public static string WriteEventWrapperSubscription(EventInfo eventInfo, string i)
        {
            var parameters = eventInfo.EventHandlerType.GetMethod("Invoke").GetParameters();
            var eventArgs = Join(", ", parameters.Select(a => a.Name));
            var str = new StringBuilder();
            str.Append(i);
            str.Append(".");
            str.Append(eventInfo.Name);
            str.Append(" += (");
            str.Append(eventArgs);
            str.Append(") => { if (this.");
            str.Append(eventInfo.Name);
            str.Append(" != null) this.");
            str.Append(eventInfo.Name);
            str.Append("(");
            str.Append(eventArgs);
            str.Append("); };");
            return str.ToString();
        }

        public static string WriteFieldAttributes(FieldInfo fieldInfo)
        {
            var attributes = CustomAttributeData.GetCustomAttributes(fieldInfo);
            var list = FilterAttributes(attributes);
            if (list.Any())
                return
                    $"[{Join(", ", list.Select(a => a.AttributeType.ToString()).Select(s => s.EndsWith("Attribute") ? s.Substring(0, s.LastIndexOf("Attribute")) : s))}] ";
            return "";
        }

        public static string WriteGeneric(Type type)
        {
            var str = new StringBuilder();
            str.Append(GetCanonicalName(type));
            str.Append("<");
            str.Append(type.GenericTypeArguments.Select(a => Join(", ", WriteType(a))));
            str.Append(">");
            return str.ToString();
        }

        public static string WriteInstanceDefinition(ConstructorInfo constructorInfo, string instantName)
        {
            var parameters = constructorInfo.GetParameters();
            var type = constructorInfo.DeclaringType;
            var str = new StringBuilder();
            str.Append("this.");
            str.Append(instantName);
            str.Append(" = new ");
            str.Append(WriteType(type));
            str.Append("(");
            str.Append(WriteMethodParameters(parameters));
            str.Append(");");
            return str.ToString();
        }

        public static string WriteInterfaceEvent(EventInfo eventInfo)
        {
            return WriteEventSignature(eventInfo) + ";";
        }

        public static string WriteInterfaceEventSignature(EventInfo eventInfo, string interfaceName)
        {
            var str = new StringBuilder();
            str.Append("event ");
            str.Append(WriteWrapperType(eventInfo.EventHandlerType));
            str.Append(" ");
            str.Append(interfaceName);
            str.Append(".");
            str.Append(eventInfo.Name);
            return str.ToString();
        }

        public static string WriteInterfaceIndexer(PropertyInfo propInfo)
        {
            var indexers = propInfo.GetIndexParameters();
            var str = new StringBuilder();
            str.Append(WriteUnsafe(propInfo));
            str.Append(WriteWrapperType(propInfo.PropertyType));
            str.Append(" this[");
            str.Append(Join(", ", indexers.Select(i => $"{WriteWrapperType(i.ParameterType)} {i.Name}")));
            str.Append("]");
            str.Append(" {");
            if (propInfo.CanRead) str.Append(" get;");
            var methodInfo = propInfo.GetSetMethod();
            if (methodInfo != null && methodInfo.IsPublic) str.Append(" set;");
            str.Append(" }");
            return str.ToString();
        }

        public static string WriteInterfaceMethod(MethodInfo methodInfo)
        {
            return WriteMethodSignature(methodInfo) + ";";
        }

        public static string WriteInterfaceMethodBody(MethodInfo methodInfo, string typeName, string interfaceName)
        {
            return WriteMethodSignature(methodInfo) + ";";
        }

        public static string WriteInterfaceMethodSignature(MethodInfo methodInfo, string interfaceName)
        {
            var parameters = methodInfo.GetParameters();
            var str = new StringBuilder();
            str.Append(WriteUnsafe(methodInfo));
            if (methodInfo.ReturnParameter != null)
            {
                str.Append(WriteWrapperType(methodInfo.ReturnParameter.ParameterType));
                str.Append(" ");
            }

            str.Append(interfaceName);
            str.Append(".");
            str.Append(methodInfo.Name);
            str.Append("(");
            str.Append(WriteMethodParameters(parameters));
            str.Append(")");
            return str.ToString();
        }

        public static string WriteInterfaceProperty(PropertyInfo propertyInfo)
        {
            var str = new StringBuilder();
            str.Append(WritePropertyHead(propertyInfo));
            str.Append(" {");
            if (propertyInfo.CanRead) str.Append(" get;");
            var m = propertyInfo.GetSetMethod();
            if (m != null && m.IsPublic) str.Append(" set;");
            str.Append(" }");
            return str.ToString();
        }

        public static string WriteInterfacePropertyGetter(PropertyInfo propertyInfo, string wrappedInstance,
            string interfaceName)
        {
            var str = new StringBuilder();
            if (propertyInfo.CanRead)
            {
                str.Append("get { return ");
                str.Append(wrappedInstance);
                str.Append(".");
                str.Append(propertyInfo.Name);
                str.Append("; }");
            }

            return str.ToString();
        }

        public static string WriteInterfacePropertyHead(PropertyInfo propertyInfo, string interfaceName)
        {
            var str = new StringBuilder();
            str.Append(WriteUnsafe(propertyInfo));
            str.Append(WriteWrapperType(propertyInfo.PropertyType));
            str.Append(" ");
            str.Append(interfaceName);
            str.Append(".");
            str.Append(propertyInfo.Name);
            return str.ToString();
        }

        public static string WriteInterfacePropertyIndexerGetter(PropertyInfo propertyInfo, string wrappedInstance,
            string interfaceName)
        {
            var indexers = propertyInfo.GetIndexParameters();
            var indexes = Join(", ", indexers.Select(i => i.Name));
            var str = new StringBuilder();
            if (propertyInfo.CanRead)
            {
                str.Append("get { return ");
                str.Append(wrappedInstance);
                str.Append("[");
                str.Append(indexes);
                str.Append("]; }");
            }

            return str.ToString();
        }

        public static string WriteInterfacePropertyIndexerHead(PropertyInfo propertyInfo, string interfaceName)
        {
            var indexers = propertyInfo.GetIndexParameters();
            var str = new StringBuilder();
            str.Append(WriteUnsafe(propertyInfo));
            str.Append(WriteWrapperType(propertyInfo.PropertyType));
            str.Append(" ");
            str.Append(interfaceName);
            str.Append(".this[");
            str.Append(Join(", ", indexers.Select(i => $"{WriteWrapperType(i.ParameterType)} {i.Name}")));
            str.Append("]"); return str.ToString();
        }

        public static string WriteInterfacePropertyIndexerSetter(PropertyInfo propertyInfo, string wrappedInstance,
            string interfaceName)
        {
            var indexers = propertyInfo.GetIndexParameters();
            var indexes = Join(", ", indexers.Select(i => i.Name));
            var str = new StringBuilder();
            var m = propertyInfo.GetSetMethod();
            if (m != null && m.IsPublic)
            {
                str.Append("set { ");
                str.Append(wrappedInstance);
                str.Append("[");
                str.Append(indexes);
                str.Append("] = value; }");
            }

            return str.ToString();
        }

        public static string WriteInterfacePropertySetter(PropertyInfo propertyInfo, string wrappedInstance,
                    string interfaceName)
        {
            var str = new StringBuilder();
            var m = propertyInfo.GetSetMethod();
            if (m != null && m.IsPublic)
            {
                str.Append("set { ");
                str.Append(wrappedInstance);
                str.Append(".");
                str.Append(propertyInfo.Name);
                str.Append(" = value; }");
            }

            return str.ToString();
        }

        public static string WriteMethod(MethodInfo methodInfo, string source)
        {
            var str = new StringBuilder();
            var parameters = methodInfo.GetParameters();
            var returnType = methodInfo.ReturnParameter.ParameterType;
            if (!IsVoid(returnType)) str.Append("return ");
            if (returnType.IsSubclassOf(typeof(Delegate)))
            {
                str.Append("new ");
                str.Append(WriteWrapperType(returnType));
                str.Append("(");
            }

            str.Append(source);
            str.Append(".");
            str.Append(methodInfo.Name);
            str.Append("(");
            str.Append(Join(", ", parameters.Select(p =>
                {
                    var s = new StringBuilder();
                    var type = p.ParameterType;

                    if (type.IsSubclassOf(typeof(Delegate)))
                    {
                        s.Append("new ");
                        s.Append(WriteType(type));
                        s.Append("(");
                    }

                    s.Append(WriteParamName(p));
                    if (type.IsSubclassOf(typeof(Delegate))) s.Append(")");
                    return s;
                }
            )));
            if (returnType.IsSubclassOf(typeof(Delegate))) str.Append(")");
            str.Append(");");
            return str.ToString();
        }

        public static string WriteMethodParameter(ParameterInfo parameterInfo)
        {
            var attributes = CustomAttributeData.GetCustomAttributes(parameterInfo);
            var list = FilterAttributes(attributes);
            var str = new StringBuilder();
            str.Append(WriteUnsafe(parameterInfo));
            str.Append(WriteParameterAttributes(list.ToArray()));
            str.Append(WriteParamType(parameterInfo));
            str.Append(" ");
            str.Append(parameterInfo.Name);
            if (parameterInfo.IsOptional)
            {
                str.Append(" = ");
                if (parameterInfo.ParameterType == typeof(bool))
                    str.Append(parameterInfo.DefaultValue.ToString().ToLower());
                else
                    str.Append(parameterInfo.DefaultValue);
            }

            return str.ToString();
        }

        public static string WriteMethodParameters(ParameterInfo[] parameterInfos)
        {
            return Join(", ", parameterInfos.Select(WriteMethodParameter));
        }

        public static string WriteMethodSignature(MethodInfo methodInfo)
        {
            var parameters = methodInfo.GetParameters();
            var str = new StringBuilder();
            str.Append(WriteUnsafe(methodInfo));
            if (methodInfo.ReturnParameter != null)
            {
                str.Append(WriteWrapperType(methodInfo.ReturnParameter.ParameterType));
                str.Append(" ");
            }

            str.Append(methodInfo.Name);
            str.Append("(");
            str.Append(WriteMethodParameters(parameters));
            str.Append(")");
            return str.ToString();
        }
        public static string WriteParameterAttribute(CustomAttributeData customAttributeData)
        {
            var s = customAttributeData.AttributeType.ToString();
            return s.EndsWith("Attribute") ? s.Substring(0, s.LastIndexOf("Attribute")) : s;
        }

        public static string WriteParameterAttributes(CustomAttributeData[] attributes)
        {
            var str = new StringBuilder();
            var list = FilterAttributes(attributes);
            if (list.Any())
            {
                str.Append("[");
                str.Append(Join(", ", list.Select(WriteParameterAttribute)));
                str.Append("] ");
            }

            return str.ToString();
        }

        public static string WriteParamName(ParameterInfo parameterInfo)
        {
            var str = new StringBuilder();
            str.Append(GetArgPrefix(parameterInfo));
            str.Append(parameterInfo.Name);
            return str.ToString();
        }

        //writeParameterAttribute = new Func<FieldInfo, string>(fieldInfo => {
        //var attributes = CustomAttributeData.GetCustomAttributes(fieldInfo);
        //if (attributes.Any()){
        //return String.Format("[{0}] ", String.Join(", ", attributes.Select(customAttributeData => customAttributeData.AttributeType.ToString())
        //.Select(s => s.EndsWith("Attribute")?s.Substring(0, s.LastIndexOf("Attribute")):s)));
        //}
        //return "";
        //});
        public static string WriteParamType(ParameterInfo parameterInfo)
        {
            var str = new StringBuilder();
            var type = parameterInfo.ParameterType;
            var isParams = CustomAttributeData.GetCustomAttributes(parameterInfo).Any(a => a is ParamArrayAttribute);

            if (isParams)
            {
                str.Append("params ");
            }
            else
            {
                var prefix = GetArgPrefix(parameterInfo);
                if (prefix.Length > 0)
                {
                    str.Append(prefix);
                    type = type.GetElementType();
                }
            }

            str.Append(WriteWrapperType(type));
            return str.ToString();
        }

        public static string WritePropertyGetter(PropertyInfo propertyInfo, string wrappedInstance)
        {
            var str = new StringBuilder();
            if (propertyInfo.CanRead)
            {
                str.Append("get { return ");
                str.Append(wrappedInstance);
                str.Append(".");
                str.Append(propertyInfo.Name);
                str.Append("; }");
            }

            return str.ToString();
        }

        public static string WritePropertyHead(PropertyInfo propertyInfo)
        {
            var str = new StringBuilder();
            str.Append(WriteUnsafe(propertyInfo));
            str.Append(WriteWrapperType(propertyInfo.PropertyType));
            str.Append(" ");
            str.Append(propertyInfo.Name);
            return str.ToString();
        }

        public static string WritePropertyIndexerGetter(PropertyInfo propertyInfo, string wrappedInstance)
        {
            var indexers = propertyInfo.GetIndexParameters();
            var indexes = Join(", ", indexers.Select(i => i.Name));
            var str = new StringBuilder();
            if (propertyInfo.CanRead)
            {
                str.Append("get { return ");
                str.Append(wrappedInstance);
                str.Append("[");
                str.Append(indexes);
                str.Append("]; }");
            }

            return str.ToString();
        }
        public static string WritePropertyIndexerHead(PropertyInfo propertyInfo)
        {
            var indexers = propertyInfo.GetIndexParameters();
            var str = new StringBuilder();
            str.Append(WriteUnsafe(propertyInfo));
            str.Append(WriteWrapperType(propertyInfo.PropertyType));
            str.Append(" ");
            str.Append(
                $"this[{Join(", ", indexers.Select(i => $"{WriteType(i.ParameterType)} {i.Name}"))}]");
            return str.ToString();
        }

        public static string WritePropertyIndexerSetter(PropertyInfo propertyInfo, string wrappedInstance)
        {
            var indexers = propertyInfo.GetIndexParameters();
            var indexes = Join(", ", indexers.Select(i => i.Name));
            var str = new StringBuilder();
            var m = propertyInfo.GetSetMethod();
            if (m != null && m.IsPublic)
            {
                str.Append("set { ");
                str.Append(wrappedInstance);
                str.Append("[");
                str.Append(indexes);
                str.Append("] = value; }");
            }

            return str.ToString();
        }
        public static string WritePropertySetter(PropertyInfo propertyInfo, string wrappedInstance)
        {
            var str = new StringBuilder();
            var m = propertyInfo.GetSetMethod();
            if (m != null && m.IsPublic)
            {
                str.Append("set { ");
                str.Append(wrappedInstance);
                str.Append(".");
                str.Append(propertyInfo.Name);
                str.Append(" = value; }");
            }

            return str.ToString();
        }

        public static string WriteStaticMethod(MethodInfo methodInfo, string source)
        {
            var str = new StringBuilder();
            var parameters = methodInfo.GetParameters();
            var returnType = methodInfo.ReturnParameter.ParameterType;
            if (!IsVoid(returnType)) str.Append("return ");
            if (returnType.IsSubclassOf(typeof(Delegate)))
            {
                str.Append("new ");
                str.Append(WriteWrapperType(returnType));
                str.Append("(");
            }

            str.Append(source);
            str.Append(".");
            str.Append(methodInfo.Name);
            str.Append("(");
            str.Append(Join(", ", parameters.Select(WriteParamName)));
            if (returnType.IsSubclassOf(typeof(Delegate))) str.Append(")");
            str.Append(");");
            return str.ToString();
        }

        public static string WriteType(Type t)
        {
            var str = new StringBuilder();

            if (IsVoid(t))
            {
                str.Append("void");
            }
            else if (t == typeof(bool))
            {
                str.Append("bool");
            }
            else if (t == typeof(string))
            {
                str.Append("string");
            }
            else if (t == typeof(int))
            {
                str.Append("int");
            }
            else if (t == typeof(long))
            {
                str.Append("long");
            }
            else if (t == typeof(short))
            {
                str.Append("short");
            }
            else if (t == typeof(float))
            {
                str.Append("float");
            }
            else if (t == typeof(double))
            {
                str.Append("double");
            }
            else if (t == typeof(uint))
            {
                str.Append("uint");
            }
            else if (t == typeof(ushort))
            {
                str.Append("ushort");
            }
            else if (t == typeof(ulong))
            {
                str.Append("ulong");
            }
            else if (t == typeof(byte))
            {
                str.Append("byte");
            }
            else if (t == typeof(char))
            {
                str.Append("char");
            }
            else if (t == typeof(decimal))
            {
                str.Append("decimal");
            }
            else if (t == typeof(Enum))
            {
                str.Append("enum");
            }
            else if (t == typeof(sbyte))
            {
                str.Append("sbyte");
            }
            //else if (type == typeof(struct))
            //str.Append("struct");
            else if (t == typeof(short))
            {
                str.Append("short");
            }
            else if (t.IsArray)
            {
                str.Append(WriteArray(t));
            }
            else if (t.IsGenericType)
            {
                str.Append(WriteGeneric(t));
            }
            else if (t.IsSubclassOf(typeof(Delegate)))
            {
                str.Append(t.FullName.Replace("+", "."));
                return str.ToString();
            }
            else
            {
                str.Append(t.FullName.Replace("+", "."));
                return str.ToString();
            }

            if (t.IsNested)
            {
                str.Append(".");
                str.Append(t.Name);
            }

            return str.ToString();
        }

        public static string WriteTypeAttributes(Type type)
        {
            var attributes = CustomAttributeData.GetCustomAttributes(type);
            var list = FilterAttributes(attributes);
            if (list.Any())
                return
                    $"[{Join(", ", list.Select(a => a.AttributeType.ToString()).Select(s => s.EndsWith("Attribute") ? s.Substring(0, s.LastIndexOf("Attribute")) : s))}] ";
            return "";
        }

        public static string WriteUnsafe(object o)
        {
            var usafe = false;
            if (o is ConstructorInfo info)
                usafe = info.GetParameters().Any(p => p.ParameterType.IsPointer);
            else if (o is PropertyInfo propertyInfo)
                usafe = propertyInfo.PropertyType.IsPointer;
            else if (o is MethodInfo methodInfo)
                usafe = methodInfo.GetParameters().Any(p => p.ParameterType.IsPointer) ||
                        methodInfo.ReturnType.IsPointer;

            return usafe ? "unsafe " : Empty;
        }

        public static string WriteWrapperIndexer(PropertyInfo propertyInfo, string wrappedInstance)
        {
            var indexers = propertyInfo.GetIndexParameters();
            var str = new StringBuilder();
            var indexes = Join(", ", indexers.Select(i => i.Name));
            str.Append(WriteUnsafe(propertyInfo));
            str.Append(WriteWrapperType(propertyInfo.PropertyType));
            str.Append("this[");
            str.Append(Join(", ", indexers.Select(i => $"{WriteWrapperType(i.ParameterType)} {i.Name}")));
            str.Append("]");
            str.Append("{");
            if (propertyInfo.CanRead)
            {
                str.Append("get { return ");
                str.Append(wrappedInstance);
                str.Append("[");
                str.Append(indexes);
                str.Append("];");
            }

            var m = propertyInfo.GetSetMethod();
            if (m != null && m.IsPublic)
            {
                str.Append("set { ");
                str.Append(wrappedInstance);
                str.Append("[");
                str.Append(indexes);
                str.Append("] = value;");
            }

            str.Append("}");
            return str.ToString();
        }

        public static string WriteWrapperType(Type type)
        {
            var str = new StringBuilder();

            if (IsVoid(type))
                str.Append("void");
            else if (typeMapDictionary.ContainsKey(type))
                str.Append(typeMapDictionary[type]);
            else
                str.Append(WriteType(type));

            return str.ToString();
        }
    }
}