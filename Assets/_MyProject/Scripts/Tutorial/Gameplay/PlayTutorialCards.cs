using System;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace Tutorial
{
    public class PlayTutorialCards : MonoBehaviour
    {
        public static Action OnNextStep;
        
        [SerializeField] private GameObject dragAnimation;
        [SerializeField] private int round;
        

        private void OnEnable()
        {
            GameplayManager.UpdatedGameState += CheckGameState;
        }

        private void OnDisable()
        {
            GameplayManager.UpdatedGameState -= CheckGameState;
        }

        private void CheckGameState()
        {
            round = GameplayManager.Instance.CurrentRound;
            switch (GameplayManager.Instance.GameplayState)
            {
                case GameplayState.StartingAnimation:
                    break;
                case GameplayState.ResolvingBeginingOfRound:
                    break;
                case GameplayState.Playing:

                    if (round == 1)
                    {
                        dragAnimation.SetActive(true);
                    }
                    GameplayPlayer.AddedCardToTable += CheckForLocation;
                   
                    break;
                case GameplayState.Waiting:
                    break;
                case GameplayState.ResolvingEndOfRound:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void CheckForLocation(PlaceCommand _command)
        {
            Debug.Log("Usao je u Check i postaivo na lejn" + _command.Location);
            
            
            if (round == 1)
            {
                if (_command.Location ==  0 && _command.Card.Details.Id ==  1)
                {
                    
                    GameplayPlayer.AddedCardToTable -= CheckForLocation;
                    FinishAnimation();
                   return;
                }
                GameplayManager.Instance.MyPlayer.CancelAllCommands();
                
            }
            if (round == 2)
            {
                if (_command.Location == LaneLocation.Mid && _command.Card.Details.Id ==  3)
                {
                    GameplayPlayer.AddedCardToTable -= CheckForLocation;
                    return;
                    
                }
                GameplayManager.Instance.MyPlayer.CancelAllCommands();
            }
            if (round == 3)
            {
                if (_command.Location == LaneLocation.Bot &&  _command.Card.Details.Id ==  8)
                {
                    GameplayPlayer.AddedCardToTable -= CheckForLocation;
                    OnNextStep?.Invoke();
                    return;
                } 
                GameplayManager.Instance.MyPlayer.CancelAllCommands();
            }
            if (round == 4)
            {
                if (_command.Location == LaneLocation.Mid &&  _command.Card.Details.Id ==  7)
                {
                    GameplayPlayer.AddedCardToTable -= CheckForLocation;
                    OnNextStep?.Invoke();
                    return;
                }
                GameplayManager.Instance.MyPlayer.CancelAllCommands();
                
            }
            if (round == 5)
            {
                if (_command.Location == LaneLocation.Mid &&  _command.Card.Details.Id ==  9)
                {
                    
                    GameplayPlayer.AddedCardToTable -= CheckForLocation;
                    OnNextStep?.Invoke();
                    return;
                } 
                GameplayManager.Instance.MyPlayer.CancelAllCommands();
               
            }
            if (round == 6)
            {
                if (_command.Location == LaneLocation.Bot &&  _command.Card.Details.Id ==  5)
                {
                    GameplayPlayer.AddedCardToTable -= CheckForLocation;
                    OnNextStep?.Invoke();
                    return;
                  
                } 
                GameplayManager.Instance.MyPlayer.CancelAllCommands(); 
            }
        }

        

        private void FinishAnimation()
        {
            
            dragAnimation.SetActive(false);
            GameplayTutorial.Instance.ShowMana();
            
        }
    }
}