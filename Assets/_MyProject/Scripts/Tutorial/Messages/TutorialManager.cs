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
        [SerializeField] private GameObject goldiPart;
        [SerializeField] private GameObject stakeFirstPart;
        [SerializeField] private TextMeshProUGUI stakeFirstPartText;
        [SerializeField] private GameObject stakeSecondPart;
        [SerializeField] private TextMeshProUGUI stakeSecondPartText;
        [SerializeField] private Button input;
        [SerializeField] private GameObject samuKitsunePart;
        [SerializeField] private GameObject mukongPart;
        [SerializeField] private GameObject geishaKoPart;
        [SerializeField] private GameObject satiTheTigarPart;

        [SerializeField] private GameObject darkLayerQoomonsInHandMid;
        [SerializeField] private GameObject darkLayerQoomonsInHandBot;
            
        [SerializeField] private GameObject playerNameHolder;
        [SerializeField] private TextMeshProUGUI playerNameText;
        [SerializeField] private GameObject statsDarkerLayer;
        [SerializeField] private GameObject playerNameStats;
        [SerializeField] private TextMeshProUGUI playerNameStatsText;

        
        [SerializeField] private GameObject laneTop;
        [SerializeField] private GameObject laneMid;
        [SerializeField] private GameObject laneBot;
        

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
            PlayerStatsDisplay.OnPlayerNameClicked += OnPlayerClicked;
            PlayerStatsDisplay.OnPlayerStatsClose += OnPlayerStatsClosed;
            PlayTutorialCards.OnCardPlacedCorrected += TurnOfGOParts;
        }

        private void OnPlayerStatsClosed()
        {
            PlayerStatsDisplay.OnPlayerStatsClose -= OnPlayerStatsClosed;
            Next();
        }

        private void OnPlayerClicked()
        {
            PlayerStatsDisplay.OnPlayerNameClicked -= OnPlayerClicked;
            Next();
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
            endTurnHighlight.SetActive(false); ;
            counter = 6;
            Debug.Log("EndTurn Counter: "+counter);
            coroutineTutorial = StartCoroutine(ShowStep());
            EndTurnHandler.OnEndTurn -= EndTurn;
        }

        

        private void OnDisable()
        {
            input.onClick.RemoveListener(Next);
            PlayTutorialCards.OnCardPlacedCorrected -= TurnOfGOParts;
        }

        private void OnCloseShowAbility(CardObject _cardObject)
        {
            Debug.Log("OnCLoseShowAbility "+counter);
            if (counter == 9)
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
            goldiPart.SetActive(false);
            samuKitsunePart.SetActive(false);
            geishaKoPart.SetActive(false);
            satiTheTigarPart.SetActive(false);
            mukongPart.SetActive(false);
            laneTop.SetActive(false);
            laneMid.SetActive(false);
            laneBot.SetActive(false);
            darkLayerQoomonsInHandMid.SetActive(false);
            darkLayerQoomonsInHandBot.SetActive(false);
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
                gainMana.SetActive(true);
               
            }
            else if (counter==2)
            {
                gainMana.SetActive(false);
                qommonsCantPlay.SetActive(true);
                qommonsCantPlayText.text = "You can't play those now";
            }
            else if(counter == 3)
            {
                qommonsCantPlay.SetActive(false);
                input.gameObject.SetActive(false);
                playerNameHolder.SetActive(true);
                playerNameText.text = DataManager.Instance.PlayerData.Name;
            }
            else if (counter == 4)
            {
                playerNameHolder.SetActive(false);
                statsDarkerLayer.SetActive(true);
                playerNameStats.SetActive(true);
                playerNameStatsText.text =  DataManager.Instance.PlayerData.Name;
                
            }
            else if (counter==5) // Odavde nastaviti na gore sa brojevima
            {
                statsDarkerLayer.SetActive(false);
                playerNameStats.SetActive(false);
                endTurnHighlight.SetActive(true);
                endTurnText.text = "Press the end turn button";
            }
            if (counter == 6)
            {
                yield return new WaitUntil(() => GameplayTutorial.Instance.cardsPlayed);
                _opponentQoomonCard = opponentCommonPlaces.GetComponentInChildren<CardObject>().gameObject;
                _opponentQoomonCard.SetActive(false);
                Debug.Log("Nakon 4 sekunde");
                qoomonAddsPower.SetActive(true);
                input.gameObject.SetActive(true);
                StopCoroutine(coroutineTutorial);
            }
            else if (counter == 7)
            {
                _opponentQoomonCard.SetActive(true);
                qoomonAddsPower.SetActive(false);
                _myQoomonCard =  myCoomonPlaces.GetComponentInChildren<CardObject>().gameObject;
                _myQoomonCard.SetActive(false);
                highestPowerWin.SetActive(true);
            }
            else if (counter == 8)
            {
                _myQoomonCard.SetActive(true);
                highestPowerWin.SetActive(false);
                winLocations.SetActive(true);
                input.gameObject.SetActive(true);
            }
            else if(counter == 9)
            {
                winLocations.SetActive(false);
                isAddsPowerAndHighestPowerPanelShowen = true;
            
                input.gameObject.SetActive(false);
                yield return new WaitUntil(() => GameplayManager.Instance.GameplayState == GameplayState.Playing);
                yield return new WaitForSeconds(2);
                cardsSpecialAbilities.SetActive(true);
                
                //CardDetailsPanel.OnClose +=  
                
            }
            else if (counter == 10)
            {
                yield return new WaitForSeconds(4);
                cardsSpecialAbilities.SetActive(false);
                hoverWinLocations.SetActive(true);
                input.gameObject.SetActive(true);
            }
            else if (counter == 11)
            {
                input.gameObject.SetActive(false);
                hoverWinLocations.SetActive(false);
                goldiPart.SetActive(true);
                laneMid.SetActive(true);
                darkLayerQoomonsInHandMid.SetActive(true);
                yield return new WaitUntil(() => GameplayManager.Instance.CurrentRound == 3);
                hoverWinLocationsText.text = "";
                goldiPart.SetActive(false);
                darkLayerQoomonsInHandMid.SetActive(false);
                yield return new WaitUntil(() => GameplayManager.Instance.GameplayState == GameplayState.Playing);
                stakeFirstPart.SetActive(true);
                Next();
            }
            else if (counter == 12)
            {
                yield return new WaitUntil(() => BetClickHandler.Instance.DidIBetThisRound);
                stakeFirstPart.SetActive(false);
                yield return new WaitForSeconds(6);
                stakeSecondPart.SetActive(true);
                input.gameObject.SetActive(true);
            }
            else if(counter == 13)
            {
                samuKitsunePart.SetActive(true);
                darkLayerQoomonsInHandBot.SetActive(true);
                laneBot.SetActive(true);
                stakeSecondPart.SetActive(false);
                input.gameObject.SetActive(false);
                PlayTutorialCards.OnNextStep += Next;
            }
            else if(counter == 14)
            {
                yield return new WaitUntil(() => GameplayManager.Instance.CurrentRound == 4 && GameplayManager.Instance.GameplayState == GameplayState.Playing);
                samuKitsunePart.SetActive(false);
                darkLayerQoomonsInHandBot.SetActive(false);
                laneBot.SetActive(false);
                mukongPart.SetActive(true);
                darkLayerQoomonsInHandMid.SetActive(true);
                laneMid.SetActive(true);
               
            }
            else if(counter == 15)
            {
                yield return new WaitUntil(() => GameplayManager.Instance.CurrentRound == 5 && GameplayManager.Instance.GameplayState == GameplayState.Playing);
                mukongPart.SetActive(false);
                darkLayerQoomonsInHandMid.SetActive(false);
                geishaKoPart.SetActive(true);
                darkLayerQoomonsInHandMid.SetActive(true);
                laneMid.SetActive(true);
               
            }
            else if(counter == 16)
            {
                yield return new WaitUntil(() => GameplayManager.Instance.CurrentRound == 6 && GameplayManager.Instance.GameplayState == GameplayState.Playing);
                geishaKoPart.SetActive(false);
                darkLayerQoomonsInHandMid.SetActive(false);
                laneMid.SetActive(false);
                satiTheTigarPart.SetActive(true);
                darkLayerQoomonsInHandBot.SetActive(true);
                laneBot.SetActive(true);
               
                
            }
            else if (counter == 17)
            {
                PlayTutorialCards.OnNextStep -= Next;
            }
            
        }
    }
}

