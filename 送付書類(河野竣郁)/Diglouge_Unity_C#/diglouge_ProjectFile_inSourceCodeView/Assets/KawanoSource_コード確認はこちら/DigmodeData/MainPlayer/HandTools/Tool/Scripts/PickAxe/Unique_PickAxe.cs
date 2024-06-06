using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Syuntoku.DigMode.Tool.Unique;
using Syuntoku.Status;

namespace Syuntoku.DigMode.Tool.Unique
{

    /// <summary>
    /// ピッケルの固有効果
    /// </summary>
    public class Unique_PickAxe : UniqueCharacteristics
    {

        enum UNIQUE_ACTIVE_NORMAL
        {
            NORMAL1 = 0x01,
            NORMAL2 = 0x02,
            MAX = 2,
        }

        enum UNIQUE_ACTIVE_RARE
        {
            RARE1 = 0x01,
            RARE2 = 0x02,
            MAX = 2,
        }

        enum UNIQUE_ACTIVE_LEGEND
        {
            LEGEND1 = 0x01,
            LEGEND2 = 0x02,
            MAX = 2,
        }


        public Unique_PickAxe()
        {

        }

        public override void RollUniqueData()
        {
            base.RollUniqueData();

            if (GameUtility.CheckUnderParsent(_toolUniqueScriptable.pickAxe.legend2_popupProbability))
            {
                activeFlag_Legend |= (uint)UNIQUE_ACTIVE_LEGEND.LEGEND2;

                if (ActiveEmptyFrameCheck()) return;

            }
            if (GameUtility.CheckUnderParsent(_toolUniqueScriptable.pickAxe.legend1_popupProbability))
            {
                activeFlag_Legend |= (uint)UNIQUE_ACTIVE_LEGEND.LEGEND1;
                if (ActiveEmptyFrameCheck()) return;
            }

            if (GameUtility.CheckUnderParsent(_toolUniqueScriptable.pickAxe.rare2_popupProbability))
            {
                activeFlag_Rare |= (uint)UNIQUE_ACTIVE_RARE.RARE2;
                if (ActiveEmptyFrameCheck()) return;
            }

            if (GameUtility.CheckUnderParsent(_toolUniqueScriptable.pickAxe.rare1_popupProbability))
            {
                activeFlag_Rare |= (uint)UNIQUE_ACTIVE_RARE.RARE1;
                if (ActiveEmptyFrameCheck()) return;
            }

            if (GameUtility.CheckUnderParsent(_toolUniqueScriptable.pickAxe.normal2_popupProbability))
            {
                activeFlag_Normal |= (uint)UNIQUE_ACTIVE_NORMAL.NORMAL2;
                if (ActiveEmptyFrameCheck()) return;

            }

            if (GameUtility.CheckUnderParsent(_toolUniqueScriptable.pickAxe.normal1_popupProbability))
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
            if ((activeFlag_Rare & (int)UNIQUE_ACTIVE_RARE.RARE1) == (int)UNIQUE_ACTIVE_RARE.RARE1)
            {
                Rare1();
            }
            if ((activeFlag_Rare & (int)UNIQUE_ACTIVE_RARE.RARE2) == (int)UNIQUE_ACTIVE_RARE.RARE2)
            {
                Rare2();
            }
            if ((activeFlag_Legend & (int)UNIQUE_ACTIVE_LEGEND.LEGEND1) == (int)UNIQUE_ACTIVE_LEGEND.LEGEND1)
            {
                Legend1();
            }
            if ((activeFlag_Legend & (int)UNIQUE_ACTIVE_LEGEND.LEGEND2) == (int)UNIQUE_ACTIVE_LEGEND.LEGEND2)
            {
                Legend2();
            }
        }   

        public override void DisableSetiing(ToolInfo toolInfo)
        {
            if ((activeFlag_Normal & (int)UNIQUE_ACTIVE_NORMAL.NORMAL1) == (int)UNIQUE_ACTIVE_NORMAL.NORMAL1)
            {
                _baseStatus.digmodeStatus.AddDigDamagePlibility(-_toolUniqueScriptable.pickAxe.normal1_blockDamageMagnification);
            }

            if ((activeFlag_Rare & (int)UNIQUE_ACTIVE_RARE.RARE1) == (int)UNIQUE_ACTIVE_RARE.RARE1)
            {
                _baseStatus.digmodeStatus.AddImmediatelyBreakManification(-_toolUniqueScriptable.pickAxe.rare1_immediatelyBreakProbability);
            }

            base.DisableSetiing(toolInfo);
        }

