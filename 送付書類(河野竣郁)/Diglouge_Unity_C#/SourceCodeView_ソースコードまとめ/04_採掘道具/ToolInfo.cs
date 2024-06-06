using UnityEngine;
using Syuntoku.DigMode.Tool.Unique;
using Syuntoku.DigMode.Tool.Scriptable;
using Syuntoku.Status;

namespace Syuntoku.DigMode.Tool
{
    [System.Serializable]
    public class ToolInfo
    {
        public enum ToolState
        {
            NOT_POSSESSION,
            LEVEL1,
            LEVEL2,
            LEVEL3,
            LEVEL4,
            LEVEL5,
            LEVEL6,
            LEVEL7,
            LEVEL8,
            LEVEL9,
            LEVEL10,
        }

        public bool _isEquipmet;
        public bool _isHand;
        public ToolStatus _toolStatus;
        public int _toolState;
        /// <summary>
        /// この装備にある固有データ
        /// </summary>
        public UniqueCharacteristics _uniqueCharacteristics;
        /// <summary>
        /// 装備強化のクラス
        /// </summary>
        public DigToolUpgrade _toolUpgladeStatus;

        public ToolInfo()
        {
            _toolState = (int)ToolState.NOT_POSSESSION;
        }

        public virtual void Initialize(UniqueCharacteristicsScriptable uniqueCharacteristicsScriptable, ParkConditionsManage parkConditionsManage, StatusManage baseStatus, GameObject playerObject)
        {

        }

        public virtual void ToolUpglade()
        {
            
        }

        public void ActiveOrLevelUp()
        {
            _toolState++;
        }

        public void SetHandSetting(StatusManage statusManage)
        {
            _uniqueCharacteristics.ActiveSetting(this);
            _uniqueCharacteristics.maxEmptyCheck();
            _toolUpgladeStatus.PulusStatus(statusManage, this);
            _isHand = true;
        }

        public void OutHandSetting(StatusManage statusManage)
        {
            _uniqueCharacteristics.DisableSetiing(this);
            _toolUpgladeStatus.DisableStatus(statusManage, this);
            _isHand = false;
        }

        public void SetUniqueData(UniqueCharacteristics uniqueCharacteristics)
        {
            _uniqueCharacteristics = uniqueCharacteristics;
        }

        public virtual string GetToolInfoText()
        {
            return "";
        }

        /// <summary>
        /// 現在のレベルを取得
        /// </summary>
        /// <returns>０～ツールのMAXLevel　＊取得していない場合はint.MaxValue</returns>
        public int GetArrayIndexLevelData()
        {
            if (_toolState == (int)ToolState.NOT_POSSESSION) return int.MaxValue;

            return _toolState - 1;
        }
    }
}
