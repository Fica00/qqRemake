using System;
using System.Linq;

public static class Utils
{
    public static string RemoveWhitespace(this string _input)
    {
        return new string(_input.ToCharArray()
            .Where(_c => !char.IsWhiteSpace(_c))
            .ToArray());
    }

    public static string GetItemName(ItemType _item)
    {
        switch (_item)
        {
            case ItemType.Qoomon:
                return "Qoomon";
            case ItemType.Exp:
                return "EXP";
            default:
                throw new ArgumentOutOfRangeException(nameof(_item), _item, null);
        }
    }
}
