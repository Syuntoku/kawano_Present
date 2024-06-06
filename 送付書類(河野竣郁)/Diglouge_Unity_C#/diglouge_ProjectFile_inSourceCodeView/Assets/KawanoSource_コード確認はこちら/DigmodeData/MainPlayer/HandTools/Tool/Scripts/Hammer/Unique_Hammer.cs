using System.Collections;
using System.Collections.Generic;
using Syuntoku.Status;
using UnityEngine;

namespace Syuntoku.DigMode.Tool.Unique
{

    public class Unique_Hammer : UniqueCharacteristics
    {
        enum UNIQUE_NOAMAL
        {
            NORMAL1 = 0x01,
            NORMAL2 = 0x02,
        }
        enum UNIQUE_RARE
        {
            RARE1 = 0x01,
            RARE2 = 0x02,
        }
        enum UNIQUE_LEGEND
        {
            LEGEND1 = 0x01,
            LEGEND2 = 0x02,
        }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="toolScriptable"></param>
        /// <param name="parkConditionsManage"></param>
        /// <param name="baseStatus"></param>
        public override void Initialize(UniqueCharacteristicsScriptable toolScriptable, ParkConditionsManage parkConditionsManage, StatusManage baseStatus)
        {
            base.Initialize(toolScriptable, parkConditionsManage, baseStatus);
        }

        /// <summary>
        /// ツールを持った時の初期設定
        /// </summary>
        public override void ActiveSetting(ToolInfo toolInfo)
        {
            if ((activeFlag_Normal & (int)UNIQUE_NOAMAL.NORMAL1) == (int)UNIQUE_NOAMAL.NORMAL1)
            {
                Normal1();
            }
            if ((activeFlag_Normal & (int)UNIQUE_NOAMAL.NORMAL1) == (int)UNIQUE_NOAMAL.NORMAL1)
            {
                Normal2();
            }
            if ((activeFlag_Rare & (int)UNIQUE_RARE.RARE1) == (int)UNIQUE_RARE.RARE1)
            {
                Rare1();
            }
            if ((activeFlag_Rare & (int)UNIQUE_RARE.RARE2) == (int)UNIQUE_RARE.RARE2)
            {
                Rare2();
            }
            if ((activeFlag_Legend & (int)UNIQUE_LEGEND.LEGEND1) == (int)UNIQUE_LEGEND.LEGEND1)
            {
                Legend1((HammerInfo)toolInfo);
            }
            if ((activeFlag_Legend & (int)UNIQUE_LEGEND.LEGEND2) == (int)UNIQUE_LEGEND.LEGEND2)
            {
                Legend2();
            }
        }

        /// <summary>
        /// ツールを外した時の設定
        /// </summary>
        public override void DisableSetiing(ToolInfo toolInfo)
        {
            if ((activeFlag_Normal & (int)UNIQUE_NOAMAL.NORMAL1) == (int)UNIQUE_NOAMAL.NORMAL1)
            {
                _baseStatus.digmodeStatus.AddDigDamagePlibility(-_toolUniqueScriptable.hammer.noamal1_DamageUpProbability);

            }
            if ((activeFlag_Rare & (int)UNIQUE_RARE.RARE1) == (int)UNIQUE_RARE.RARE1)
            {
                _baseStatus.digmodeStatus.AddImmediatelyBreakManification(-_toolUniqueScriptable.hammer.rare1_BreakProbability);

            }
            if ((activeFlag_Rare & (int)UNIQUE_RARE.RARE2) == (int)UNIQUE_RARE.RARE2)
            {
                _baseStatus.digmodeStatus.AddSpeedMagnification(-_toolUniqueScriptable.hammer.rare2_SpeedUpMagnifacation);

            }
            if ((activeFlag_Legend & (int)UNIQUE_LEGEND.LEGEND1) == (int)UNIQUE_LEGEND.LEGEND1)
            {
                Legend1((HammerInfo)toolInfo);
            }

            base.DisableSetiing(toolInfo);
        }


