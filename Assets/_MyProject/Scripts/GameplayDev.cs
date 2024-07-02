using System.Linq;
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


    [Button("Draw master octo")]
    private void DrawMasterOcto()
    {
        GameplayManager.Instance.MyPlayer.AddCardToHand(GameplayManager.Instance.MyPlayer.GetCardFromDeck(17), true);
    }

    [Button("Discard master octo")]
    private void DiscardMasterOcto()
    {
        GameplayManager.Instance.MyPlayer.DiscardCardFromHand(GameplayManager.Instance.MyPlayer.GetQommonFromHand(17));
    }


    [Button("Draw soulomon")]
    private void DrawSoulomon()
    {
        GameplayManager.Instance.MyPlayer.AddCardToHand(GameplayManager.Instance.MyPlayer.GetCardFromDeck(18), true);
    }

    [Button("Remove all cards in hand")]
    private void ForcePlaceTeapot()
    {
        CardObject _card = CardsManager.Instance.CreateCard(35, true);
        foreach (var _cardInHand in GameplayManager.Instance.MyPlayer.CardsInHand.ToList())
        {
            GameplayManager.Instance.MyPlayer.DiscardCardFromHand(_cardInHand);
        }

        GameplayManager.Instance.MyPlayer.AddCardToHand(_card);
    }
}