using System;
using UnityEngine;
using UnityEngine.UI;

public class FilterHandler : MonoBehaviour
{
    [SerializeField] private Button byName;
    [SerializeField] private Button byMana;
    [SerializeField] private Button byPower;
    [SerializeField] private Button close;

    public static Action OnUpdatedFilter;
    public static FilterType FilterType;
    
    public void Setup()
    {
        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        byName.onClick.AddListener(SortByName);
        byMana.onClick.AddListener(SortByMana);
        byPower.onClick.AddListener(SortByPower);
        close.onClick.AddListener(Close);
    }

    private void OnDisable()
    {
        byName.onClick.RemoveListener(SortByName);
        byMana.onClick.RemoveListener(SortByMana);
        byPower.onClick.RemoveListener(SortByPower);
        close.onClick.RemoveListener(Close);
    }

    private void SortByName()
    {
        SetSort(FilterType.ByName);
    }

    private void SortByMana()
    {
        SetSort(FilterType.ByMana);
    }
    
    private void SortByPower()
    {
        SetSort(FilterType.ByPower);
    }

    private void SetSort(FilterType _filter)
    {
        FilterType = _filter;
        OnUpdatedFilter?.Invoke();
        Close();
    }

    private void Close()
    {
        gameObject.SetActive(false);
    }
}