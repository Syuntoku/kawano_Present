using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Syuntoku.DigMode.Tool;


[CreateAssetMenu(menuName = "Scriptable/UniqueCharacteristicsScriptable")]
public class UniqueCharacteristicsScriptable : ScriptableObject
{
    public string note;

    public UniqueSetting[] uniqueSettings;

    [Header("�c�[���̌ŗL�����̐ݒ�")]
    [Header("��͂�")]
    public PickAxeUniqueCharacteristics pickAxe;

    [Header("�n���}�[")]
    public HammarUniqueCharacteristics hammer;

    [Header("�e")]
    public GunUniqueCharacteristics gun;
}

[System.Serializable]
public class PickAxeUniqueCharacteristics
{
    [Header("�m�[�}��")]

    [Header("������ �ŗL�P"), Multiline(3)]
    public string normal1_explain;

    [Header("���I�m��")]
    public int normal1_popupProbability;

    [Header("�_���[�W�A�b�v�{��")]
    public int normal1_blockDamageMagnification;

    [Header("������ �ŗL2"), Multiline(3)]
    public string normal2_explain2;

    [Header("���I�m��")]
    public int normal2_popupProbability;

    [Header("up�m��")]
    public int normal2_DamageUpProbability;
    [Header("�����{��")]
    public int normal2_DamageUpMagnification;

    [Header("���A")]

    [Header("������ �ŗL�P"), Multiline(3)]
    public string rare1_explain;


    [Header("���I�m��")]
    public int rare1_popupProbability;

    [Header("���j��m��")]
    public int rare1_immediatelyBreakProbability;

    [Header("������ �ŗL2"), Multiline(3)]
    public string rare2_Explain2;

    [Header("���I�m��")]
    public int rare2_popupProbability;

    [Header("����")]
    public int rare2_Time;
    [Header("�ړ����x�{��")]
    public int rare2_PlayerSpeed;
    
    [Header("���W�F���h")]

    [Header("������ �ŗL�P"), Multiline(3)]
    public string legend1_explain;

    [Header("���I�m��")]
    public int legend1_popupProbability;

    [Header("����")]
    public int�@legend1_Time;
    [Header("�����{��")]
    public int�@legend1_DanageUp;

    [Header("������ �ŗL2"), Multiline(3)]
    public string legend2_Explain2;

    [Header("���I�m��")]
    public int legend2_popupProbability;

    [Header("����")]
    public int legend2_Time;
    [Header("�ړ����x�{��")]
    public int legend2_PlayerSpeed;

}

[System.Serializable]
public class HammarUniqueCharacteristics
{

    [Header("�m�[�}��")]

    [Header("������ �ŗL�P"), Multiline(3)]
    public string normal1_explain;


    [Header("���I�m��")]
    public int noamal1_popupProbability;

    [Header("�����m��")]
    public int noamal1_DamageUpProbability;

    [Header("������ �ŗL�Q"), Multiline(3)]
    public string�@normal2_explain;

    [Header("���I�m��")]
    public int noamal2_popupProbability;

    [Header("�����m��")]
    public int noamal2_DamageUpProbability;

    [Header("�����{��")]
    public int noamal2_DamageUpMagnifacation;

    [Header("���A")]

    [Header("������ �ŗL1"), Multiline(3)]
    public string rare1_explain;

    [Header("���I�m��")]
    public int rare1_popupProbability;

    [Header("���j��m��")]
    public int rare1_BreakProbability;

    [Header("������ �ŗL1"), Multiline(3)]
    public string rare2_explain;

    [Header("���I�m��")]
    public int rare2_popupProbability;

    [Header("����")]
    public int rare2_Time;

    [Header("�����m��")]
    public int rare2_SpeedUpMagnifacation;

    [Header("���W�F���h")]

    [Header("������ �ŗL1"), Multiline(3)]
    public string legend1_explain;

    [Header("���I�m��")]
    public int legend1_popupProbability;

    [Header("�͈̓A�b�v")]
    public int legend1_RangeUp;


    [Header("������ �ŗL1"), Multiline(3)]
    public string legend2_explain;

    [Header("���I�m��")]
    public int legend2_popupProbability;

    [Header("����")]
    public int legend2_Time;

    [Header("�N�[���^�C�������m��")]
    public int legend2_coolTime;
}

[System.Serializable]
public class GunUniqueCharacteristics
{

    [Header("�m�[�}��")]

    [Header("������ �ŗL�P"), Multiline(3)]
    public string normal1_explain;

    [Header("���I�m��")]
    public int noamal1_popupProbability;

    [Header("�_���[�W�A�b�v�{��")]
    public int noamal1_damageUpProbability;

    [Header("������ �ŗL�Q"), Multiline(3)]
    public string normal2_explain;

    [Header("���I�m��")]
    public int normal2_popupProbability;

    [Header("�e�̑��x�A�b�v�{��")]
    public int noama21_bulletSpeedup;


    [Header("���A")]

    [Header("������ �ŗL1"), Multiline(3)]
    public string rare1_explain;

    [Header("���I�m��")]
    public int rare1_popupProbability;

    [Header("���j��m��")]
    public int rare1_immediatelyBreakProbability;


    [Header("������ �ŗL1"), Multiline(3)]
    public string rare2_explain;

    [Header("���I�m��")]
    public int rare2_popupProbability;

    [Header("����")]
    public int rare2_Time;

    [Header("�{�[���X�s�[�h�A�b�v�{��")]
    public int rare2_BulletSpeedUp;

    [Header("���W�F���h")]

    [Header("������ �ŗL1"), Multiline(3)]
    public string legend1_explain;

    [Header("���I�m��")]
    public int legend1_popupProbability;

    [Header("���ˑ�����")]
    public int legend1_NumberOfShots;

    [Header("������ �ŗL2"), Multiline(3)]
    public string legend2_explain;

    [Header("���I�m��")]
    public int legend2_popupProbability;

    [Header("����")]
    public int legend2_Time;

    [Header("�e�̃X�s�[�h�A�b�v�{��")]
    public int legend2_BulletSpeedUp;

    [Header("������ �ŗL3"), Multiline(3)]
    public string legend3_explain;

    [Header("���I�m��")]
    public int legend3_popupProbability;

    [Header("�e�̔��ˑ�����")]
    public int legend3_reflectionCount;
}

[System.Serializable]
public class UniqueSetting
{

    [Header("�ŗL�����̏o���ݒ�")]
    [Header("�ݒ�̊J�n�E�F�[�u�ƍŌ�̃E�F�[�u")]
    public int startWaveCount;
    public int endWaveCount;

    [Header("�ŗL����������g���L���ɂȂ�m��")]
    public int FirstFlame;
    public int SecondFlame;
    public int ThirdFlame;
}

