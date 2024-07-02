using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Tutorial
{
    public class TutorialManager : TutorialMessage
    {
        [SerializeField] private GameObject parentGameObject;  
        [SerializeField] private GameObject qommonsHighlight;  
        [SerializeField] private GameObject qommonsCantPlay;       
        [SerializeField] private GameObject endTurnHighlight;
        [SerializeField] private GameObject qoomonAddsPower; 
        [SerializeField] private GameObject highestPowerWin;
        [SerializeField] private GameObject winLocations;
        [SerializeField] private GameObject gainMana;
        [SerializeField] private GameObject cardsSpecialAbilities;
        [SerializeField] private GameObject hoverWinLocations;
        [SerializeField] private GameObject goldiPart;
        [SerializeField] private GameObject stakeFirstPart;
        [SerializeField] private GameObject stakeSecondPart;
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
        
        [SerializeField] private GameObject EndGameButtonBlocator;
        
        [SerializeField] private GameObject laneTop;
        [SerializeField] private GameObject laneMid;
        [SerializeField] private GameObject laneBot;

        [SerializeField] private GameObject darkOverlayPrefab;
         [SerializeField] private List<CardObject> QQCardsList = new List<CardObject>();

        [SerializeField] private GameObject myCoomonPlaces;
        [SerializeField] private GameObject opponentCommonPlaces;

        [SerializeField] private GameObject placeHolderObject;
        
        //Qoomons id;
        [SerializeField] private int sammieID = 1;
        [SerializeField] private int goldieID = 3;
        [SerializeField] private int samuKitsuneID = 8;
        [SerializeField] private int mukongID = 7;
        [SerializeField] private int geishaKoID = 9;
        [SerializeField] private int tigarID = 5;
        
        private GameObject _myQoomonCard;
        private GameObject _opponentQoomonCard;
        
        private int counter;
        private Coroutine coroutineTutorial;
        
        
        public bool isAddsPowerAndHighestPowerPanelShowen;

        private void OnEnable()
        {
            input.onClick.AddListener(Next);
            PlayTutorialCards.OnCardPlacedCorrected += TurnOfGoParts;
            GameplayTutorial.OnDrawSecondTwoCards += AddQoomonCardInList;
            GameplayPlayer.RemovedCardFromTable += DisplayCancledCard;
            GameplayPlayer.AddedCardToTable += TurnOffReturnToHand;
        }

        private void TurnOffReturnToHand(PlaceCommand _command)
        {
            Debug.Log("TurnOffToHand");
            _command.Card.CanChangePlace = false;
            _command.Card.GetComponent<CardInteractions>().CanDrag = false;
        }

        private void AddQoomonCardInList(CardObject _card)
        {
            if (!_card.IsMy)
            {
                return;
            }

            Debug.Log("Card is going to hand"+_card.Details.Id);
            if (_card.Details.Id == 1)
            {
                Debug.Log(1);
                return;
            }
            Debug.Log(2);
            _card.GetComponent<CardInteractions>().CanDrag = false;
            QQCardsList.Add(_card);
        }

        private void TurnOffDrag()
        {
            foreach (var _card in QQCardsList)
            {
                _card.GetComponent<CardInteractions>().CanDrag = false;
            }
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
               // hoverWinLocations.SetActive(false);
                EndTurnHandler.OnEndTurn -= EndTurnGoldi;
            }
        }

        private void EndTurn()
        {
            endTurnHighlight.SetActive(false); ;
            counter = 6;
            coroutineTutorial = StartCoroutine(ShowStep());
            EndTurnHandler.OnEndTurn -= EndTurn;
        }

        private void OnDisable()
        {
            input.onClick.RemoveListener(Next);
            PlayTutorialCards.OnCardPlacedCorrected -= TurnOfGoParts;
        }

        private void OnShowAbility(CardObject _cardObject)
        {
            if (counter == 9)
            {
                cardsSpecialAbilities.SetActive(false);
            }
        }

        private void OnCLoseAbitliy()
        {
            CardInteractions.OnClicked -= OnShowAbility;
            CardDetailsPanel.OnClose -= OnCLoseAbitliy;
            Next();
        }

        private void Next()
        {
            TurnOfGoParts();
            counter++;
            coroutineTutorial = StartCoroutine(ShowStep());
        }

        private void TurnOfGoParts()
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
            placeHolderObject.SetActive(false);
        }

        public override void Setup()
        {
            base.Setup();
            counter = 0;
            coroutineTutorial = StartCoroutine(ShowStep());
            gameObject.SetActive(true);
           
            
        }

        private IEnumerator ShowStep()
        {
            if (counter==0)
            {
                GameplayTutorial.OnDrawSecondTwoCards -= AddQoomonCardInList;
                GameplayPlayer.DrewCard += AddQoomonCardInList;
                yield return new WaitForSeconds(2);
                input.gameObject.SetActive(true);
                qommonsHighlight.SetActive(true);
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
                
            }
            else if(counter == 3)
            {
                PlayerStatsDisplay.OnPlayerNameClicked += OnPlayerClicked;
                qommonsCantPlay.SetActive(false);
                input.gameObject.SetActive(false);
                playerNameHolder.SetActive(true);
                playerNameText.text = DataManager.Instance.PlayerData.Name;
                EndTurnHandler.OnEndTurn += EndTurn;
            }
            else if (counter == 4)
            {
                
                PlayerStatsDisplay.OnPlayerStatsClose += OnPlayerStatsClosed;
                playerNameHolder.SetActive(false);
                statsDarkerLayer.SetActive(true);
                playerNameStats.SetActive(true);
                playerNameStatsText.text =  DataManager.Instance.PlayerData.Name;
                
                
            }
            else if (counter==5) // Odavde nastaviti na gore sa brojevima
            {
                EndGameButtonBlocator.SetActive(false);
                statsDarkerLayer.SetActive(false);
                playerNameStats.SetActive(false);
                endTurnHighlight.SetActive(true);
               
            }
            if (counter == 6)
            {
                yield return new WaitUntil(() => GameplayTutorial.Instance.cardsPlayed);
                _opponentQoomonCard = opponentCommonPlaces.GetComponentInChildren<CardObject>().gameObject;
                _opponentQoomonCard.SetActive(false);
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
                CardInteractions.OnClicked += OnShowAbility;
                winLocations.SetActive(false);
                hoverWinLocations.SetActive(true);
                isAddsPowerAndHighestPowerPanelShowen = true;
                input.gameObject.SetActive(false);
                yield return new WaitUntil(() => GameplayManager.Instance.GameplayState == GameplayState.Playing);
                yield return new WaitForSeconds(2);
                TurnOffDrag();
                cardsSpecialAbilities.SetActive(true);
                QQCardsList.Single(x=>x.Details.Id == goldieID).gameObject.transform.SetParent(parentGameObject.transform);
                QQCardsList.Single(x => x.Details.Id == goldieID).GetComponent<CardInteractions>().CanDrag = false;
                
               // TurnOfDarkLayerOverQoomonById(goldieID);      
               CardDetailsPanel.OnClose += OnCLoseAbitliy;

            }
            else if (counter == 10)
            {
                yield return new WaitForSeconds(1);
                cardsSpecialAbilities.SetActive(false);
                Next();
                // hoverWinLocations.SetActive(true);
                // input.gameObject.SetActive(true);
               
            }
            else if (counter == 11)
            {
                // input.gameObject.SetActive(false);
                // hoverWinLocations.SetActive(false);
                QQCardsList.Single(x => x.Details.Id == goldieID).GetComponent<CardInteractions>().CanDrag = true;
                
                EndTurnHandler.OnEndTurn += EndTurnGoldi;
                goldiPart.SetActive(true);
                laneMid.SetActive(true);
                darkLayerQoomonsInHandMid.SetActive(true);
                yield return new WaitUntil(() => GameplayManager.Instance.CurrentRound == 3);
                TurnOffDrag();
                goldiPart.SetActive(false);
                darkLayerQoomonsInHandMid.SetActive(false);
                yield return new WaitUntil(() => GameplayManager.Instance.GameplayState == GameplayState.Playing);
                EndGameButtonBlocator.SetActive(true);
                TurnOffDrag();
                stakeFirstPart.SetActive(true);
                Next();
                
            }
            else if (counter == 12)
            {
                
                yield return new WaitUntil(() => BetClickHandler.Instance.DidIBetThisRound);
                stakeFirstPart.SetActive(false);
                yield return new WaitForSeconds(6);
                TurnOffDrag();
                stakeSecondPart.SetActive(true);
                input.gameObject.SetActive(true);
            }
            else if(counter == 13)
            {
                QQCardsList.Single(x => x.Details.Id == samuKitsuneID).GetComponent<CardInteractions>().CanDrag = true;
                EndGameButtonBlocator.SetActive(false);
                samuKitsunePart.SetActive(true);
                placeHolderObject.SetActive(true);
                placeHolderObject.transform.SetSiblingIndex(3);
                darkLayerQoomonsInHandBot.SetActive(true);
                QQCardsList.Single(x=>x.Details.Id == samuKitsuneID).gameObject.transform.SetParent(parentGameObject.transform);
                
                laneBot.SetActive(true);
                stakeSecondPart.SetActive(false);
                input.gameObject.SetActive(false);
                PlayTutorialCards.OnNextStep += Next;
            }
            else if(counter == 14)
            {
                yield return new WaitUntil(() => GameplayManager.Instance.CurrentRound == 4 && GameplayManager.Instance.GameplayState == GameplayState.Playing);
                TurnOffDrag();
                samuKitsunePart.SetActive(false);
                darkLayerQoomonsInHandBot.SetActive(false);
                laneBot.SetActive(false);
                CardObject _cardMukong = QQCardsList.Single(x => x.Details.Id == mukongID);
                _cardMukong.GetComponent<CardInteractions>().CanDrag = true;
                _cardMukong.gameObject.transform.SetParent(parentGameObject.transform);
                mukongPart.SetActive(true);
                darkLayerQoomonsInHandMid.SetActive(true);
                laneMid.SetActive(true);
               
            }
            else if(counter == 15)
            {
                yield return new WaitUntil(() => GameplayManager.Instance.CurrentRound == 5 && GameplayManager.Instance.GameplayState == GameplayState.Playing);
                TurnOffDrag();
                mukongPart.SetActive(false);
                darkLayerQoomonsInHandMid.SetActive(false);
                QQCardsList.Single(x => x.Details.Id == geishaKoID).GetComponent<CardInteractions>().CanDrag = true;
                QQCardsList.Single(x=>x.Details.Id == geishaKoID).gameObject.transform.SetParent(parentGameObject.transform);
                geishaKoPart.SetActive(true);
                placeHolderObject.SetActive(true);
                placeHolderObject.transform.SetSiblingIndex(4);
                darkLayerQoomonsInHandMid.SetActive(true);
                laneMid.SetActive(true);
               
            }
            else if(counter == 16)
            {
                yield return new WaitUntil(() => GameplayManager.Instance.CurrentRound == 6 && GameplayManager.Instance.GameplayState == GameplayState.Playing);
                TurnOffDrag();
                geishaKoPart.SetActive(false);
                darkLayerQoomonsInHandMid.SetActive(false);
                laneMid.SetActive(false);
                QQCardsList.Single(x => x.Details.Id == tigarID).GetComponent<CardInteractions>().CanDrag = true;
                QQCardsList.Single(x=>x.Details.Id == tigarID).gameObject.transform.SetParent(parentGameObject.transform);
                satiTheTigarPart.SetActive(true);
                placeHolderObject.SetActive(true);
                placeHolderObject.transform.SetSiblingIndex(1);
                darkLayerQoomonsInHandBot.SetActive(true);
                laneBot.SetActive(true);
               
                
            }
            else if (counter == 17)
            {
                hoverWinLocations.SetActive(false);
                PlayTutorialCards.OnNextStep -= Next;
                GameplayPlayer.DrewCard -= AddQoomonCardInList;
                GameplayPlayer.RemovedCardFromTable -= DisplayCancledCard;
                GameplayPlayer.AddedCardToTable -= TurnOffReturnToHand;
            }
            
        }

        private void DisplayCancledCard(PlaceCommand _command)
        {
            Coroutine _startDisplayCardCoroutine = StartCoroutine(DisplayCancledCardCoroutine(_command));
        }

        private IEnumerator DisplayCancledCardCoroutine(PlaceCommand _command)
        {
            Debug.Log("Card returned to Hand");
            switch (_command.Card.Details.Id)
            {
                case 1:
                    //_command.Card.gameObject.transform.SetSiblingIndex(0);
                    yield return new WaitForSeconds(1);
                    _command.Card.gameObject.transform.SetParent(parentGameObject.transform);
                    break;
                case 3:
                   // _command.Card.gameObject.transform.SetSiblingIndex(2);
                    yield return new WaitForSeconds(1);
                    _command.Card.gameObject.transform.SetParent(parentGameObject.transform);
                    placeHolderObject.transform.SetSiblingIndex(2);
                    break;
                case 8:
                    _command.Card.gameObject.transform.SetSiblingIndex(3);
                    yield return new WaitForSeconds(1);
                    _command.Card.gameObject.transform.SetParent(parentGameObject.transform);
                    placeHolderObject.transform.SetSiblingIndex(3);
                    break;
                case 7:
                    //_command.Card.gameObject.transform.SetSiblingIndex(4);
                    yield return new WaitForSeconds(1);
                    _command.Card.gameObject.transform.SetParent(parentGameObject.transform);
                    placeHolderObject.transform.SetSiblingIndex(4);
                    break;
                case 9:
                    _command.Card.gameObject.transform.SetSiblingIndex(4);
                    yield return new WaitForSeconds(1);
                    _command.Card.gameObject.transform.SetParent(parentGameObject.transform);
                    placeHolderObject.transform.SetSiblingIndex(4);
                    break;
                case 5:
                    _command.Card.gameObject.transform.SetSiblingIndex(1);
                    yield return new WaitForSeconds(1);
                    _command.Card.gameObject.transform.SetParent(parentGameObject.transform);
                    placeHolderObject.transform.SetSiblingIndex(1);
                    break;
            }
        }
    }
}

