using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Syuntoku.DigMode.Tool.Scriptable;

namespace Syuntoku.DigMode.Weapon
{
    [CreateAssetMenu(fileName = "MinigunSettings", menuName = "Scriptable/Weapon/Create MinigunSetting")]
    public class MinigunScriptable : ScriptableObject
    {
        public WeaponBaseStatus weaponStatus;
        public GameObject weaponPrf;

        public UpgradeIcon upgradeIcon;
    }
}
