using System;
using System.Collections.Generic;

public class CommandsHandler
{
    public static Action<PlaceCommand> AddedNewCommandDuringTurn;
    private List<PlaceCommand> myCommandsThisTurn = new List<PlaceCommand>();
    private List<PlaceCommand> opponentCommands = new List<PlaceCommand>();
    private List<PlaceCommand> opponentCommandsThisTurn = new List<PlaceCommand>();

    public List<PlaceCommand> MyCommands => MyCommandsThisTurn;

    public List<PlaceCommand> MyCommandsThisTurn { get; } = new List<PlaceCommand>();

    public List<PlaceCommand> OpponentCommands => opponentCommands;
    public List<PlaceCommand> OpponentCommandsThisTurn { get; } = new List<PlaceCommand>();
    
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
            MyCommandsThisTurn.Add(_command);
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
            MyCommandsThisTurn.Remove(_command);
        }
        else
        {
            opponentCommands.Remove(_command);
        }
    }

    public PlaceCommand GetCommand(CardObject _cardObject)
    {
        foreach (var _command in MyCommandsThisTurn)
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
