using Syuntoku.Status;

namespace Syuntoku.DigMode.Tool
{
    public class HammerUpgrade : DigToolUpgrade
    {
        public HammerUpgrade()
        {
            _statusDrawCount = 4;
        }

        float _outlineDamageUpMagnification;
        int _digRange;

        const int MAX_DAMAGEUP_LEVEL = 4;
        const int MAX_INTERVAL_LEVEL = 4;
        const int MAX_SPEEDUP_LEVEL = 4;

        string[] ToolUpgladeInfoText = {
            "採掘ダメージ,採掘ダメージ,採掘ダメージ,採掘ダメージ,採掘ダメージ" ,
            "ツールを掘る速度,ツールを掘る速度,ツールを掘る速度,ツールを掘る速度,ツールを掘る速度",
            "外周マスダメージ,採掘範囲拡大,外周マスダメージ,外周マスダメージ,外周マスダメージ",
        };

        float[] damageUp_amountOfChangeInLevel =
        {
            0.1f,
            0.1f,
            0.1f,
            0.3f,
            0.3f,
        };

        float[] interval_amountOfChangeInLevel =
        {
            0.05f,
            0.05f,
            0.1f,
            0.1f,
            0.2f,
        };

        float[] outline_amountOfChangeInLevel =
        {
            0.2f,
            0.0f,
            0.2f,
            0.2f,
            0.4f,
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

            if (_thirdUpglade_level == 1)
            {
                _digRange++;
                return;
            }

            _outlineDamageUpMagnification += outline_amountOfChangeInLevel[_thirdUpglade_level];
        }

        public override void PulusStatus(StatusManage statusManage, ToolInfo toolInfo)
        {
            statusManage.digmodeStatus.AddDigDamagePlibility(_damageUpMagnification);
            statusManage.digmodeStatus.ReductionToolInvervalMagnification(_toolInvervalMagnification);

            HammerInfo hammerInfo = (HammerInfo)toolInfo;

            hammerInfo._flameDamageMagnification += _outlineDamageUpMagnification;
            hammerInfo._breakRange += _digRange;
        }

        public override void DisableStatus(StatusManage baseStatus, ToolInfo toolinfo)
        {
            baseStatus.digmodeStatus.AddDigDamagePlibility(-_damageUpMagnification);
            baseStatus.digmodeStatus.ReductionToolInvervalMagnification(-_toolInvervalMagnification);

            HammerInfo hammerInfo = (HammerInfo)toolinfo;

            hammerInfo._flameDamageMagnification -= _outlineDamageUpMagnification;

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
            statusText += "外周マスダメージ　　" + (int)(_outlineDamageUpMagnification * PERCENTAGE) + "％\n";
            statusText += "採掘範囲拡大　　　　+" + _digRange + "マス\n";
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
            else if (step == 1)
            {
                work += "," + (int)(interval_amountOfChangeInLevel[level] * PERCENTAGE) + "％\n";
            }
            else
            {
                if (level == 1)
                {
                    work += ",1";
                }
                else
                {
                    work += "," + (int)(outline_amountOfChangeInLevel[level] * PERCENTAGE) + "％\n";
                }
            }

            return work;
        }
    }
}
