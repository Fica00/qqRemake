using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial
{
    public class TutorialMana : TutorialMessage
    {
        [SerializeField] private GameObject qommonsHighlight;
        [SerializeField] private TextMeshProUGUI qommonsText;        
        [SerializeField] private GameObject qommonsCantPlay;        
        [SerializeField] private TextMeshProUGUI qommonsCantPlayText;        
        [SerializeField] private GameObject endTurnHighlight;
        [SerializeField] private TextMeshProUGUI endTurnText;
        [SerializeField] private GameObject qoomonAddsPower;
        [SerializeField] private TextMeshProUGUI addsPowerText;
        [SerializeField] private GameObject highestPowerWin;
        [SerializeField] private TextMeshProUGUI highestPowerWinText;
        [SerializeField] private GameObject winLocations;
        [SerializeField] private TextMeshProUGUI winLocationsText;
        [SerializeField] private GameObject gainMana;
        [SerializeField] private TextMeshProUGUI gainManaText;
        [SerializeField] private GameObject cardsSpecialAbilities;
        [SerializeField] private TextMeshProUGUI cardsSpecialAbilitiesText;
        [SerializeField] private GameObject hoverWinLocations;
        [SerializeField] private TextMeshProUGUI hoverWinLocationsText;
        [SerializeField] private GameObject stakeFirstPart;
        [SerializeField] private TextMeshProUGUI stakeFirstPartText;
        [SerializeField] private GameObject stakeSecondPart;
        [SerializeField] private TextMeshProUGUI stakeSecondPartText;
        [SerializeField] private Button input;

        private int counter;
        private Coroutine coroutineTutorial;

        private void OnEnable()
        {
            input.onClick.AddListener(Next);
            EndTurnHandler.OnEndTurn += EndTurn;
            CardInteractions.OnClicked += OnCloseShowAbility;
        }

        private void EndTurn()
        {
            endTurnHighlight.SetActive(false); 
            Debug.Log("Counter" +counter);
            counter = 3;
            coroutineTutorial = StartCoroutine(ShowForSecondRound());
            ShowStep();
            EndTurnHandler.OnEndTurn -= EndTurn;
        }

        

        private void OnDisable()
        {
            input.onClick.RemoveListener(Next);
        }

        private void OnCloseShowAbility(CardObject _cardObject)
        {
            Debug.Log("OnCLoseShowAbility "+counter);
            if (counter == 6)
            {
                cardsSpecialAbilities.SetActive(false);
                CardInteractions.OnClicked -= OnCloseShowAbility;
                Next();
            }
            
        }
        

        private void Next()
        {
            counter++;
            Debug.Log("Next counter" + counter);
            if (counter < 3)
            {
                ShowStep();
            }
            else
            {
                coroutineTutorial = StartCoroutine(ShowForSecondRound());
            }
        }

        public override void Setup()
        {
            base.Setup();
            counter = 0;
            ShowStep();
            gameObject.SetActive(true);
            input.gameObject.SetActive(true);
        }

        private void ShowStep()
        {
            if (counter==0)
            {
                qommonsHighlight.SetActive(true);
                qommonsText.text = "Qoomons cost mana";
            }
            else if (counter==1)
            {
                qommonsHighlight.SetActive(false);
                qommonsCantPlay.SetActive(true);
                qommonsCantPlayText.text = "You can't play those now";
            }
            else if (counter==2)
            {
                qommonsCantPlay.SetActive(false);
                input.gameObject.SetActive(false);
                endTurnHighlight.SetActive(true);
                endTurnText.text = "Press the end turn button";
            }
        }
        
        private IEnumerator  ShowForSecondRound()
        { 
            if (counter == 3)
            {
                yield return new WaitForSeconds(4);
                Debug.Log("Nakon 4 sekunde");
                qoomonAddsPower.SetActive(true);
                input.gameObject.SetActive(true);
                StopCoroutine(coroutineTutorial);
            }
            else if (counter == 4)
            {
                qoomonAddsPower.SetActive(false);
                highestPowerWin.SetActive(true);
            }
            else if(counter == 5)
            {
                highestPowerWin.SetActive(false);
                gainMana.SetActive(true);
            }
            else if (counter == 6)
            {
                gainMana.SetActive(false);
                cardsSpecialAbilities.SetActive(true);
                input.gameObject.SetActive(false);
                
            }
            else if (counter == 7)
            {
                yield return new WaitForSeconds(4);
                cardsSpecialAbilities.SetActive(false);
                hoverWinLocations.SetActive(true);
                input.gameObject.SetActive(true);
            }
            else if (counter == 8)
            {
                input.gameObject.SetActive(false);
                yield return new WaitUntil(() => GameplayManager.Instance.CurrentRound == 3);
                hoverWinLocations.SetActive(false);
                stakeFirstPart.SetActive(true);
            }
            else if (counter == 9)
            {
                yield return new WaitUntil(() => BetClickHandler.Instance.DidIBetThisRound);
                stakeFirstPart.SetActive(false);
                stakeSecondPart.SetActive(true);
            }
        }
    }
}

