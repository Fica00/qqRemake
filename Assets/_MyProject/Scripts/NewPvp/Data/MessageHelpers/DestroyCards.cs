using System;
using System.Collections.Generic;

namespace MessageHelpers
{
    [Serializable]
    public class DestroyCards
    {
        public List<int> PlaceIds;
        public bool DestroyMyCards;
        public List<CardObject> _cardsToDestroy;
    }
}