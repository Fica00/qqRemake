using System;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace Tutorial
{
    public class PlayTutorialCards : MonoBehaviour
    {
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
                        CommandsHandler.AddedNewCommandForMe += FinishAnimation;
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
                
                Debug.Log(_command.Location);
                if (_command.Location !=  0)   //LaneLocation.Top)
                {
                    GameplayManager.Instance.MyPlayer.CancelAllCommands();
                }
                GameplayPlayer.AddedCardToTable -= CheckForLocation;
            }
            

            if (round == 2)
            {
                if (_command.Location != LaneLocation.Mid)
                {
                    GameplayManager.Instance.MyPlayer.CancelAllCommands();
                }
            }
        }

        

        private void FinishAnimation()
        {
            CommandsHandler.AddedNewCommandForMe -= FinishAnimation;
            dragAnimation.SetActive(false);
            GameplayTutorial.Instance.ShowMana();
            
        }
    }
}