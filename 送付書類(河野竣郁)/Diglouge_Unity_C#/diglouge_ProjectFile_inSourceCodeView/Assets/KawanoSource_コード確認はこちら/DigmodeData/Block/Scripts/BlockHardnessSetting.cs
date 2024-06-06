using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Create BlockHardnesSetting")]
public class BlockHardnessSetting : ScriptableObject
{
    [Header("�z�΂̊�b�@HP")]
    [Header("��bHP�Ɋe�u���b�N��HP�𑫂��Čv�Z���܂�")]
    public HardnessSetting[] hardnessSettings;
}

[System.Serializable]
public class HardnessSetting
{
    public int Depth;
    public int hardness;
}

