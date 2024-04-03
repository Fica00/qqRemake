using System;
using System.Collections.Generic;
using UnityEngine;

public class SpriteProvider : MonoBehaviour
{
    public static SpriteProvider Instance;
    [SerializeField] private List<ItemSprite> items;

    private void Awake()
    {
        if (Instance==null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Sprite Get(ItemType _itemType)
    {
        foreach (var _item in items)
        {
            if (_item.ItemType == _itemType)
            {
                return _item.Sprite;
            }
        }

        throw new Exception();
    }

    public Sprite GetQoomonSprite(int _qoomonId)
    {
        return CardsManager.Instance.GetCardObject(_qoomonId).Details.Sprite;
    }
}