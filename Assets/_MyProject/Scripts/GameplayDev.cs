using NaughtyAttributes;
using UnityEngine;

public class GameplayDev : MonoBehaviour
{
    [Button("Bot forfeit")]
    private void BotForfeit()
    {
        GameplayManager.Instance.ForceEndGame(GameResult.Escaped);
    }
    
    [Button("Draw")]
    private void Draw()
    {
        GameplayManager.Instance.ForceEndGame(GameResult.Draw);
    }
    
    [Button("I won")]
    private void Won()
    {
        GameplayManager.Instance.ForceEndGame(GameResult.IWon);
    }
    
    [Button("I lost")]
    private void Lost()
    {
        GameplayManager.Instance.ForceEndGame(GameResult.ILost);
    }
    
        
    [Button("I forfeit")]
    private void IForfeit()
    {
        GameplayManager.Instance.ForceEndGame(GameResult.IForefiet);
    }
}
