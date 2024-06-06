using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Syuntoku.DigMode.Tool;

namespace Syuntoku.DigMode.Weapon
{
    /// <summary>
    /// リボルバー情報
    /// </summary>
    public class RevolverInfo : WeaponInfo
    {
        public RevolverInfo(RevolverScriptable revolverScriptable)
        {
            _weaponUpgrade = new RailgunUpgrade();
            _weaponUpgrade.SetToolIcon(revolverScriptable.upgradeIcon);
            _weaponBaseStatus = revolverScriptable.weaponStatus;
        }
    }
}
