using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Syuntoku.DigMode.Tool.Scriptable;

namespace Syuntoku.DigMode.Weapon
{
    [CreateAssetMenu(fileName = "RocketLauncherSettings", menuName = "Scriptable/Weapon/Create RocketLauncherSetting")]
    public class RocketLauncherScriptable : ScriptableObject
    {
        public WeaponBaseStatus weaponStatus;
        public GameObject weaponPrf;

        public UpgradeIcon upgradeIcon;
    }
}