        public override bool Active(StatusManage magnificationStatus, Rarelity rarelity, int num)
        {
            switch(rarelity)
            {
                case Rarelity.NORMAL:

                    if ((activeFlag_Normal & (int)UNIQUE_ACTIVE_NORMAL.NORMAL1) != 0)
                    {
                        Normal1();
                        return true;
                    }
                    if ((activeFlag_Rare & (int)UNIQUE_ACTIVE_RARE.RARE1) != 0)
                    {
                        Normal2();
                        return true;
                    }

                    break;
                case Rarelity.RARE:
                    if ((activeFlag_Rare & (int)UNIQUE_ACTIVE_RARE.RARE2) != 0)
                    {
                        Rare1();
                        return true;
                    }

                    if ((activeFlag_Legend & (int)UNIQUE_ACTIVE_LEGEND.LEGEND1) != 0)
                    {
                        Rare2();
                        return true;
                    }


                    break;
                case Rarelity.LEGEND:

                    if ((activeFlag_Legend & (int)UNIQUE_ACTIVE_LEGEND.LEGEND1) != 0)
                    {
                        Legend1();
                    return true;
                    }

                    if ((activeFlag_Legend & (int)UNIQUE_ACTIVE_LEGEND.LEGEND2) != 0)
                    {
                        Legend2();
                        return true;
                    }

                    break;
            }

            return false;
        }

        public void Normal1()
        {
            _baseStatus.digmodeStatus.AddDigDamagePlibility(_toolUniqueScriptable.pickAxe.normal1_blockDamageMagnification);
        }

        public void Normal2()
        {
            _baseStatus.digmodeStatus.publicStatus.AddPribiltyAction(Digmode.StatusKind.DigPowerMagnification, _toolUniqueScriptable.pickAxe.normal2_DamageUpMagnification, _toolUniqueScriptable.pickAxe.normal2_DamageUpProbability, false, true);
        }
        public void Rare1()
        {
            _baseStatus.digmodeStatus.AddImmediatelyBreakManification(_toolUniqueScriptable.pickAxe.rare1_immediatelyBreakProbability);
        }

        public void Rare2()
        {
            _baseStatus.digmodeStatus.AddTimerChangeAction(PublicStatus.ActiveTrigger.Break, Digmode.StatusKind.PlayerSpeedMagnification, _toolUniqueScriptable.pickAxe.rare2_PlayerSpeed,_toolUniqueScriptable.pickAxe.rare2_Time,true,true);
        }

        public void Legend1()
        {
            _baseStatus.digmodeStatus.AddTimerChangeAction(PublicStatus.ActiveTrigger.Break, Digmode.StatusKind.DigPowerMagnification, _toolUniqueScriptable.pickAxe.legend1_DanageUp, _toolUniqueScriptable.pickAxe.legend1_Time, true, true);

        }

        public void Legend2()
        {
            _baseStatus.digmodeStatus.AddTimerChangeAction(PublicStatus.ActiveTrigger.Break, Digmode.StatusKind.PlayerSpeedMagnification, _toolUniqueScriptable.pickAxe.legend2_PlayerSpeed, _toolUniqueScriptable.pickAxe.legend2_Time, true, true);


        }

        override public string ExplainSet()
        {
           string explain = "";
            
            if ((activeFlag_Normal & (int)UNIQUE_ACTIVE_NORMAL.NORMAL1) == (int)UNIQUE_ACTIVE_NORMAL.NORMAL1)
            {
                explain += "　ノーマル　";
                explain += _toolUniqueScriptable.pickAxe.normal1_explain;
                explain += "\n";
            }
            if ((activeFlag_Normal & (int)UNIQUE_ACTIVE_NORMAL.NORMAL2) == (int)UNIQUE_ACTIVE_NORMAL.NORMAL2)
            {
                explain += "　ノーマル　";
                explain += _toolUniqueScriptable.pickAxe.normal2_explain2;
                explain += "\n";

            }
            if ((activeFlag_Rare & (int)UNIQUE_ACTIVE_RARE.RARE1) == (int)UNIQUE_ACTIVE_RARE.RARE1)
            {
                explain += "　レア　";
                explain += _toolUniqueScriptable.pickAxe.rare1_explain;
                explain += "\n";

            }

            if ((activeFlag_Rare & (int)UNIQUE_ACTIVE_RARE.RARE2) == (int)UNIQUE_ACTIVE_RARE.RARE2)
            {
                explain += "　レア　";
                explain += _toolUniqueScriptable.pickAxe.rare2_Explain2;
                explain += "\n";

            }

            if ((activeFlag_Legend & (int)UNIQUE_ACTIVE_LEGEND.LEGEND1) == (int)UNIQUE_ACTIVE_LEGEND.LEGEND1)
            {
                explain += "　レジェンド　";
                explain += _toolUniqueScriptable.pickAxe.legend1_explain;
                explain += "\n";

            }

            if ((activeFlag_Legend & (int)UNIQUE_ACTIVE_LEGEND.LEGEND2) == (int)UNIQUE_ACTIVE_LEGEND.LEGEND2)
            {
                explain += "　レジェンド　";
                explain += _toolUniqueScriptable.pickAxe.legend2_Explain2;
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
