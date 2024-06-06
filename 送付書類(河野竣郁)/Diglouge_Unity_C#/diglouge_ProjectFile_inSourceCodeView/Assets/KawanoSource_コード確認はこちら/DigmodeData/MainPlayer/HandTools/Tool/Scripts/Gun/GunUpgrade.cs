using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Syuntoku.Status;

namespace Syuntoku.DigMode.Tool
{
    public class GunUpgrade : DigToolUpgrade
    {
        public GunUpgrade()
        {
            _statusDrawCount = 4;
        }

        float _ballSpeedMagnification;
        int _addBallCount;
        int _reflectCount;

        const int MAX_DAMAGEUP_LEVEL = 4;
        const int MAX_INTERVAL_LEVEL = 4;
        const int MAX_GUN_SPECIAL_LEVEL = 4;

        string[] ToolUpgladeInfoText = {
            "�̌@�_���[�W,�̌@�_���[�W,�̌@�_���[�W,�̌@�_���[�W,�̌@�_���[�W",
            "�e�̑��x,�e�̑��x,�e�̑��x,�e�̑��x,�e�̑��x",
            "���˃{�[����,�{�[�����ˉ�,���˃{�[����,�{�[�����ˉ�,���˃{�[����",
        };

        float[] damageUp_amountOfChangeInLevel =
        {
            0.1f,
            0.2f,
            0.1f,
            0.1f,
            0.1f,
        };

        float[] ballSppedUp_amountOfChangeInLevel =
        {
            0.15f,
            0.25f,
            0.25f,
            0.25f,
            0.25f,
        };

        int[] gunSpecialLevelUpCount =
        {
            1,
            1,
            1,
            2,
            2,
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
            _ballSpeedMagnification += ballSppedUp_amountOfChangeInLevel[_secondUpglade_level];
        }

        public override void ThirdUpGlade()
        {
            _thirdUpglade_level++;
            if (_thirdUpglade_level >= MAX_GUN_SPECIAL_LEVEL) _thirdUpglade_level = MAX_GUN_SPECIAL_LEVEL;


            switch (_thirdUpglade_level)
            {
                //�������x��
                case 0:
                    _addBallCount++;
                    break;
                case 1:
                    _reflectCount++;
                    break;
                case 2:
                    _addBallCount++;
                    break;
                case 3:
                    _reflectCount += 2;
                    break;
                case 4:
                    _addBallCount += 2;
                    break;
            }
        }

        public override void PulusStatus(StatusManage statusManage, ToolInfo toolInfo)
        {
            GunInfo gunInfo = (GunInfo)toolInfo;

            statusManage.digmodeStatus.AddDigDamagePlibility(_damageUpMagnification);
            gunInfo._shotSpeedMagnification += _ballSpeedMagnification;
            gunInfo._shotCount += _addBallCount;
            statusManage.digmodeStatus.ballStatus._reflectionCount += _reflectCount;
        }

        public override void DisableStatus(StatusManage baseStatus, ToolInfo toolinfo)
        {
            GunInfo gunInfo = (GunInfo)toolinfo;

            baseStatus.digmodeStatus.AddDigDamagePlibility(-_damageUpMagnification);
            gunInfo._shotSpeedMagnification -= _ballSpeedMagnification;
            gunInfo._shotCount -= _addBallCount;
            baseStatus.digmodeStatus.ballStatus._reflectionCount -= _reflectCount;
        }

        public override string[] GetToolUpgladeInfo()
        {
            return ToolUpgladeInfoText;
        }

        public override string GetNowStatusText()
        {
            string statusText = "";

            statusText += "�̌@�_���[�W�@�@�@�@" + (int)(_damageUpMagnification * PERCENTAGE) + "��\n";
            statusText += "�{�[���̃X�s�[�h�@�@" + (int)(_toolInvervalMagnification * PERCENTAGE) + "��\n";
            statusText += "�{�[�����ː��@�@�@�@" + _addBallCount + "��\n";
            statusText += "�{�[���̔��ˉ񐔁@�@" + _reflectCount + "��\n";
            return statusText;
        }

        public override string GetUpGladeAmount(int step, int level)
        {
            string work = "";

            string[] split = ToolUpgladeInfoText[step].Split(",");
            work = split[level];
            work += "�@�@";

            if (step == 0)
            {
                work += "," + (int)(damageUp_amountOfChangeInLevel[level] * PERCENTAGE) + "��\n";
            }
            if (step == 1)
            {
                work += "," + (int)(ballSppedUp_amountOfChangeInLevel[level] * PERCENTAGE) + "��\n";
            }
            if (step == 2)
            {
                if (level >= 3)
                {
                    work += ",2��";
                }
                else
                {
                    work += ",1��";
                }
            }


            return work;
        }
    }
}

