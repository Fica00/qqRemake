using System;
using System.Collections.Generic;

[Serializable]
public class DeckData
{
    public int Id;
    public List<int> CardsInDeck = new ();
    public string Name = "New deck";
}
