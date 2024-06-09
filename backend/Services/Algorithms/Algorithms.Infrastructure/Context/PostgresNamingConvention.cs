using System.Globalization;
using System.Text;
namespace Algorithms.Infrastructure.Context;

public static class PostgresNamingConvention
{
    /// <summary>
    /// Converts pascal case to snake case that's compatible with postgre. Eg MyTableName -> my_table_name.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string ConvertName(this string name)
    {
        if (name == null)
            return null;

        var stringBuilder = new StringBuilder();

        for (int i = 0; i < name.Length; i++)
        {
            char character = name[i];
            if (char.IsUpper(character))
            {
                if (i != 0)
                {
                    stringBuilder.Append('_');
                }
                stringBuilder.Append(char.ToLower(character, CultureInfo.InvariantCulture));
            }
            else
            {
                stringBuilder.Append(character);
            }
        }

        return stringBuilder.ToString();
    }
}