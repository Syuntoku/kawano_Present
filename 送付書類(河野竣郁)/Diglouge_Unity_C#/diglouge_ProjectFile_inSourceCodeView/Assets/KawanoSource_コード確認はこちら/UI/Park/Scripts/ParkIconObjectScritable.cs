using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Create ParkIconObjectData")]
public class ParkIconObjectScritable : ScriptableObject
{
    public ParkIcon[] parkIconData;

    [SerializeField] public Material level1Material;
    [SerializeField] public Material level2Material;
    [SerializeField] public Material level3Material;

    public Dictionary<int , ParkIcon> _parkIconDirectionaryData;

    public void SetDirectionary()
    {
        _parkIconDirectionaryData = new Dictionary<int, ParkIcon>();
        foreach (ParkIcon item in parkIconData)
        {
            _parkIconDirectionaryData.Add(item.parkId,item);
        }
    }
}

[System.Serializable]
public class ParkIcon
{
    public int parkId;
    public GameObject IconObject;
}
