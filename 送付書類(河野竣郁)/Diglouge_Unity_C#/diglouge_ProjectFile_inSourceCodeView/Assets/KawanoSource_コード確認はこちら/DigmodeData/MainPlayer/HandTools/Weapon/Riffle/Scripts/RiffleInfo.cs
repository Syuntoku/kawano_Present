using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Syuntoku.DigMode.Tool;

namespace Syuntoku.DigMode.Weapon
{
    /// <summary>
    /// リボルバー情報
    /// </summary>
    public class　RiffleInfo : WeaponInfo
    {
        public RiffleInfo(RiffleScriptable riffleScriptable)
        {
            _weaponUpgrade = new RiffleUpgrade();
            _weaponUpgrade.SetToolIcon(riffleScriptable.upgradeIcon);
            _weaponBaseStatus = riffleScriptable.weaponStatus;
        }
    }
}
