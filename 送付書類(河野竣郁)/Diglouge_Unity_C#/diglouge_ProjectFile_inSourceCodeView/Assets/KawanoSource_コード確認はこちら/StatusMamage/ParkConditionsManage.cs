using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Syuntoku.DigMode.Tool;
using Syuntoku.DigMode.Player;
using UniRx;

/// <summary>
/// ������d�g�݂��g���Ƃ��̃N���X
/// </summary>
public class ParkConditionsManage : MonoBehaviour
{

    public delegate void SwingAction();
    public SwingAction swingAction;

    public delegate void GunReflection();
    public GunReflection gunReflection;

    public delegate void GunBlockBreak();
    public GunReflection gunBleak;

    public const string ATTACH_OBJECT_NAME = "InventoryManage";

    ParkConditionsManage()
    {
        GetParkToBreakCount = 0;
        MaxGetParkToBreakCount = 1;
        waveCount = 1;
    }

    [Header("========================")]
    [Header("�Q�[���S�̂ł̏��")]
    [Header("========================")]

    [Header("�E�F�[�u�J�E���g")]
    public uint waveCount;

    [Header("�Q�[�����̈ړ��ʁ@�G���[�̉\��")]
    public uint game_MoveDistance;

    [Header("���̃Q�[�����œ|�����G�̐�")]
    public uint all_enemyKill;

    [Header("========================")]
    [Header("�̌@�֘A�̏������܂�")]
    [Header("========================")]


    [Header("�u���b�N��G�����Ƃ��̏���")]

    [Header("�_���[�W��^�����u���b�N�̏ꏊ")]
    public Vector3 tachBlockPos;
    [Header("�u���b�N��j�󂵂��ꏊ")]
    public Vector3 BreakBlockPos;

    public bool bRipple;
    public float ripple_Length;
    public float ripple_damagePribility;

    [Header("��x�ɍ̌@���鐔")]
    public uint _oneSwingDamage;

    [Header("�G�����u���b�N�̕���")]
    public uint blockState_Direction_virtical;
    public uint blockState_Direction_horizontal;

    public enum BlockState_Direction
    {
        UP,
        DOWN,
        LEFT,
        RIGHT,
    }

    [Header("�u���b�N�̋���")]
    public int blockState_Distance;

    [Header("����ɂ���u���b�N��")]
    public int aroundBlockNum;

    [Header("�u���b�N�̔j�󑍐�")]
    public int BlockBreakCount;

    public uint GetParkToBreakCount;
    public uint MaxGetParkToBreakCount;

    [Header("�ʏ�f�[�^")]


    [Header("�E�F�[�u���̈ړ��ʁ@�G���[�̉\��")]
    public uint wave_moveDistance;

    [Header("������K�w")]
    public int hierarchy;

    public bool IsJump { get => _jumpRP.Value; set => _jumpRP.Value = value; }
    public IReadOnlyReactiveProperty<bool> JumpReactiveProperty => _jumpRP;
    [Header("�W�����v��")]
    [SerializeField] private BoolReactiveProperty _jumpRP = new BoolReactiveProperty();

    [Header("�W���G���̑��擾��")]
    public int all_juwelry1;
    public int all_juwelry2;
    public int all_juwelry3;
    public int all_juwelry4;
    public int all_juwelry5;
    public int all_juwelry6;
    public int all_juwelry7;

    [Header("========================")]
    [Header("�퓬�֘A�̏������܂�")]
    [Header("========================")]

    [Header("���̃E�F�[�u�Ń_���[�W���󂯂Ă��邩")]
    public bool bWaveDamage;

    [Header("�e������������")]
    public int shotCount;

    [Header("����̎g�p���� *�؂�ւ���������")]
    public float weaponUseTime;

    [Header("�A�˂������@�{�^���������Ă��鎞��")]
    public int rapidFireCount;

    [Header("�E�F�[�u���œ|�����G�̐�")]
    public int wave_enmyKill;

    [Header("�G�Ƀ_���[�W��^��������")]
    public int damageEnemyLength;

    [Header("�G���_���[�W��H����Ă����")]
    public int enemyDamageCount;

    #region Methot

    /// <summary>
    /// �j�󐔂̃J�E���g�𑝂₷
    /// </summary>
    public void AddBreakCounter()
    {
        BlockBreakCount++;
        GetParkToBreakCount++;
    }

    /// <summary>
    /// �c�[����U�����ۂ̃��\�b�h�����s
    /// </summary>
    public void Swing()
    {
        swingAction();
        _oneSwingDamage = 0;
    }

    /// <summary>
    /// �c�[���F�{�[�������˂������̃��\�b�h�����s
    /// </summary>
    public void GunUpDateReflectionBuff()
    {
        gunReflection();
    }

    /// <summary>
    /// �c�[���F�{�[���Ńu���b�N���󂵂����̃��\�b�h�����s
    /// </summary>
    public void GunUpdateBlockBreakBuff()
    {
        gunBleak();
    }

    /// <summary>
    /// �x�[�X�̂g�o��{���ŉ񕜁@osa����Ɏ������肢���܂�
    /// </summary>
    /// <param name="healPribility"></param>
    public void HealBaseHPPribility(float healPribility)
    {

    }
    /// <summary>
    /// �x�[�X�̂g�o�����̂܂܉񕜁@osa����Ɏ������肢���܂�
    /// </summary>
    /// <param name="healPribility"></param>
    public void HealBaseHP(float healPower)
    {

    }


    #endregion

    public void DigConditionChange(ToolInfo toolInfo, RaycastHit hit, FirstPerson firstPerson)
    {
        blockState_Distance = (int)hit.distance;
        switch (toolInfo._toolStatus.toolKind)
        {
            case ToolGenerater.ToolName.PICK_AXE:
                _oneSwingDamage = 1;
                break;
            case ToolGenerater.ToolName.HAMMER:
                _oneSwingDamage = 8;
                break;
        }
    }

    private void OnDestroy()
    {
        _jumpRP.Dispose();
    }
}



