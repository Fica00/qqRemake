using System;
using UnityEngine;

public class BasePanel : MonoBehaviour
{
    public virtual void Show()
    {
        throw new Exception();
    }

    public virtual void Close()
    {
        throw new Exception();
    }
}
