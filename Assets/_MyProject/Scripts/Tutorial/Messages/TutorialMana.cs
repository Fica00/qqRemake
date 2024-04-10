using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial
{
    public class TutorialMana : TutorialMessage
    {
        [SerializeField] private GameObject qommonsHighlight;
        [SerializeField] private TextMeshProUGUI qommonsText;
        [SerializeField] private GameObject endTurnHighlight;
        [SerializeField] private TextMeshProUGUI endTurnText;
        [SerializeField] private Button input;

        private int counter;

        private void OnEnable()
        {
            input.onClick.AddListener(Next);
        }

        private void OnDisable()
        {
            input.onClick.RemoveListener(Next);
        }

        private void Next()
        {
            counter++;
            ShowStep();
        }

        public override void Setup()
        {
            base.Setup();
            counter = 0;
            ShowStep();
            gameObject.SetActive(true);
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
                qommonsText.text = "You can't play those now";
            }
            else if (counter==2)
            {
                qommonsHighlight.SetActive(false);
                endTurnHighlight.SetActive(true);
                endTurnText.text = "Press the end turn button";
            }
            else if (counter == 3)
            {
                gameObject.SetActive(false);
            }
        }
    }
}

