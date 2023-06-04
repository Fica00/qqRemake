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

    protected virtual void AddCommand(PlaceCommand _command)
    {
        if (_command.IsMyPlayer)
        {
            myCommands.Add(_command);
        }
        else
        {
            opponentCommands.Add(_command);
        }
    }

    protected virtual void RemoveCommand(PlaceCommand _command)
    {
        if (_command.IsMyPlayer)
        {
            myCommands.Remove(_command);
        }
        else
        {
            opponentCommands.Remove(_command);
        }
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

    public void SetOpponentCommands(List<PlaceCommand> _commands)
    {
        opponentCommands = _commands;
    }
}
