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

        public static string OpponentsName = "Ninja Frog";
        public static string OpponentsDeck = "Opponents deck";
        public static int OpponentAvatarId = 0;

        
        public static string MyName = "Sati-the-tiger";
        public static string MyDeck = "";
        public static int MyAvatarId = 3;
        
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
                matchBackground.transform.DOMove(matchPosition.position, 1).SetDelay(0.5f).OnComplete(ShowPlayers);
            });
        }

        private void ShowPlayers()
        {
            Utils.DoColor(vsImage,1,1,0, () =>
            {
                myPlayer.Setup(MyName, MyDeck, MyAvatarId);
                myPlayer.transform.localScale = Vector3.one;
                Utils.DoColor(searchingForOpponent,1,1,0);
                myPlayer.transform.DOMove(myPlayerPosition.position, 1).OnComplete(() =>
                {
                    opponentPlayer.Setup(OpponentsName, OpponentsDeck, OpponentAvatarId);
                    Utils.DoColor(searchingForOpponent,1,0,1, () =>
                    {
                        opponentPlayer.transform.localScale = Vector3.one;
                    });
                    opponentPlayer.transform.DOMove(opponentPlayerPosition.position, 1).SetDelay(1).OnComplete(() =>
                    {
                        
                        int _delay = 1;
                        Color _color = searchingForOpponent.color;
                        _color.a = 0;
                        searchingForOpponent.DOColor(_color, .2f).OnComplete(() =>
                        {
                            _color.a = 1;
                            searchingForOpponent.text = "Match Found";
                            searchingForOpponent.DOColor(_color, .2f).OnComplete(() =>
                            {
                                Utils.DoScale(opponentPlayer.gameObject,Vector3.zero, 1,_delay);
                                Utils.DoScale(myPlayer.gameObject,Vector3.zero, 1,_delay);
                                Utils.DoColor(vsImage,0,1,_delay);
                                Utils.DoColor(searchingForOpponent,0,1,_delay);
                                Utils.DoScale(matchBackground,Vector3.zero,1,_delay, () =>
                                {
                                    gameObject.SetActive(false);
                                    callBack?.Invoke();
                                });
                            });
                        });
                    });
                });
            });
        }
    }
}