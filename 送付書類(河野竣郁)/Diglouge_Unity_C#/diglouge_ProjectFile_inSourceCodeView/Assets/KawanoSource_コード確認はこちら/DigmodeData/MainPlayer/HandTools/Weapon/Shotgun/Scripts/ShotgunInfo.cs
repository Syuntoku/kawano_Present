using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Syuntoku.DigMode.Tool;

namespace Syuntoku.DigMode.Weapon
{
    /// <summary>
    /// ÉäÉ{ÉãÉoÅ[èÓïÒ
    /// </summary>
    public class ShotgunInfo : WeaponInfo
    {
        public ShotgunInfo(ShotgunScriptable shotgunScriptable)
        {
            _weaponUpgrade = new ShotgunUpgrade();
            _weaponUpgrade.SetToolIcon(shotgunScriptable.upgradeIcon);
            _weaponBaseStatus = shotgunScriptable.weaponStatus;
        }
    }
}
