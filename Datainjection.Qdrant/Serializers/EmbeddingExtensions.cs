using Domain.Attributes;
using System.Collections;
using System.Reflection;
using System.Text;

namespace DataInjection.Qdrant.Serializers
{
    public static class EmbeddingExtensions
    {
        public static string ToEmbeddingString<T>(this T entity)
        {
            if (entity is null)
            {
                return string.Empty;
            }

            var sb = new StringBuilder();
            ProcessObject(entity, sb, 0);
            return sb.ToString().Trim();
        }

        private static void ProcessObject(object obj, StringBuilder sb, int indent)
        {
            if (obj is null)
            {
                return;
            }

            var type = obj.GetType();
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                var embeddableAttr = property.GetCustomAttribute<EmbeddableAttribute>();
                if (embeddableAttr is null)
                {
                    continue;
                }

                var value = property.GetValue(obj);
                if (value is null)
                {
                    continue;
                }

                var propertyName = property.Name;
                AppendIndent(sb, indent);

                if (IsSimpleType(property.PropertyType))
                {
                    sb.AppendLine($"{propertyName}: {FormatSimpleValue(value)}");
                }
                else if (value is IEnumerable enumerable and not string)
                {
                    sb.AppendLine($"{propertyName}:");
                    ProcessCollection(enumerable, sb, indent + 1);
                }
                else
                {
                    sb.AppendLine($"{propertyName}:");
                    ProcessObject(value, sb, indent + 1);
                }
            }
        }

        private static void ProcessCollection(IEnumerable collection, StringBuilder sb, int indent)
        {
            foreach (var item in collection)
            {
                if (item is null)
                {
                    continue;
                }

                if (IsSimpleType(item.GetType()))
                {
                    AppendIndent(sb, indent);
                    sb.AppendLine($"- {FormatSimpleValue(item)}");
                }
                else
                {
                    AppendIndent(sb, indent);
                    sb.AppendLine("-");
                    ProcessObject(item, sb, indent + 1);
                }
            }
        }

        private static bool IsSimpleType(Type type)
        {
            return type.IsPrimitive
                || type.IsEnum
                || type == typeof(string)
                || type == typeof(decimal)
                || type == typeof(DateTime)
                || type == typeof(DateTimeOffset)
                || type == typeof(TimeSpan)
                || type == typeof(Guid);
        }

        private static string FormatSimpleValue(object value)
        {
            return value switch
            {
                string str => $"\"{str}\"",
                DateTime dt => $"\"{dt:yyyy-MM-dd HH:mm:ss}\"",
                DateTimeOffset dto => $"\"{dto:yyyy-MM-dd HH:mm:ss}\"",
                _ => value.ToString() ?? string.Empty
            };
        }

        private static void AppendIndent(StringBuilder sb, int indent)
        {
            sb.Append(new string(' ', indent * 2));
        }
    }
}