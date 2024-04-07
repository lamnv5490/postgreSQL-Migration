using System.Text;

namespace PostgreSQL_Migration.APIs.Helpers
{
    public static class ReflectionHelpers
    {
        public static string GetDefinition(this Type t)
        {
            if (t == typeof(void))
            {
                return "void";
            }

            var sb = new StringBuilder();

            var name = t.IsGenericType ? t.Name[..t.Name.IndexOf("`", StringComparison.Ordinal)] : t.Name;

            sb.Append(name);

            if (t.IsGenericType)
            {
                sb.Append('<');
                var first = true;
                foreach (var genericType in t.GenericTypeArguments)
                {
                    if (!first)
                    {
                        sb.Append(", ");
                    }

                    first = false;
                    sb.Append(GetDefinition(genericType));
                }

                sb.Append('>');
            }

            if (t.IsArray)
            {
                sb.Append("[]");
            }

            return sb.ToString();
        }
    }
}
