using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Syuntoku.Status;

namespace Syuntoku.DigMode.Tool
{
    public class PickaxeUpgrade : DigToolUpgrade
    {
        public PickaxeUpgrade()
        {
            _statusDrawCount = 3;
        }

        float _speedUpMagnification;

        const int MAX_DAMAGEUP_LEVEL = 4;
        const int MAX_INTERVAL_LEVEL = 4;
        const int MAX_SPEEDUP_LEVEL = 4;

        float[] damageUp_amountOfChangeInLevel =
        {
            0.20f,
            0.20f,
            0.20f,
            0.20f,
            0.40f,
        };
        float[] interval_amountOfChangeInLevel =
        {
            0.10f,
            0.20f,
            0.10f,
            0.10f,
            0.20f,
        };

        float[] sppedUp_amountOfChangeInLevel =
        {
            0.050f,
            0.10f,
            0.10f,
            0.10f,
            0.150f,
        };

        string[] ToolUpgladeInfoText = {
            "採掘ダメージ,採掘ダメージ,採掘ダメージ,採掘ダメージ,採掘ダメージ" ,
            "ツールを掘る速度,ツールを掘る速度,ツールを掘る速度,ツールを掘る速度,ツールを掘る速度",
            "移動速度,移動速度,移動速度,移動速度,移動速度",
        };


        public override void FirstUpGlade()
        {
            _firstUpglade_level++;
            if (_firstUpglade_level >= MAX_DAMAGEUP_LEVEL) _firstUpglade_level = MAX_DAMAGEUP_LEVEL;
            _damageUpMagnification += damageUp_amountOfChangeInLevel[_firstUpglade_level];
        }

        public override void SecondUpGlade()
        {
            _secondUpglade_level++;
            if (_secondUpglade_level >= MAX_INTERVAL_LEVEL) _secondUpglade_level = MAX_INTERVAL_LEVEL;
            _toolInvervalMagnification += interval_amountOfChangeInLevel[_secondUpglade_level];
        }

        public override void ThirdUpGlade()
        {
            _thirdUpglade_level++;
            if (_thirdUpglade_level >= MAX_SPEEDUP_LEVEL) _thirdUpglade_level = MAX_SPEEDUP_LEVEL;
            _speedUpMagnification += sppedUp_amountOfChangeInLevel[_thirdUpglade_level];
        }


        public override void PulusStatus(StatusManage statusManage, ToolInfo toolInfo)
        {
            statusManage.digmodeStatus.AddDigDamagePlibility(_damageUpMagnification);
            statusManage.digmodeStatus.ReductionToolInvervalMagnification(_toolInvervalMagnification);
            statusManage.digmodeStatus.AddSpeedMagnification(_speedUpMagnification);
        }

        public override void DisableStatus(StatusManage baseStatus, ToolInfo toolinfo)
        {
            baseStatus.digmodeStatus.AddDigDamagePlibility(-_damageUpMagnification);
            baseStatus.digmodeStatus.ReductionToolInvervalMagnification(-_toolInvervalMagnification);
            baseStatus.digmodeStatus.AddSpeedMagnification(-_speedUpMagnification);
        }

        public override string[] GetToolUpgladeInfo()
        {
            return ToolUpgladeInfoText;
        }

        public override string GetNowStatusText()
        {
            string statusText = "";

            statusText += "採掘ダメージ　　　　" + (int)(_damageUpMagnification * PERCENTAGE) + "％\n";
            statusText += "ツールを振る速度　　" + (int)(_toolInvervalMagnification * PERCENTAGE) + "％\n";
            statusText += "移動速度　　　　　　" + (int)(_speedUpMagnification * PERCENTAGE) + "％\n";

            return statusText;
        }
        public override string GetUpGladeAmount(int step, int level)
        {
            string work = "";

            string[] split = ToolUpgladeInfoText[step].Split(",");
            work = split[level];

            work += "　　";

            if (step == 0)
            {
                work += "," + (int)(damageUp_amountOfChangeInLevel[level] * PERCENTAGE) + "％\n";
            }

            if (step == 1)
            {
                work += "," + (int)(interval_amountOfChangeInLevel[level] * PERCENTAGE) + "％\n";
            }

            if (step == 2)
            {
                work += "," + (int)(sppedUp_amountOfChangeInLevel[level] * PERCENTAGE) + "％\n";
            }

            return work;
        }
    }
}
