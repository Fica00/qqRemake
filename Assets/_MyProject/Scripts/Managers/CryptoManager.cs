using System;
using System.Collections;
using UnityEngine;

public class CryptoManager : MonoBehaviour
{
   public static CryptoManager Instance;

   private void Awake()
   {
      if (Instance==null)
      {
         Instance = this;
      }
      else
      {
         Destroy(gameObject);
      }
   }

   public void Purchase(double _cost,string _playerId, Action<PurchaseResponse> _callBack)
   {
      GameObject _loading = Instantiate(AssetsManager.Instance.Loading, null);
      StartCoroutine(BuyRoutine());
      IEnumerator BuyRoutine()
      {
         yield return new WaitForSeconds(1);
         FirebaseManager.Instance.AddUSDCToPlayer(_cost, _playerId, (_status) =>
         {
            if (_status)
            {
               _callBack?.Invoke(new PurchaseResponse { Message = string.Empty, Result = PurchaseResult.Successful });
            }
            else
            {
               DialogsManager.Instance.OkDialog.Setup("Something went wrong while sending founds to owner, please contract our support");
               _callBack?.Invoke(new PurchaseResponse { Message = string.Empty, Result = PurchaseResult.Failed });
            }
         });
        
         Destroy(_loading);
      }
   }
}
