using UnityEngine;
using Syuntoku.DigMode;

/// <summary>
/// ワールドのデータを設定
/// </summary>
[CreateAssetMenu(menuName = "Scriptable/Create WorldSetting")]
public class WorldSettingScriptable : ScriptableObject
{
    [Header("seed値")]
    public int seed;

    public int WORLD_SETTING_DEFAULT;
    public int enableChankDistance;
    public float homeLightPower;
    public float digLightPower;

    [Header("ワールドの大きさ　大きさ/１０マスの配列を用意")]
    public WorldSize[] worldSizes;
    [Header("どの岩をアクティブするかなどの設定")]
    public GenerateSetting[] generateSettings;
}

[System.Serializable]
public class WorldSize
{
    public string sizeSettingName;
    public Index3D size;
}

[System.Serializable]
public class GenerateSetting
{
    public int hirarchyMin;
    public int hirarchyMax;
    [Header("ブロックの設定")]
    public Status[] blockstatus;
    [Header("宝石の設定")]
    public Status[] juwelrystatus;
}

[System.Serializable]
public class Status
{
    public string name;
    public bool active;
}

