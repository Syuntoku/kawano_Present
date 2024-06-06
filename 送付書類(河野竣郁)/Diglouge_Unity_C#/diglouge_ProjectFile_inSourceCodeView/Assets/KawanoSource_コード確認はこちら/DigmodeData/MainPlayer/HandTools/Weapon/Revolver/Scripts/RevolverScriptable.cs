using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Syuntoku.DigMode.Tool.Scriptable;

namespace Syuntoku.DigMode.Weapon
{
    [CreateAssetMenu(fileName = "RevolverSetting‚“",menuName = "Scriptable/Weapon/Create RebolverSetting")]
    public class RevolverScriptable : ScriptableObject
    {
        public WeaponBaseStatus weaponStatus;
        public GameObject weaponPrf;
        public UpgradeIcon upgradeIcon;
    }
}
