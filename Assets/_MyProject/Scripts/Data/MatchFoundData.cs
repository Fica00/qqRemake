using Newtonsoft.Json;

public class MatchFoundData
{
    [JsonProperty("roomName")]
    public string RoomName;

    [JsonProperty("firstPlayer")]
    public string FirstPlayer;

    [JsonProperty("secondPlayer")]
    public string SecondPlayer;
}