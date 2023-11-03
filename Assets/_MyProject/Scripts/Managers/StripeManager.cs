using System;
using System.Collections;
using UnityEngine;

public class StripeManager : MonoBehaviour
{
    public static StripeManager Instance;

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

    public void Purchase(double _cost, Action<PurchaseResponse> _callBack)
    {
        GameObject _loading = Instantiate(AssetsManager.Instance.Loading, null);
        StartCoroutine(BuyRoutine());
        IEnumerator BuyRoutine()
        {
            yield return new WaitForSeconds(1);
            _callBack?.Invoke(new PurchaseResponse()
            {
                Successfully = true
            });
            Destroy(_loading);
        }
    }
}
