using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Syuntoku.DigMode.Tool.Unique;
using Syuntoku.Status;
using Syuntoku.DigMode.Tool;

public class Unique_Gun : UniqueCharacteristics
{
    public enum UNIQUE_ACTIVE_NORMAL
    {
        NORMAL1 = 0x01,
        NORMAL2 = 0x02,
        MAX = 2,
    }

    public enum UNIQUE_ACTIVE_RARE
    {
        RARE1 = 0x01,
        RARE2 = 0x02,
        MAX = 2,
    }

    public enum UNIQUE_ACTIVE_LEGEND
    {
        LEGEND1 = 0x01,
        LEGEND2 = 0x02,
        LEGEND3 = 0x04,
        MAX = 2,
    }

    public Unique_Gun()
    {

    }

    public override void Initialize(UniqueCharacteristicsScriptable toolScriptable, ParkConditionsManage parkConditionsManage, StatusManage baseStatus)
    {
        base.Initialize(toolScriptable, parkConditionsManage, baseStatus);
    }

    public override void RollUniqueData()
    {
        base.RollUniqueData();

        

        if (GameUtility.CheckUnderParsent(_toolUniqueScriptable.gun.legend2_popupProbability))
        {
            activeFlag_Legend |= (uint)UNIQUE_ACTIVE_LEGEND.LEGEND2;
            if (ActiveEmptyFrameCheck()) return;

        }

        if (GameUtility.CheckUnderParsent(_toolUniqueScriptable.gun.legend1_popupProbability))
        {
            activeFlag_Legend |= (uint)UNIQUE_ACTIVE_LEGEND.LEGEND1;
            if (ActiveEmptyFrameCheck()) return;
        }

        if (GameUtility.CheckUnderParsent(_toolUniqueScriptable.gun.rare2_popupProbability))
        {
            activeFlag_Rare |= (uint)UNIQUE_ACTIVE_RARE.RARE2;
            if (ActiveEmptyFrameCheck()) return;
        }

        if (GameUtility.CheckUnderParsent(_toolUniqueScriptable.gun.rare1_popupProbability))
        {
            activeFlag_Rare |= (uint)UNIQUE_ACTIVE_RARE.RARE1;
            if (ActiveEmptyFrameCheck()) return;

        }

        if (GameUtility.CheckUnderParsent(_toolUniqueScriptable.gun.normal2_popupProbability))
        {
            activeFlag_Normal |= (uint)UNIQUE_ACTIVE_NORMAL.NORMAL2;
            if (ActiveEmptyFrameCheck()) return;

        }

        if (GameUtility.CheckUnderParsent(_toolUniqueScriptable.gun.noamal1_popupProbability))
        {
            activeFlag_Normal |= (uint)UNIQUE_ACTIVE_NORMAL.NORMAL1;
            if (ActiveEmptyFrameCheck()) return;
        }
    }

    public override void ActiveSetting(ToolInfo toolInfo)
    {
        if ((activeFlag_Normal & (int)UNIQUE_ACTIVE_NORMAL.NORMAL1) == (int)UNIQUE_ACTIVE_NORMAL.NORMAL1)
        {
            Normal1();
        }
        if ((activeFlag_Normal & (int)UNIQUE_ACTIVE_NORMAL.NORMAL2) == (int)UNIQUE_ACTIVE_NORMAL.NORMAL2)
        {
            Normal2();
        }
        if ((activeFlag_Rare & (int)UNIQUE_ACTIVE_RARE.RARE2) == (int)UNIQUE_ACTIVE_RARE.RARE2)
        {
            Rare1();
        }
        if ((activeFlag_Rare & (int)UNIQUE_ACTIVE_RARE.RARE2) == (int)UNIQUE_ACTIVE_RARE.RARE2)
        {
            Rare2();
        }
        if ((activeFlag_Legend & (int)UNIQUE_ACTIVE_LEGEND.LEGEND1) == (int)UNIQUE_ACTIVE_LEGEND.LEGEND1)
        {
            Legend1((GunInfo)toolInfo);
        }
        if ((activeFlag_Legend & (int)UNIQUE_ACTIVE_LEGEND.LEGEND2) == (int)UNIQUE_ACTIVE_LEGEND.LEGEND2)
        {
            Legend2();
        }
    }

    public override void DisableSetiing(ToolInfo toolInfo)
    {

    }

