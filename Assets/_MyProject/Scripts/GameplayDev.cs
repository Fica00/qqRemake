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

    [Button("Force last round the match")]
    private void ForceLastRound()
    {
        GameplayManager.Instance.SetCurrentRoundWithoutUpdate(6);
    }

    [Button("Get mana")]
    private void GetMana()
    {
        GameplayManager.Instance.MyPlayer.Energy++;
    }

    [Button()]
    private void DrawCard()
    {
        GameplayManager.Instance.DrawCard(GameplayManager.Instance.MyPlayer);
    }
    
    [Button()]
    private void RecalculatePower()
    {
        GameplayManager.Instance.TableHandler.CalculatePower();
    }
}
