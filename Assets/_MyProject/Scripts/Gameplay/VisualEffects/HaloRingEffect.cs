using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HaloRingEffect : MonoBehaviour
{
   [SerializeField] private Sprite[] sprites;
   private Image imageDisplay;

   private void Awake()
   {
      imageDisplay = GetComponent<Image>();
   }

   private IEnumerator Start()
   {
      foreach (var _sprite in sprites)
      {
         imageDisplay.sprite = _sprite;
         yield return new WaitForSeconds(0.3f);
      }
      
      Destroy(gameObject);
   }
}
