using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public struct Agency
{
    public string Name;
    public int Level;
}
public class AgencyManager : MonoBehaviour
{
    public static AgencyManager Instance;

    public List<Agency> Agencies;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public bool DoesAgencyExist(string _name)
    {
        return Agencies.Any(_ => _.Name == _name);
    }
}