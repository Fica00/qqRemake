using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public static class JsonUtilities
{
    public static GameData ConvertJsonToGameData(string _json)
    {
        // Deserialize the JSON to a dynamic object to handle both arrays and dictionaries.
        dynamic _obj = JsonConvert.DeserializeObject(_json);

        if (_obj==null)
        {
            return null;
        }

        // Check if "Marketplace" is an array (it could be an array or a dictionary/object).
        if (_obj.Marketplace is JArray)
        {
            // Create a new dictionary to hold the offers with generated keys.
            var _marketplaceDict = new Dictionary<string, GamePassOffer>();

            // Iterate through the array and populate the dictionary.
            int _i = 0; // This is just an example. In a real case, you should use meaningful keys.
            foreach (var _offer in _obj.Marketplace)
            {
                // Convert each offer to a GamePassOffer object.
                var _offerObj = _offer.ToObject<GamePassOffer>();
                _marketplaceDict.Add(_i.ToString(), _offerObj);
                _i++;
            }

            // Replace the array with our new dictionary.
            _obj.Marketplace = JObject.FromObject(_marketplaceDict);
        }

        // Serialize the modified object back to JSON.
        string _modifiedJson = JsonConvert.SerializeObject(_obj);

        // Deserialize the JSON to the final GameData class.
        GameData _gameData = JsonConvert.DeserializeObject<GameData>(_modifiedJson);
        
        return _gameData;
    }

    public static Dictionary<string, GamePassOffer> ConvertJsonToMarketplace(string _json)
    {
        // Attempt to deserialize the JSON to a JToken (which could be an array or object).
        JToken _token = JToken.Parse(_json);

        // Prepare the dictionary to hold the marketplace offers.
        var _marketplaceDict = new Dictionary<string, GamePassOffer>();

        // Check if the token is an array.
        if (_token is JArray _marketplaceArray)
        {
            // Iterate through the array and populate the dictionary.
            for (int _i = 0; _i < _marketplaceArray.Count; _i++)
            {
                // Convert each offer to a GamePassOffer object.
                GamePassOffer _offerObj = _marketplaceArray[_i].ToObject<GamePassOffer>();
                _marketplaceDict.Add(_i.ToString(), _offerObj);
            }
        }
        else if (_token is JObject _marketplaceObject)
        {
            // If it's an object, we can deserialize it directly to a dictionary.
            _marketplaceDict = _marketplaceObject.ToObject<Dictionary<string, GamePassOffer>>();
        }

        return _marketplaceDict;
    }
}
