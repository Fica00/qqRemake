using UnityEngine;
using UnityEngine.UI;
public class OverlayHandler : MonoBehaviour
{
    [SerializeField] protected GameObject OverlayGameObject;
    [SerializeField] protected Button closeButton;

    public virtual void Setup()
    {
        OverlayGameObject.SetActive(true);
    }

    public virtual void Close() => OverlayGameObject.SetActive(false);
}