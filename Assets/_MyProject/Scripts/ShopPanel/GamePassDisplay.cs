using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePassDisplay : MonoBehaviour
{
   public Action<GamePassDisplay> OnSelected;
   
   [SerializeField] private Image imageDisplay;
   [SerializeField] private TextMeshProUGUI costDisplay;
   [SerializeField] private Button button;
   [SerializeField] private GameObject shadow;
   private GamePassOffer offer;

   public GamePassOffer Offer => offer;

   private void OnEnable()
   {
      button.onClick.AddListener(Select);
   }

   private void OnDisable()
   {
      button.onClick.RemoveListener(Select);
   }

   private void Select()
   {
      OnSelected?.Invoke(this);
   }

   public void Setup(GamePassOffer _offer)
   {
      offer = _offer;
      imageDisplay.sprite = offer.GamePass.Sprite;
      costDisplay.text = offer.GamePass.StorageSize.ToString();
   }

   public void ShowAsSelected()
   {
      transform.localScale = new Vector3(1.2f, 1.2f, 1f);
      shadow.SetActive(false);
   }

   public void ShowAsDeselected()
   {
      transform.localScale = Vector3.one;
      shadow.SetActive(true);
   }
}
