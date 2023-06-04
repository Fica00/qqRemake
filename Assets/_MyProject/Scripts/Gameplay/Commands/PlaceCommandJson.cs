using System;

[Serializable]
public class PlaceCommandJson
{
    public int CardId;
    public int PlaceId;
    public bool MyPlayer;
    public LaneLocation LaneLocation;

    public static PlaceCommandJson Create(PlaceCommand _command)
    {
        PlaceCommandJson _placeCommandJson = new PlaceCommandJson();
        _placeCommandJson.PlaceId = _command.PlaceId;
        _placeCommandJson.CardId = _command.Card.Details.Id;
        _placeCommandJson.LaneLocation = _command.Location;
        _placeCommandJson.MyPlayer = _command.IsMyPlayer;
        return _placeCommandJson;
    }

    public static PlaceCommand ToPlaceCommand(PlaceCommandJson _placeCommandJson)
    {
        PlaceCommand _placeCommand = new PlaceCommand();

        CardObject _card = CardsManager.Instance.CreateCard(_placeCommandJson.CardId, _placeCommand.IsMyPlayer);
        _placeCommand.Card = _card;
        _placeCommand.PlaceId = _placeCommandJson.PlaceId;
        _placeCommand.IsMyPlayer = _placeCommandJson.MyPlayer;
        _placeCommand.Location = _placeCommandJson.LaneLocation;

        return _placeCommand;
    }
}
