using System.IO.Hashing;
using System.Text;
using GBFRDataTools.Hashing;

namespace RelinkMulti;

public static class UtilityExtensions
{
    public const string EMPTY_HASH = "887AE0B0";

    extension(string str)
    {
        public string ToSnakeCase()
        {
            StringBuilder sb = new();

            sb.Append(char.ToLower(str[0]));
            foreach (char c in str[1..])
            {
                if (char.ToUpper(c) == c)
                {
                    sb.Append($"_{c}");
                }
                else
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }
    }

    extension(XXHash32Custom)
    {
        public static string HashAsString(string input)
            => XXHash32Custom.Hash(input).ToString("X8");
    }
}