        public override void RollUniqueData()
        {
            base.RollUniqueData();

            if (GameUtility.CheckUnderParsent(_toolUniqueScriptable.hammer.legend2_popupProbability))
            {
                activeFlag_Legend |= (int)UNIQUE_LEGEND.LEGEND2;
                if (ActiveEmptyFrameCheck()) return;
            }

            if (GameUtility.CheckUnderParsent(_toolUniqueScriptable.hammer.legend1_popupProbability))
            {
                activeFlag_Legend |= (int)UNIQUE_LEGEND.LEGEND1;
                if (ActiveEmptyFrameCheck()) return;
            }

            if (GameUtility.CheckUnderParsent(_toolUniqueScriptable.hammer.rare2_popupProbability))
            {
                activeFlag_Rare |= (int)UNIQUE_RARE.RARE2;
                if (ActiveEmptyFrameCheck()) return;
            }

            if (GameUtility.CheckUnderParsent(_toolUniqueScriptable.hammer.rare1_popupProbability))
            {
                activeFlag_Rare |= (int)UNIQUE_RARE.RARE1;
                if (ActiveEmptyFrameCheck()) return;
            }

            if (GameUtility.CheckUnderParsent(_toolUniqueScriptable.hammer.noamal2_popupProbability))
            {
                activeFlag_Normal |= (int)UNIQUE_NOAMAL.NORMAL2;
                if (ActiveEmptyFrameCheck()) return;
            }

            if (GameUtility.CheckUnderParsent(_toolUniqueScriptable.hammer.noamal1_popupProbability))
            {
                activeFlag_Normal |= (int)UNIQUE_NOAMAL.NORMAL1;
                if (ActiveEmptyFrameCheck()) return;
            }
        }

        void Normal1()
        {
            _baseStatus.digmodeStatus.AddDigDamagePlibility(_toolUniqueScriptable.hammer.noamal1_DamageUpProbability);
        }

        void Normal2()
        {
            _baseStatus.digmodeStatus.publicStatus.AddPribiltyAction(Digmode.StatusKind.DigPowerMagnification, _toolUniqueScriptable.hammer.noamal2_DamageUpMagnifacation, _toolUniqueScriptable.hammer.noamal2_DamageUpProbability, false, true);
        }

        void Rare1()
        {
            _baseStatus.digmodeStatus.AddImmediatelyBreakManification(_toolUniqueScriptable.hammer.rare1_BreakProbability);
        }
        void Rare2()
        {
            _baseStatus.digmodeStatus.AddSpeedMagnification(_toolUniqueScriptable.hammer.rare2_SpeedUpMagnifacation);
        }

        void Legend1(HammerInfo hammerInfo)
        {
            hammerInfo.AddBreakRange(1);
        }

        void Legend2()
        {
            _baseStatus.digmodeStatus.AddTimerChangeAction(PublicStatus.ActiveTrigger.Break, Digmode.StatusKind.DigCooltime, _toolUniqueScriptable.hammer.legend2_coolTime, _toolUniqueScriptable.hammer.legend2_Time, true, true);
        }


        override public string ExplainSet()
        {
           string explain = "";

            if ((activeFlag_Normal & (int)UNIQUE_NOAMAL.NORMAL1) == (int)UNIQUE_NOAMAL.NORMAL1)
            {
                explain += "　ノーマル　";
                explain += _toolUniqueScriptable.hammer.normal1_explain;
                explain += "\n";
            }
            if ((activeFlag_Normal & (int)UNIQUE_NOAMAL.NORMAL2) == (int)UNIQUE_NOAMAL.NORMAL2)
            {
                explain += "　ノーマル　";
                explain += _toolUniqueScriptable.hammer.normal2_explain;
                explain += "\n";

            }
            if ((activeFlag_Rare & (int)UNIQUE_RARE.RARE1) == (int)UNIQUE_RARE.RARE1)
            {
                explain += "　レア　";
                explain += _toolUniqueScriptable.hammer.rare1_explain;
                explain += "\n";

            }

            if ((activeFlag_Rare & (int)UNIQUE_RARE.RARE2) == (int)UNIQUE_RARE.RARE2)
            {
                explain += "　レア　";
                explain += _toolUniqueScriptable.hammer.rare2_explain;
                explain += "\n";

            }

            if ((activeFlag_Legend & (int)UNIQUE_LEGEND.LEGEND1) == (int)UNIQUE_LEGEND.LEGEND1)
            {
                explain += "　レジェンド　";
                explain += _toolUniqueScriptable.hammer.legend1_explain;
                explain += "\n";

            }

            if ((activeFlag_Legend & (int)UNIQUE_LEGEND.LEGEND2) == (int)UNIQUE_LEGEND.LEGEND2)
            {
                explain += "　レジェンド　";
                explain += _toolUniqueScriptable.hammer.legend2_explain;
                explain += "\n";

            }

            if (explain == "")
            {
                explain = "\n　固有性能無し";
            }

            return explain;
        }
    }
}
