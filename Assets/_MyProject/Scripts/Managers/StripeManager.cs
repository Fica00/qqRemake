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

    public void PurchaseResult(PurchaseResponseServer _response)
    {
        PurchaseResponse _clientResponse = new PurchaseResponse(){Successfully = _response.Result==1, Message = 
        _response.Message};
        callBack?.Invoke(_clientResponse);
        Destroy(loading);
    }
}
