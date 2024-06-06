using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Syuntoku.DigMode.Tool;
using Syuntoku.DigMode.Player;
using UniRx;

/// <summary>
/// 条件や仕組みを使うときのクラス
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
    [Header("ゲーム全体での情報")]
    [Header("========================")]

    [Header("ウェーブカウント")]
    public uint waveCount;

    [Header("ゲーム中の移動量　エラーの可能性")]
    public uint game_MoveDistance;

    [Header("このゲーム内で倒した敵の数")]
    public uint all_enemyKill;

    [Header("========================")]
    [Header("採掘関連の情報を入れます")]
    [Header("========================")]


    [Header("ブロックを触ったときの条件")]

    [Header("ダメージを与えたブロックの場所")]
    public Vector3 tachBlockPos;
    [Header("ブロックを破壊した場所")]
    public Vector3 BreakBlockPos;

    public bool bRipple;
    public float ripple_Length;
    public float ripple_damagePribility;

    [Header("一度に採掘する数")]
    public uint _oneSwingDamage;

    [Header("触ったブロックの方向")]
    public uint blockState_Direction_virtical;
    public uint blockState_Direction_horizontal;

    public enum BlockState_Direction
    {
        UP,
        DOWN,
        LEFT,
        RIGHT,
    }

    [Header("ブロックの距離")]
    public int blockState_Distance;

    [Header("周りにあるブロック数")]
    public int aroundBlockNum;

    [Header("ブロックの破壊総数")]
    public int BlockBreakCount;

    public uint GetParkToBreakCount;
    public uint MaxGetParkToBreakCount;

    [Header("通常データ")]


    [Header("ウェーブ中の移動量　エラーの可能性")]
    public uint wave_moveDistance;

    [Header("今いる階層")]
    public int hierarchy;

    public bool IsJump { get => _jumpRP.Value; set => _jumpRP.Value = value; }
    public IReadOnlyReactiveProperty<bool> JumpReactiveProperty => _jumpRP;
    [Header("ジャンプ中")]
    [SerializeField] private BoolReactiveProperty _jumpRP = new BoolReactiveProperty();

    [Header("ジュエルの総取得数")]
    public int all_juwelry1;
    public int all_juwelry2;
    public int all_juwelry3;
    public int all_juwelry4;
    public int all_juwelry5;
    public int all_juwelry6;
    public int all_juwelry7;

    [Header("========================")]
    [Header("戦闘関連の情報を入れます")]
    [Header("========================")]

    [Header("このウェーブでダメージを受けているか")]
    public bool bWaveDamage;

    [Header("銃を撃った総数")]
    public int shotCount;

    [Header("武器の使用時間 *切り替え時初期化")]
    public float weaponUseTime;

    [Header("連射した数　ボタンを押している時間")]
    public int rapidFireCount;

    [Header("ウェーブ内で倒した敵の数")]
    public int wave_enmyKill;

    [Header("敵にダメージを与えた距離")]
    public int damageEnemyLength;

    [Header("敵がダメージを食らっている回数")]
    public int enemyDamageCount;

    #region Methot

    /// <summary>
    /// 破壊数のカウントを増やす
    /// </summary>
    public void AddBreakCounter()
    {
        BlockBreakCount++;
        GetParkToBreakCount++;
    }

    /// <summary>
    /// ツールを振った際のメソッドを実行
    /// </summary>
    public void Swing()
    {
        swingAction();
        _oneSwingDamage = 0;
    }

    /// <summary>
    /// ツール：ボールが反射した時のメソッドを実行
    /// </summary>
    public void GunUpDateReflectionBuff()
    {
        gunReflection();
    }

    /// <summary>
    /// ツール：ボールでブロックを壊した時のメソッドを実行
    /// </summary>
    public void GunUpdateBlockBreakBuff()
    {
        gunBleak();
    }

    /// <summary>
    /// ベースのＨＰを倍率で回復　osaさんに実装お願いします
    /// </summary>
    /// <param name="healPribility"></param>
    public void HealBaseHPPribility(float healPribility)
    {

    }
    /// <summary>
    /// ベースのＨＰをそのまま回復　osaさんに実装お願いします
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



