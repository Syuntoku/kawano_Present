using UnityEngine;
using Syuntoku.DigMode;

/// <summary>
/// ���[���h�̃f�[�^��ݒ�
/// </summary>
[CreateAssetMenu(menuName = "Scriptable/Create WorldSetting")]
public class WorldSettingScriptable : ScriptableObject
{
    [Header("seed�l")]
    public int seed;

    public int WORLD_SETTING_DEFAULT;
    public int enableChankDistance;
    public float homeLightPower;
    public float digLightPower;

    [Header("���[���h�̑傫���@�傫��/�P�O�}�X�̔z���p��")]
    public WorldSize[] worldSizes;
    [Header("�ǂ̊���A�N�e�B�u���邩�Ȃǂ̐ݒ�")]
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
    [Header("�u���b�N�̐ݒ�")]
    public Status[] blockstatus;
    [Header("��΂̐ݒ�")]
    public Status[] juwelrystatus;
}

[System.Serializable]
public class Status
{
    public string name;
    public bool active;
}

