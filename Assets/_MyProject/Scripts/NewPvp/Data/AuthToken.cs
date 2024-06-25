using System;
using Newtonsoft.Json;

[Serializable]
public class AuthToken
{
    [JsonProperty("token")] public string Token;
}
