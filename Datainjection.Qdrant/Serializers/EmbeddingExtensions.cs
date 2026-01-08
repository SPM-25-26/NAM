using Domain.Attributes;
using System.Collections;
using System.Reflection;
using System.Text;

namespace DataInjection.Qdrant.Serializers
{
    /// <summary>
    /// Provides extension methods for serializing objects into embedding strings
    /// based on properties marked with the <see cref="Domain.Attributes.EmbeddableAttribute"/>.
    /// </summary>
    public static class EmbeddingExtensions
    {
        /// <summary>
        /// Converts the specified entity to an embedding string representation.
        /// Only properties marked with <see cref="Domain.Attributes.EmbeddableAttribute"/> are included.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="entity">The entity to convert.</param>
        /// <returns>
        /// A string representation of the entity suitable for embedding, or an empty string if the entity is null.
        /// </returns>
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

        /// <summary>
        /// Recursively processes an object and appends its embeddable properties to the string builder.
        /// </summary>
        /// <param name="obj">The object to process.</param>
        /// <param name="sb">The string builder to append to.</param>
        /// <param name="indent">The current indentation level.</param>
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

        /// <summary>
        /// Processes a collection and appends its items to the string builder.
        /// </summary>
        /// <param name="collection">The collection to process.</param>
        /// <param name="sb">The string builder to append to.</param>
        /// <param name="indent">The current indentation level.</param>
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

        /// <summary>
        /// Determines whether the specified type is a simple type (primitive, enum, string, decimal, date/time, or Guid).
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>True if the type is simple; otherwise, false.</returns>
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

        /// <summary>
        /// Formats a simple value for embedding output.
        /// </summary>
        /// <param name="value">The value to format.</param>
        /// <returns>The formatted string representation of the value.</returns>
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

        /// <summary>
        /// Appends indentation spaces to the string builder.
        /// </summary>
        /// <param name="sb">The string builder to append to.</param>
        /// <param name="indent">The number of indentation levels.</param>
        private static void AppendIndent(StringBuilder sb, int indent)
        {
            sb.Append(new string(' ', indent * 2));
        }
    }
}