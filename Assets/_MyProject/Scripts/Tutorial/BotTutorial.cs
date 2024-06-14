using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;


public class BotTutorial : BotPlayer
{
    private Coroutine playCoroutine;
    private bool hasPlayedThisRound;
    private BotType botType;
    public override void Setup()
    {
        base.Setup();
        ((GameplayPlayer)this).CardsInDeck = new List<CardObject>();
        DeckData _deckToPlay = DeckInitializer.InitializeDecks().Single(x => x.Name == "BotTutorial");
        
         foreach (var _cardInDeck in DeckInitializer.InitializeDecks().Single(x => x.Id==5).CardsInDeck)
        {
            CardObject _cardObject = CardsManager.Instance.CreateCard(_cardInDeck, IsMy);
            _cardObject.transform.SetParent(transform);
            
            ((GameplayPlayer)this).CardsInDeck.Add(_cardObject);
        }
        


        /*
         * TODO: 4 Vidi kako možeš da manipulišeš botom.
         */

    }
    
    
    protected override void ManageGameState()
    {
        switch (GameplayManager.Instance.GameplayState)
        {
            case GameplayState.ResolvingBeginingOfRound:
                hasPlayedThisRound = false;
                break;
            case GameplayState.Playing:
                if (playCoroutine != null)
                {
                    StopCoroutine(playCoroutine);
                }
                if (!hasPlayedThisRound)
                {
                    Debug.Log("BotTutorial");
                    playCoroutine = StartCoroutine(PlayCards());
                }
                break;
            case GameplayState.Waiting:
                break;
            case GameplayState.ResolvingEndOfRound:
                break;
        }
    }
    
    
    
    private IEnumerator PlayCards()   // TODO: Srediti kako da igra po potezima
    {
        Debug.Log("botType"+botType);
        botType = BotType.Version2;
        if (botType is BotType.Version2 or BotType.Version3)
        {
            Debug.Log("Pre wait-a");
            yield return new WaitUntil(() => GameplayManager.Instance.IFinished );
            Debug.Log("OVDE je vec prosao zato sto je GameplayManager.Instance.IFinished"+GameplayManager.Instance.IFinished);
            int _randomNumber = Random.Range(0, 10);
            if (_randomNumber > 5)
            {
                if (_randomNumber < 7)
                {
                    yield return new WaitForSeconds(2);
                }
                else if (_randomNumber < 9)
                {
                    yield return new WaitForSeconds(4);
                }
                else
                {
                    yield return new WaitForSeconds(6);
                }
            }
        }

        int[] _playerPower = GameplayManager.Instance.TableHandler.GetAllPower(true).ToArray();
        int[] _botPower = GameplayManager.Instance.TableHandler.GetAllPower(false).ToArray();

        bool[] _canPlaceCard = new bool[3];

        if (GameplayManager.Instance.CurrentRound is 1)
        {
            CardObject _cardObject = CardsManager.Instance.CreateCard(0, false);
            PlaceCard(_cardObject,_botPower, 0);
        }

        if (GameplayManager.Instance.CurrentRound is 2)
        {
            CardObject _cardObject = CardsManager.Instance.CreateCard(4, false);
            PlaceCard(_cardObject,_botPower, 1);
        }
        
        if (GameplayManager.Instance.CurrentRound is 3)
        {
            CardObject _cardObject = CardsManager.Instance.CreateCard(4, false);
            PlaceCard(_cardObject,_botPower, 2);
        }
        
        if (GameplayManager.Instance.CurrentRound is 4)
        {
            CardObject _cardObject = CardsManager.Instance.CreateCard(21, false);
            PlaceCard(_cardObject,_botPower, 2);
        }
        
        if (GameplayManager.Instance.CurrentRound is 5)
        {
            CardObject _cardObject1 = CardsManager.Instance.CreateCard(4, false);
            CardObject _cardObject2 = CardsManager.Instance.CreateCard(8, false);
            PlaceCard(_cardObject1,_botPower, 0);
            PlaceCard(_cardObject2,_botPower, 0);
        }
        
        if (GameplayManager.Instance.CurrentRound is 6)
        {
            CardObject _cardObject = CardsManager.Instance.CreateCard(29, false);
            PlaceCard(_cardObject,_botPower, 0);
        }

        

        hasPlayedThisRound = true;
        GameplayManager.Instance.OpponentFinished();
    }
    
    private void PlaceCard(CardObject _card, int[] _power, int _index)
    {
        if (Energy < _card.Stats.Energy)
        {
            return;
        }

        LanePlaceIdentifier _place = GameplayManager.Instance.Lanes[_index].GetPlaceLocation(false);
        LaneAbility _laneAbility = null;
        
        if (GameplayManager.Instance.LaneAbilities.ContainsKey(GameplayManager.Instance.Lanes[_index]))
        {
            _laneAbility = GameplayManager.Instance.LaneAbilities[GameplayManager.Instance.Lanes[_index]];
        }
        if (_place == null)
        {
            return;
        }

        if (_laneAbility!=null)
        {
            foreach (var _ability in _laneAbility.Abilities)
            {
                if (_ability is LaneAbilityChangePowerToQommonsHere)
                {
                    LaneAbilityChangePowerToQommonsHere _lowerPowerAbility =
                        (_ability as LaneAbilityChangePowerToQommonsHere);
                    if (_lowerPowerAbility.PowerAmount<0&& Math.Abs(_lowerPowerAbility.PowerAmount)>_card.Details.Power)
                    {
                        return;
                    }
                }
            }
           
        }

        if (_card.TryToPlace(_place))
        {
            _card.Display.HideCardOnTable();
            _power[_index] += _card.Stats.Power;
        }
    }
}
