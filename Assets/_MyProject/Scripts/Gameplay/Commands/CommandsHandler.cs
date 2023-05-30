using System.Collections.Generic;

public class CommandsHandler
{
    List<PlaceCommand> myCommands = new List<PlaceCommand>();
    List<PlaceCommand> opponentCommands = new List<PlaceCommand>();

    public List<PlaceCommand> MyCommands => myCommands;
    public List<PlaceCommand> OpponentCommands => opponentCommands;

    public void Setup()
    {
        GameplayPlayer.AddedCardToTable += AddCommand;
        GameplayPlayer.RemovedCardFromTable += RemoveCommand;
    }

    public void Close()
    {
        GameplayPlayer.AddedCardToTable -= AddCommand;
        GameplayPlayer.RemovedCardFromTable -= RemoveCommand;
    }

    void AddCommand(PlaceCommand _command)
    {
        if (_command.Player == GameplayManager.Instance.MyPlayer)
        {
            myCommands.Add(_command);
        }
        else
        {
            opponentCommands.Add(_command);
        }
    }

    void RemoveCommand(PlaceCommand _command)
    {
        if (_command.Player == GameplayManager.Instance.MyPlayer)
        {
            myCommands.Remove(_command);
        }
        else
        {
            opponentCommands.Remove(_command);
        }
    }

    public bool CheckIfCardMovedThisTurn(CardObject _cardObject)
    {
        foreach (var _command in myCommands)
        {
            if (_command.Card == _cardObject)
            {
                return true;
            }
        }

        return false;
    }

    public PlaceCommand GetCommand(CardObject _cardObject)
    {
        foreach (var _command in myCommands)
        {
            if (_command.Card == _cardObject)
            {
                return _command;
            }
        }

        throw new System.Exception("Cant find command for object: " + _cardObject);
    }
}
