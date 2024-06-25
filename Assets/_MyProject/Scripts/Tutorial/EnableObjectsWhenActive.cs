using System.Collections.Generic;
using UnityEngine;

public class EnableObjectsWhenActive : MonoBehaviour
{
   [SerializeField] private List<GameObject> objectsToActivate;

   private void OnEnable()
   {
      ManageObjects(true);
   }

   private void OnDisable()
   {
      ManageObjects(false);
   }

   private void ManageObjects(bool _status)
   {
      foreach (var _gameObject in objectsToActivate)
      {
         _gameObject.SetActive(_status);
      }
   }
}
