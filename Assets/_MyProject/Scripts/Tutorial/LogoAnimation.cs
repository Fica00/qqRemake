using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial
{
    public class LogoAnimation : MonoBehaviour
    {
        [SerializeField] private Image logo;
        [SerializeField] private TextMeshProUGUI welcomeLabel;

        public void Setup(Action _callBack)
        {
            gameObject.SetActive(true);
            logo.SetAlphaZero();
            welcomeLabel.SetAlphaZero();

            Utils.DoColor(welcomeLabel,1,1,1);
            Utils.DoColor(logo,1,1,1, () =>
            {
                Utils.DoScale(logo,new Vector3(3,3,3), 1,1, () =>
                {
                    gameObject.SetActive(false);
                    _callBack?.Invoke();
                });
                
                Utils.DoColor(logo,0,1,1);
                Utils.DoColor(welcomeLabel,0,1,1);
            });
        }
    }
}