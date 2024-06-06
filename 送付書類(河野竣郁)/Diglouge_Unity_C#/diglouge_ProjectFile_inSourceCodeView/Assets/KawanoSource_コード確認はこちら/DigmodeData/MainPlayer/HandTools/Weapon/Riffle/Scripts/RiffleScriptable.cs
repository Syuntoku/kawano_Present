using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Syuntoku.DigMode.Tool.Scriptable;

namespace Syuntoku.DigMode.Weapon
{
    [CreateAssetMenu(fileName = "RiffleSettings", menuName = "Scriptable/Weapon/Create RiffleSetting")]
    public class RiffleScriptable : ScriptableObject
    {
        public WeaponBaseStatus weaponStatus;
        public GameObject weaponPrf;
        public UpgradeIcon upgradeIcon;
    }
}
