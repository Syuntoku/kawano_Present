using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Syuntoku.DigMode.Tool;

namespace Syuntoku.DigMode.Weapon
{
    /// <summary>
    /// ƒŒ[ƒ‹ƒKƒ“î•ñ
    /// </summary>
    public class RailgunInfo : WeaponInfo
    {
        public RailgunInfo(RailgunScriptable railgunScriptable)
        {
            _weaponUpgrade = new RailgunUpgrade();
            _weaponUpgrade.SetToolIcon(railgunScriptable.upgradeIcon);
            _weaponBaseStatus =railgunScriptable.weaponStatus;
        }
    }
}
