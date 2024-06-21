using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerTutorial : GameplayPlayer
{
    public override void Setup()
    {
        base.Setup();

        CardsInDeck = new List<CardObject>();
        foreach (var _cardInDeck in DeckInitializer.InitializeDecks().Single(x => x.Name=="Starter").CardsInDeck)
        {
            CardObject _cardObject = CardsManager.Instance.CreateCard(_cardInDeck, IsMy);
            _cardObject.transform.SetParent(transform);
            CardsInDeck.Add(_cardObject);
        }
        
    }
    
    
    
    
    
  
    
    
}
