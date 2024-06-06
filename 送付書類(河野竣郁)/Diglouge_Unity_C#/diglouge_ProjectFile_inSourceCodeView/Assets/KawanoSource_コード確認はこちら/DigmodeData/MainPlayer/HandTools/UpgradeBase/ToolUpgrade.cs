using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Syuntoku.DigMode.Tool.Scriptable;

namespace Syuntoku.DigMode.Tool
{
    public class ToolUpgrade
    {
        protected const int DEFALULT_UPGLADE_COUNT = 3;
        protected const int PERCENTAGE = 100;

        public UpgradeIcon _upgradeIcon;

        public ToolUpgrade()
        {
            _firstUpglade_level = -1;
            _secondUpglade_level = -1;
            _thirdUpglade_level = -1;
        }

        public int _firstUpglade_level;
        public int _secondUpglade_level;
        public int _thirdUpglade_level;
        public int _statusDrawCount;

        public void SetToolIcon(UpgradeIcon icon)
        {
            _upgradeIcon = icon;
        }

        virtual public void FirstUpGlade()
        {
            _firstUpglade_level++;
        }

        virtual public void SecondUpGlade()
        {
            _secondUpglade_level++;
        }

        virtual public void ThirdUpGlade()
        {
            _thirdUpglade_level++;
        }

        virtual public void ChageStatus()
        {

        }

        /// <summary>
        /// �c�[���̋����ł���e�L�X�g���擾
        /// </summary>
        virtual public string[] GetToolUpgladeInfo()
        {
            return null;
        }

        /// <summary>
        /// ���݂̃X�e�[�^�X���e�L�X�g�Ŏ擾
        /// </summary>
        virtual public string GetNowStatusText()
        {
            return "";
        }

        /// <summary>
        /// �ꃌ�x������������̕ω��ʂ��擾
        /// </summary>
        /// <param name="step">�擾���ݒ�@0:firstUpglade 1:secondUpglade 2:thirdUpglade</param>
        /// <param name="level"></param>
        virtual public string GetUpGladeAmount(int step, int level)
        {
            return "�ω���,������";
        }
    }
}
