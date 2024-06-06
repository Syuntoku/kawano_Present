using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Create BlockHardnesSetting")]
public class BlockHardnessSetting : ScriptableObject
{
    [Header("zÎ‚ÌŠî‘b@HP")]
    [Header("Šî‘bHP‚ÉŠeƒuƒƒbƒN‚ÌHP‚ğ‘«‚µ‚ÄŒvZ‚µ‚Ü‚·")]
    public HardnessSetting[] hardnessSettings;
}

[System.Serializable]
public class HardnessSetting
{
    public int Depth;
    public int hardness;
}

