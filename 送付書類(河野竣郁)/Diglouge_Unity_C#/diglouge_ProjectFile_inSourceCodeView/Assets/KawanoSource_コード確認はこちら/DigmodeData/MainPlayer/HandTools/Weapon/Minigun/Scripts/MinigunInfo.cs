using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Syuntoku.DigMode.Tool;

namespace Syuntoku.DigMode.Weapon
{
    /// <summary>
    /// ミニガン情報
    /// </summary>
    public class MinigunInfo : WeaponInfo
    {
        public MinigunInfo(MinigunScriptable minigunScriptable)
        {
            _weaponUpgrade = new MinigunUpgrade();
            _weaponUpgrade.SetToolIcon(minigunScriptable.upgradeIcon);
            _weaponBaseStatus = minigunScriptable.weaponStatus;
        }
    }
}
