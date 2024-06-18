using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial
{
    public class TutorialManager : TutorialMessage
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
        [SerializeField] private GameObject samuKitsunePart;
        [SerializeField] private GameObject mukongPart;
        [SerializeField] private GameObject geishaKoPart;
        [SerializeField] private GameObject satiTheTigarPart;
        [SerializeField] private GameObject laneTop;
        [SerializeField] private GameObject laneMid;
        [SerializeField] private GameObject laneBot;
        [SerializeField] private GameObject battleTextGameObject;
        [SerializeField] private TextMeshProUGUI battleText;

        [SerializeField] private GameObject myCoomonPlaces;
        [SerializeField] private GameObject opponentCommonPlaces;

        
        
        private int counter;
        private Coroutine coroutineTutorial;
        public bool isAddsPowerAndHighestPowerPanelShowen = false;

        private void OnEnable()
        {
            
            input.onClick.AddListener(Next);
            EndTurnHandler.OnEndTurn += EndTurn;
            EndTurnHandler.OnEndTurn += EndTurnGoldi;
            CardInteractions.OnClicked += OnCloseShowAbility;
        }

        private void EndTurnGoldi()
        {
            if (counter == 9)
            {
                hoverWinLocations.SetActive(false);
                EndTurnHandler.OnEndTurn -= EndTurnGoldi;
            }
        }

        private void EndTurn()
        {
            endTurnHighlight.SetActive(false); 
            Debug.Log("Counter" +counter);
            counter = 3;
            coroutineTutorial = StartCoroutine(ShowStep());
            EndTurnHandler.OnEndTurn -= EndTurn;
        }

        

        private void OnDisable()
        {
            input.onClick.RemoveListener(Next);
        }

        private void OnCloseShowAbility(CardObject _cardObject)
        {
            Debug.Log("OnCLoseShowAbility "+counter);
            if (counter == 7)
            {
                cardsSpecialAbilities.SetActive(false);
                CardInteractions.OnClicked -= OnCloseShowAbility;  //Ovde regulises onaj deo sa abilitijima
                Next();
            }
            
        }
        

        private void Next()
        {
            TurnOfGOParts();
            counter++;
            
            Debug.Log("Next counter" + counter);
           
                coroutineTutorial = StartCoroutine(ShowStep());
            
        }

        private void TurnOfGOParts()
        {
            
            samuKitsunePart.SetActive(false);
            geishaKoPart.SetActive(false);
            satiTheTigarPart.SetActive(false);
            mukongPart.SetActive(false);
            laneTop.SetActive(false);
            laneMid.SetActive(false);
            laneBot.SetActive(false);
        }

        public override void Setup()
        {
            base.Setup();
            counter = 0;
            coroutineTutorial = StartCoroutine(ShowStep());
            gameObject.SetActive(true);
            
        }

       

        private GameObject _myQoomonCard = default;
        private GameObject _opponentQoomonCard = default;
        
        private IEnumerator  ShowStep()
        {
            Debug.Log(counter);
            if (counter==0)
            {
                yield return new WaitForSeconds(2);
                input.gameObject.SetActive(true);
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
            if (counter == 3)
            {
                yield return new WaitUntil(() => GameplayTutorial.Instance.cardsPlayed);
                _opponentQoomonCard = opponentCommonPlaces.GetComponentInChildren<CardObject>().gameObject;
                _opponentQoomonCard.SetActive(false);
                Debug.Log("Nakon 4 sekunde");
                qoomonAddsPower.SetActive(true);
                input.gameObject.SetActive(true);
                StopCoroutine(coroutineTutorial);
            }
            else if (counter == 4)
            {
                _opponentQoomonCard.SetActive(true);
                qoomonAddsPower.SetActive(false);
                _myQoomonCard =  myCoomonPlaces.GetComponentInChildren<CardObject>().gameObject;
                _myQoomonCard.SetActive(false);
                highestPowerWin.SetActive(true);
            }
            else if (counter == 5)
            {
                _myQoomonCard.SetActive(true);
                highestPowerWin.SetActive(false);
                winLocations.SetActive(true);
                input.gameObject.SetActive(true);
            }
            else if(counter == 6)
            {
                winLocations.SetActive(false);
                input.gameObject.SetActive(false);
                gainMana.SetActive(true);
                input.gameObject.SetActive(true);
                isAddsPowerAndHighestPowerPanelShowen = true;
            }
            else if (counter == 7)
            {
                input.gameObject.SetActive(false);
                yield return new WaitUntil(() => GameplayManager.Instance.GameplayState == GameplayState.Playing);
                gainMana.SetActive(false);
                yield return new WaitForSeconds(2);
                cardsSpecialAbilities.SetActive(true);
                
                //CardDetailsPanel.OnClose +=  
                
            }
            else if (counter == 8)
            {
                yield return new WaitForSeconds(4);
                cardsSpecialAbilities.SetActive(false);
                hoverWinLocations.SetActive(true);
                input.gameObject.SetActive(true);
            }
            else if (counter == 9)
            {
                input.gameObject.SetActive(false);
                hoverWinLocationsText.text = "Play Goldie on middle location and end turn";
                yield return new WaitUntil(() => GameplayManager.Instance.CurrentRound == 3);
                hoverWinLocationsText.text = "";
                hoverWinLocations.SetActive(false);
                yield return new WaitUntil(() => GameplayManager.Instance.GameplayState == GameplayState.Playing);
                stakeFirstPart.SetActive(true);
                Next();
            }
            else if (counter == 10)
            {
                yield return new WaitUntil(() => BetClickHandler.Instance.DidIBetThisRound);
                stakeFirstPart.SetActive(false);
                yield return new WaitForSeconds(4);
                stakeSecondPart.SetActive(true);
                input.gameObject.SetActive(true);
            }
            else if(counter == 11)
            {
                
                samuKitsunePart.SetActive(true);
                laneBot.SetActive(true);
                stakeSecondPart.SetActive(false);
                input.gameObject.SetActive(false);
                PlayTutorialCards.OnNextStep += Next;
            }
            else if(counter == 12)
            {
                yield return new WaitUntil(() => GameplayManager.Instance.CurrentRound == 4 && GameplayManager.Instance.GameplayState == GameplayState.Playing);
                samuKitsunePart.SetActive(false);
                laneBot.SetActive(false);
                mukongPart.SetActive(true);
                laneMid.SetActive(true);
               
            }
            else if(counter == 13)
            {
                yield return new WaitUntil(() => GameplayManager.Instance.CurrentRound == 5 && GameplayManager.Instance.GameplayState == GameplayState.Playing);
                mukongPart.SetActive(false);
                geishaKoPart.SetActive(true);
                laneMid.SetActive(true);
               
            }
            else if(counter == 14)
            {
                yield return new WaitUntil(() => GameplayManager.Instance.CurrentRound == 6 && GameplayManager.Instance.GameplayState == GameplayState.Playing);
                geishaKoPart.SetActive(false);
                laneMid.SetActive(false);
                satiTheTigarPart.SetActive(true);
                laneBot.SetActive(true);
               
                
            }
            else if (counter == 15)
            {
                PlayTutorialCards.OnNextStep -= Next;
            }
            
        }
    }
}

