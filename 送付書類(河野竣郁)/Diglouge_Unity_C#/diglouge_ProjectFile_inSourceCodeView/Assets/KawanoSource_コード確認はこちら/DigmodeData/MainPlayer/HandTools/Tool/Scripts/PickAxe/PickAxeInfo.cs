using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Syuntoku.DigMode.Tool.Scriptable;
using Syuntoku.DigMode.Tool.Unique;
using Syuntoku.Status;

namespace Syuntoku.DigMode.Tool
{
    [System.Serializable]
    public class PickAxeInfo : ToolInfo
    {
        public void SetUp(PickAxeInfoScriptable pickAxeInfoScriptable)
        {
            _toolStatus = pickAxeInfoScriptable.tooldata;
            _toolStatus.damageManager.damage = pickAxeInfoScriptable.damageInfos[0].damage;
            _toolStatus.interval = pickAxeInfoScriptable.damageInfos[0].Interval;

            _toolUpgladeStatus = new PickaxeUpgrade();
            _toolUpgladeStatus.SetToolIcon(pickAxeInfoScriptable.upgradeIcon);
            _uniqueCharacteristics = new Unique_PickAxe();
        }

        public override void Initialize(UniqueCharacteristicsScriptable uniqueCharacteristicsScriptable,ParkConditionsManage parkConditionsManage,StatusManage baseStatus, GameObject playerObject)
        {
            _uniqueCharacteristics.Initialize(uniqueCharacteristicsScriptable, parkConditionsManage, baseStatus);
        }
    }
}
