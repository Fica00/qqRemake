using System.Collections.Generic;
using System.Linq;

public static class Helpers
{
    public static List<CardObject> OrderQommons(List<int> _qommonIds)
    {
        List<CardObject> _cards = new List<CardObject>();
        foreach (var _qommonId in _qommonIds)
        {
            _cards.Add(CardsManager.Instance.GetCardObject(_qommonId));
        }
        
        return  _cards.OrderBy(_qommonInDeck=> _qommonInDeck.Details.Mana).
            ThenBy(_qommonInDeck=>_qommonInDeck.Details.Power).
            ToList();
    }
}