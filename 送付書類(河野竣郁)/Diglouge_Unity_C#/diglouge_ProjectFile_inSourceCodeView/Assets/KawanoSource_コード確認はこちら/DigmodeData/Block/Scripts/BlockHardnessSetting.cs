using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Create BlockHardnesSetting")]
public class BlockHardnessSetting : ScriptableObject
{
    [Header("鉱石の基礎　HP")]
    [Header("基礎HPに各ブロックのHPを足して計算します")]
    public HardnessSetting[] hardnessSettings;
}

[System.Serializable]
public class HardnessSetting
{
    public int Depth;
    public int hardness;
}

