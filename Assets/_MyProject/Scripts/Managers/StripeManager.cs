using System;
using System.Collections;
using UnityEngine;

public class StripeManager : MonoBehaviour
{
    public static StripeManager Instance;
    private Action<PurchaseResponse> callBack;
    private GameObject loading;

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
        loading = Instantiate(AssetsManager.Instance.Loading, null);
        JavaScriptManager.Instance.StripePurchase(_cost);
        callBack = _callBack;
    }

    public void PurchaseResult(bool _result)
    {
        PurchaseResponse _response = new PurchaseResponse(){Successfully = _result};
        callBack?.Invoke(_response);
        Destroy(loading);
    }
}
