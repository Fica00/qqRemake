using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseResultDisplay : MonoBehaviour
{
    public static PurchaseResultDisplay Instance;

    [SerializeField] private GameObject holder;
    [SerializeField] private Button closeButton;
    [SerializeField] private Image resultImage;
    [SerializeField] private TextMeshProUGUI resultDisplay;
    [SerializeField] private List<PurchaseImage> images;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        closeButton.onClick.AddListener(Close);
    }

    private void OnDisable()
    {
        closeButton.onClick.RemoveListener(Close);
    }

    private void Close()
    {
        holder.SetActive(false);
    }

    public void Setup(PurchaseResponse _response)
    {
        resultImage.sprite = images.Find(_image => _image.Result == _response.Result).Sprite;
        resultDisplay.text = _response.Message;
        holder.SetActive(true);
    }
}
