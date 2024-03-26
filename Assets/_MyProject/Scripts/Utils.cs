using System.Linq;

public static class Utils
{
    public static string RemoveWhitespace(this string _input)
    {
        return new string(_input.ToCharArray()
            .Where(_c => !char.IsWhiteSpace(_c))
            .ToArray());
    }
}
