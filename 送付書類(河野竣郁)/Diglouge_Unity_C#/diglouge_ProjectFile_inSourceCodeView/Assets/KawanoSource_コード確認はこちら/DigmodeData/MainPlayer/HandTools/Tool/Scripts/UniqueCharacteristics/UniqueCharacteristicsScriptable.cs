using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Syuntoku.DigMode.Tool;


[CreateAssetMenu(menuName = "Scriptable/UniqueCharacteristicsScriptable")]
public class UniqueCharacteristicsScriptable : ScriptableObject
{
    public string note;

    public UniqueSetting[] uniqueSettings;

    [Header("ツールの固有特性の設定")]
    [Header("つるはし")]
    public PickAxeUniqueCharacteristics pickAxe;

    [Header("ハンマー")]
    public HammarUniqueCharacteristics hammer;

    [Header("銃")]
    public GunUniqueCharacteristics gun;
}

[System.Serializable]
public class PickAxeUniqueCharacteristics
{
    [Header("ノーマル")]

    [Header("説明文 固有１"), Multiline(3)]
    public string normal1_explain;

    [Header("抽選確率")]
    public int normal1_popupProbability;

    [Header("ダメージアップ倍率")]
    public int normal1_blockDamageMagnification;

    [Header("説明文 固有2"), Multiline(3)]
    public string normal2_explain2;

    [Header("抽選確率")]
    public int normal2_popupProbability;

    [Header("up確率")]
    public int normal2_DamageUpProbability;
    [Header("強化倍率")]
    public int normal2_DamageUpMagnification;

    [Header("レア")]

    [Header("説明文 固有１"), Multiline(3)]
    public string rare1_explain;


    [Header("抽選確率")]
    public int rare1_popupProbability;

    [Header("即破壊確率")]
    public int rare1_immediatelyBreakProbability;

    [Header("説明文 固有2"), Multiline(3)]
    public string rare2_Explain2;

    [Header("抽選確率")]
    public int rare2_popupProbability;

    [Header("時間")]
    public int rare2_Time;
    [Header("移動速度倍率")]
    public int rare2_PlayerSpeed;
    
    [Header("レジェンド")]

    [Header("説明文 固有１"), Multiline(3)]
    public string legend1_explain;

    [Header("抽選確率")]
    public int legend1_popupProbability;

    [Header("時間")]
    public int　legend1_Time;
    [Header("強化倍率")]
    public int　legend1_DanageUp;

    [Header("説明文 固有2"), Multiline(3)]
    public string legend2_Explain2;

    [Header("抽選確率")]
    public int legend2_popupProbability;

    [Header("時間")]
    public int legend2_Time;
    [Header("移動速度倍率")]
    public int legend2_PlayerSpeed;

}

[System.Serializable]
public class HammarUniqueCharacteristics
{

    [Header("ノーマル")]

    [Header("説明文 固有１"), Multiline(3)]
    public string normal1_explain;


    [Header("抽選確率")]
    public int noamal1_popupProbability;

    [Header("強化確率")]
    public int noamal1_DamageUpProbability;

    [Header("説明文 固有２"), Multiline(3)]
    public string　normal2_explain;

    [Header("抽選確率")]
    public int noamal2_popupProbability;

    [Header("強化確率")]
    public int noamal2_DamageUpProbability;

    [Header("強化倍率")]
    public int noamal2_DamageUpMagnifacation;

    [Header("レア")]

    [Header("説明文 固有1"), Multiline(3)]
    public string rare1_explain;

    [Header("抽選確率")]
    public int rare1_popupProbability;

    [Header("即破壊確率")]
    public int rare1_BreakProbability;

    [Header("説明文 固有1"), Multiline(3)]
    public string rare2_explain;

    [Header("抽選確率")]
    public int rare2_popupProbability;

    [Header("時間")]
    public int rare2_Time;

    [Header("強化確率")]
    public int rare2_SpeedUpMagnifacation;

    [Header("レジェンド")]

    [Header("説明文 固有1"), Multiline(3)]
    public string legend1_explain;

    [Header("抽選確率")]
    public int legend1_popupProbability;

    [Header("範囲アップ")]
    public int legend1_RangeUp;


    [Header("説明文 固有1"), Multiline(3)]
    public string legend2_explain;

    [Header("抽選確率")]
    public int legend2_popupProbability;

    [Header("時間")]
    public int legend2_Time;

    [Header("クールタイム減少確率")]
    public int legend2_coolTime;
}

[System.Serializable]
public class GunUniqueCharacteristics
{

    [Header("ノーマル")]

    [Header("説明文 固有１"), Multiline(3)]
    public string normal1_explain;

    [Header("抽選確率")]
    public int noamal1_popupProbability;

    [Header("ダメージアップ倍率")]
    public int noamal1_damageUpProbability;

    [Header("説明文 固有２"), Multiline(3)]
    public string normal2_explain;

    [Header("抽選確率")]
    public int normal2_popupProbability;

    [Header("弾の速度アップ倍率")]
    public int noama21_bulletSpeedup;


    [Header("レア")]

    [Header("説明文 固有1"), Multiline(3)]
    public string rare1_explain;

    [Header("抽選確率")]
    public int rare1_popupProbability;

    [Header("即破壊確率")]
    public int rare1_immediatelyBreakProbability;


    [Header("説明文 固有1"), Multiline(3)]
    public string rare2_explain;

    [Header("抽選確率")]
    public int rare2_popupProbability;

    [Header("時間")]
    public int rare2_Time;

    [Header("ボールスピードアップ倍率")]
    public int rare2_BulletSpeedUp;

    [Header("レジェンド")]

    [Header("説明文 固有1"), Multiline(3)]
    public string legend1_explain;

    [Header("抽選確率")]
    public int legend1_popupProbability;

    [Header("発射増加数")]
    public int legend1_NumberOfShots;

    [Header("説明文 固有2"), Multiline(3)]
    public string legend2_explain;

    [Header("抽選確率")]
    public int legend2_popupProbability;

    [Header("時間")]
    public int legend2_Time;

    [Header("弾のスピードアップ倍率")]
    public int legend2_BulletSpeedUp;

    [Header("説明文 固有3"), Multiline(3)]
    public string legend3_explain;

    [Header("抽選確率")]
    public int legend3_popupProbability;

    [Header("弾の反射増加数")]
    public int legend3_reflectionCount;
}

[System.Serializable]
public class UniqueSetting
{

    [Header("固有特性の出現設定")]
    [Header("設定の開始ウェーブと最後のウェーブ")]
    public int startWaveCount;
    public int endWaveCount;

    [Header("固有が発動する枠が有効になる確率")]
    public int FirstFlame;
    public int SecondFlame;
    public int ThirdFlame;
}

