using System;
using UnityEngine;

namespace Tutorial
{
    public class PlayFirstCard : MonoBehaviour
    {
        [SerializeField] private GameObject dragAnimation;

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
            int _round = GameplayManager.Instance.CurrentRound;
            switch (GameplayManager.Instance.GameplayState)
            {
                case GameplayState.StartingAnimation:
                    break;
                case GameplayState.ResolvingBeginingOfRound:
                    break;
                case GameplayState.Playing:
                    if (_round==1)
                    {
                        dragAnimation.SetActive(true);
                        CommandsHandler.AddedNewCommandForMe += FinishAnimation;
                    }
                    break;
                case GameplayState.Waiting:
                    break;
                case GameplayState.ResolvingEndOfRound:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void FinishAnimation()
        {
            CommandsHandler.AddedNewCommandForMe -= FinishAnimation;
            GameplayTutorial.Instance.ShowMana();
            dragAnimation.SetActive(false);
        }
    }
}