    public void Normal1()
    {
        _baseStatus.digmodeStatus.AddDigDamagePlibility(_toolUniqueScriptable.gun.noamal1_damageUpProbability);
    }

    public void Normal2()
    {
        _baseStatus.digmodeStatus.ballStatus._speedMagnification += _toolUniqueScriptable.gun.noama21_bulletSpeedup;
    }

    public void Rare1()
    {
        _baseStatus.digmodeStatus.AddImmediatelyBreakManification(_toolUniqueScriptable.gun.rare1_immediatelyBreakProbability);
    }

    /// <summary>
    /// 破壊時の加速
    /// </summary>
    public void Rare2()
    {
        _baseStatus.digmodeStatus.ballStatus._publicGunStatus.AddReflectionStatus(Syuntoku.Status.BallStatus.BallStatusKind.SpeedMagnification, _toolUniqueScriptable.gun.rare2_BulletSpeedUp, _toolUniqueScriptable.gun.rare2_Time);
    }

    public void Legend1(GunInfo gunInfo)
    {
        gunInfo.AddShotCount(_toolUniqueScriptable.gun.legend1_NumberOfShots);
    }

    /// <summary>
    /// 破壊時の加速
    /// </summary>
    public void Legend2()
    {
        _baseStatus.digmodeStatus.ballStatus._publicGunStatus.AddReflectionStatus(Syuntoku.Status.BallStatus.BallStatusKind.SpeedMagnification, _toolUniqueScriptable.gun.legend2_BulletSpeedUp, _toolUniqueScriptable.gun.legend2_Time);
    }

    public void Legend3()
    {
        _baseStatus.digmodeStatus.ballStatus._reflectionCount += _toolUniqueScriptable.gun.legend3_reflectionCount;
    }


    /// <summary>
    /// 固有効果の説明文を生成する
    /// </summary>
    /// <returns></returns>
    override public string ExplainSet()
    {
        string explain = "";

        if ((activeFlag_Normal & (int)UNIQUE_ACTIVE_NORMAL.NORMAL1) == (int)UNIQUE_ACTIVE_NORMAL.NORMAL1)
        {
            explain += "　ノーマル　";
            explain += _toolUniqueScriptable.gun.normal1_explain;
            explain += "\n";
        }
        if ((activeFlag_Normal & (int)UNIQUE_ACTIVE_NORMAL.NORMAL2) == (int)UNIQUE_ACTIVE_NORMAL.NORMAL2)
        {
            explain += "　ノーマル　";
            explain += _toolUniqueScriptable.gun.normal2_explain;
            explain += "\n";

        }
        if ((activeFlag_Rare & (int)UNIQUE_ACTIVE_RARE.RARE1) == (int)UNIQUE_ACTIVE_RARE.RARE1)
        {
            explain += "　レア　";
            explain += _toolUniqueScriptable.gun.rare1_explain;
            explain += "\n";

        }

        if ((activeFlag_Rare & (int)UNIQUE_ACTIVE_RARE.RARE2) == (int)UNIQUE_ACTIVE_RARE.RARE2)
        {
            explain += "　レア　";
            explain += _toolUniqueScriptable.gun.rare2_explain;
            explain += "\n";

        }

        if ((activeFlag_Legend & (int)UNIQUE_ACTIVE_LEGEND.LEGEND1) == (int)UNIQUE_ACTIVE_LEGEND.LEGEND1)
        {
            explain += "　レジェンド　";
            explain += _toolUniqueScriptable.gun.legend1_explain;
            explain += "\n";

        }

        if ((activeFlag_Legend & (int)UNIQUE_ACTIVE_LEGEND.LEGEND2) == (int)UNIQUE_ACTIVE_LEGEND.LEGEND2)
        {
            explain += "　レジェンド　";
            explain += _toolUniqueScriptable.gun.legend2_explain;
            explain += "\n";

        }
        if ((activeFlag_Legend & (int)UNIQUE_ACTIVE_LEGEND.LEGEND3) == (int)UNIQUE_ACTIVE_LEGEND.LEGEND3)
        {
            explain += "　レジェンド　";
            explain += _toolUniqueScriptable.gun.legend2_explain;
            explain += "\n";

        }

        if (explain == "")
        {
            explain = "\n　固有性能無し";
        }


        return explain;
    }
}
