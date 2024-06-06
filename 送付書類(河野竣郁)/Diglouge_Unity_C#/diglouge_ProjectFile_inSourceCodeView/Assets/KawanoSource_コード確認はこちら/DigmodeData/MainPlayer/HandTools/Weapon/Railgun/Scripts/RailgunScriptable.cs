using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Syuntoku.DigMode.Tool.Scriptable;

namespace Syuntoku.DigMode.Weapon
{
    [CreateAssetMenu(fileName = "RailgunSetting‚“", menuName = "Scriptable/Weapon/Create RailgunSetting")]
    public class RailgunScriptable : ScriptableObject
    {
        public WeaponBaseStatus weaponStatus;
        public GameObject weaponPrf;

        public UpgradeIcon upgradeIcon;
    }
}
