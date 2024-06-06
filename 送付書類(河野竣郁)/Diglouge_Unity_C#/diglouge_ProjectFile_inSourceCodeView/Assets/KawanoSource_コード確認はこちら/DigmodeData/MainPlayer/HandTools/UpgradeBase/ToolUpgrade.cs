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
        /// ツールの強化できるテキストを取得
        /// </summary>
        virtual public string[] GetToolUpgladeInfo()
        {
            return null;
        }

        /// <summary>
        /// 現在のステータスをテキストで取得
        /// </summary>
        virtual public string GetNowStatusText()
        {
            return "";
        }

        /// <summary>
        /// 一レベル強化当たりの変化量を取得
        /// </summary>
        /// <param name="step">取得先を設定　0:firstUpglade 1:secondUpglade 2:thirdUpglade</param>
        /// <param name="level"></param>
        virtual public string GetUpGladeAmount(int step, int level)
        {
            return "変化量,未入力";
        }
    }
}
