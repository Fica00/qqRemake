using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial
{
    public class MatchMaking : MonoBehaviour
    {
        [SerializeField] private Image matchBackground;
        [SerializeField] private TextMeshProUGUI matchText;
        [SerializeField] private RectTransform matchPosition;
        [SerializeField] private MatchMakingPlayerDisplay myPlayer;
        [SerializeField] private MatchMakingPlayerDisplay opponentPlayer;
        [SerializeField] private Image vsImage;
        [SerializeField] private RectTransform myPlayerPosition;
        [SerializeField] private TextMeshProUGUI searchingForOpponent;
        [SerializeField] private RectTransform opponentPlayerPosition;

        public static string OpponentsName = "Opponents name";
        public static string OpponentsDeck = "Opponents deck";
        
        private Action callBack;
        
        public void Setup(Action _callBack)
        {
            myPlayer.transform.localScale = Vector3.zero;
            opponentPlayer.transform.localScale = Vector3.zero;
            vsImage.SetAlphaZero();
            searchingForOpponent.SetAlphaZero();
            callBack = _callBack;
            gameObject.SetActive(true);
            AnimateMatchLabel();
        }

        private void AnimateMatchLabel()
        {
            matchBackground.SetAlphaZero();
            matchText.SetAlphaZero();
            
            Utils.DoColor(matchBackground,1,1,0);
            Utils.DoColor(matchText,1,1,0, () =>
            {
                Utils.DoScale(matchBackground, new Vector3(0.8f,0.8f,0.8f), 1,1);
                matchBackground.transform.DOMove(matchPosition.position, 2).SetDelay(1).OnComplete(ShowPlayers);
            });
        }

        private void ShowPlayers()
        {
            Utils.DoColor(vsImage,1,1,0, () =>
            {
                myPlayer.Setup(DataManager.Instance.PlayerData.Name, DataManager.Instance.PlayerData.GetSelectedDeck().Name);
                myPlayer.transform.localScale = Vector3.one;
                Utils.DoColor(searchingForOpponent,1,1,0);
                myPlayer.transform.DOMove(myPlayerPosition.position, 1).OnComplete(() =>
                {
                    opponentPlayer.Setup(OpponentsName, OpponentsDeck);
                    Utils.DoColor(searchingForOpponent,1,0,3, () =>
                    {
                        opponentPlayer.transform.localScale = Vector3.one;
                    });
                    opponentPlayer.transform.DOMove(opponentPlayerPosition.position, 1).SetDelay(3).OnComplete(() =>
                    {
                        int _delay = 2;
                        Utils.DoScale(opponentPlayer.gameObject,Vector3.zero, 1,_delay);
                        Utils.DoScale(myPlayer.gameObject,Vector3.zero, 1,_delay);
                        Utils.DoColor(vsImage,0,1,_delay);
                        Utils.DoScale(matchBackground,Vector3.zero,1,_delay, () =>
                        {
                            gameObject.SetActive(false);
                            callBack?.Invoke();
                        });
                    });
                });
            });
        }
    }
}