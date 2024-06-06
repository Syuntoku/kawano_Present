using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Syuntoku.DigMode.Tool;

namespace Syuntoku.DigMode.Weapon
{
    /// <summary>
    /// ƒŠƒ{ƒ‹ƒo[î•ñ
    /// </summary>
    public class RocketLaucherInfo : WeaponInfo
    {
        public RocketLaucherInfo(RocketLauncherScriptable rocketLauncherScriptable)
        {
            _weaponUpgrade = new RocketLauncherUpgrade();
            _weaponUpgrade.SetToolIcon(rocketLauncherScriptable.upgradeIcon);
            _weaponBaseStatus = rocketLauncherScriptable.weaponStatus;
        }
    }
}
