using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Syuntoku.DigMode.Tool.Scriptable;

namespace Syuntoku.DigMode.Weapon
{
    [CreateAssetMenu(fileName = "ShotgunSettings", menuName = "Scriptable/Weapon/Create ShotgunSetting")]
    public class ShotgunScriptable : ScriptableObject
    {
        public WeaponBaseStatus weaponStatus;
        [Tooltip("ˆê”­‚Éo‚é’e”")]
        public int shellInBulletCount;
        [Tooltip("’e‚ªL‚ª‚é‚Æ‚«‚ÌL‚³")]
        public float shotBulletRange;    
        public GameObject weaponPrf;
        public UpgradeIcon upgradeIcon;
    }
}
