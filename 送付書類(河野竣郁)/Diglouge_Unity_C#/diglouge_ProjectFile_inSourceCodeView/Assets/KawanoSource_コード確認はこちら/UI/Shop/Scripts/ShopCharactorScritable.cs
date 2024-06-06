using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Syuntoku.DigMode.Inventory.Juwelry;

[CreateAssetMenu(menuName = "Scriptable/Create MerchandiseScritable")]
public class ShopCharactorScritable : ScriptableObject
{
    public enum SHOP_ID
    {
        BUY_TOOL,
        BUY_WEPON,
        MAX_OTHER,
    }

    public enum OTHER_ID
    {
        BUY_PARKRANDOM,
        BUY_HEALBASE,
        BUY_RETURN_TO_BASE,
        BUY_GAMBLING,
        MAX_OTHER,
    }

    [Header("--------------------------------------------------------------------------------------")]
    public string wave;
    [Header("細かいウェーブカウントの変更はこちら")]
    public int[] waveCount;

    [Header("ツールコスト")]
    public JuwelryInventory ToolCost;
    public string[] explanation_ToolCost;
    [Header("武器コスト")]
    public JuwelryInventory WeponCost;
    public string[] explanation_WeponCost;

    [Header("パークランダムコスト")]
    public JuwelryInventory ParkRandomCost;
    public string explanation_ParkCost;
    [Header("拠点HP回復コスト")]
    public JuwelryInventory HealHpCost;
    public string explanation_HealCost;

    [Header("--------------------------------------------------------------------------------------")]
    public string Second_wave;
    [Header("細かいウェーブカウントの変更はこちら")]
    public int[] Second_waveCount;

    [Header("ツールコスト")]
    public JuwelryInventory Second_ToolCost;
    public string[] explanation_Second_ToolCost;
    [Header("武器コスト")]
    public JuwelryInventory Second_WeponCost;
    public string[] explanation_Second_WeponCost;
    [Header("パークランダムコスト")]
    public JuwelryInventory Second_ParkRandomCost;
    public string explanation_Second_ParkCost;
    [Header("拠点HP回復コスト")]
    public JuwelryInventory Second_HealHpCost;
    public string explanation_Second_Second_HealCost;
    [Header("拠点強制ワープ")]
    public JuwelryInventory Second_ReturnToBaseCost;
    public string explanation_Second_ReturnToBaseCost;
    [Header("ギャンブル")]
    public JuwelryInventory Second_Gambling;
    public string explanation_Second_GamblingCost;

    [Header("-------------------------------------------------------------------------------------")]
    public string Third_wave;
    [Header("細かいウェーブカウントの変更はこちら")]
    public int[] Third_waveCount;

    [Header("ツールコスト")]
    public JuwelryInventory Third_ToolCost;
    public string[] explanation_Third_ToolCost;
    [Header("武器コスト")]
    public JuwelryInventory Third_WeponCost;
    public string[] explanation_Third_WeponCost;
    [Header("パークランダムコスト")]
    public JuwelryInventory Third_ParkRandomCost;
    public string explanation_Third_ParkCost;
    [Header("拠点HP回復コスト")]
    public JuwelryInventory Third_HealHpCost;
    public string explanation_Third_HealHpCost;
    [Header("拠点強制ワープ")]
    public JuwelryInventory Third_ReturnToBase;
    public string explanation_Third_ReturnToBaseCost;
    [Header("ギャンブル")]
    public JuwelryInventory Third_Gambling;
    public string explanation_Third_GumblingCost;

    [Header("-------------------------------------------------------------------------------------")]

    [Header("性能を表示するテキスト")]
    [Header("パークランダム")]
    public string paformance_ParkRamdom;
    [Header("拠点HP回復")]
    public string paformance_HealBase;
    [Header("拠点強制ワープ")]
    public string paformance_ReturnToBaseCost;
    [Header("ギャンブル")]
    public string paformance_GumblingCost;

    [Header("-------------------------------------------------------------------------------------")]

    [Header("再抽選するときのコスト")]
    public JuwelryInventory RerollCost;
    [Header("再抽選コスト増加量")]
    public float RerollIncreaseAmount;
}
