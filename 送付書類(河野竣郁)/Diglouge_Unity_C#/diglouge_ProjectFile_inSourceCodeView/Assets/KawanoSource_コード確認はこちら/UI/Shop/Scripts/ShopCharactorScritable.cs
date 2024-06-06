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
    [Header("�ׂ����E�F�[�u�J�E���g�̕ύX�͂�����")]
    public int[] waveCount;

    [Header("�c�[���R�X�g")]
    public JuwelryInventory ToolCost;
    public string[] explanation_ToolCost;
    [Header("����R�X�g")]
    public JuwelryInventory WeponCost;
    public string[] explanation_WeponCost;

    [Header("�p�[�N�����_���R�X�g")]
    public JuwelryInventory ParkRandomCost;
    public string explanation_ParkCost;
    [Header("���_HP�񕜃R�X�g")]
    public JuwelryInventory HealHpCost;
    public string explanation_HealCost;

    [Header("--------------------------------------------------------------------------------------")]
    public string Second_wave;
    [Header("�ׂ����E�F�[�u�J�E���g�̕ύX�͂�����")]
    public int[] Second_waveCount;

    [Header("�c�[���R�X�g")]
    public JuwelryInventory Second_ToolCost;
    public string[] explanation_Second_ToolCost;
    [Header("����R�X�g")]
    public JuwelryInventory Second_WeponCost;
    public string[] explanation_Second_WeponCost;
    [Header("�p�[�N�����_���R�X�g")]
    public JuwelryInventory Second_ParkRandomCost;
    public string explanation_Second_ParkCost;
    [Header("���_HP�񕜃R�X�g")]
    public JuwelryInventory Second_HealHpCost;
    public string explanation_Second_Second_HealCost;
    [Header("���_�������[�v")]
    public JuwelryInventory Second_ReturnToBaseCost;
    public string explanation_Second_ReturnToBaseCost;
    [Header("�M�����u��")]
    public JuwelryInventory Second_Gambling;
    public string explanation_Second_GamblingCost;

    [Header("-------------------------------------------------------------------------------------")]
    public string Third_wave;
    [Header("�ׂ����E�F�[�u�J�E���g�̕ύX�͂�����")]
    public int[] Third_waveCount;

    [Header("�c�[���R�X�g")]
    public JuwelryInventory Third_ToolCost;
    public string[] explanation_Third_ToolCost;
    [Header("����R�X�g")]
    public JuwelryInventory Third_WeponCost;
    public string[] explanation_Third_WeponCost;
    [Header("�p�[�N�����_���R�X�g")]
    public JuwelryInventory Third_ParkRandomCost;
    public string explanation_Third_ParkCost;
    [Header("���_HP�񕜃R�X�g")]
    public JuwelryInventory Third_HealHpCost;
    public string explanation_Third_HealHpCost;
    [Header("���_�������[�v")]
    public JuwelryInventory Third_ReturnToBase;
    public string explanation_Third_ReturnToBaseCost;
    [Header("�M�����u��")]
    public JuwelryInventory Third_Gambling;
    public string explanation_Third_GumblingCost;

    [Header("-------------------------------------------------------------------------------------")]

    [Header("���\��\������e�L�X�g")]
    [Header("�p�[�N�����_��")]
    public string paformance_ParkRamdom;
    [Header("���_HP��")]
    public string paformance_HealBase;
    [Header("���_�������[�v")]
    public string paformance_ReturnToBaseCost;
    [Header("�M�����u��")]
    public string paformance_GumblingCost;

    [Header("-------------------------------------------------------------------------------------")]

    [Header("�Ē��I����Ƃ��̃R�X�g")]
    public JuwelryInventory RerollCost;
    [Header("�Ē��I�R�X�g������")]
    public float RerollIncreaseAmount;
}
