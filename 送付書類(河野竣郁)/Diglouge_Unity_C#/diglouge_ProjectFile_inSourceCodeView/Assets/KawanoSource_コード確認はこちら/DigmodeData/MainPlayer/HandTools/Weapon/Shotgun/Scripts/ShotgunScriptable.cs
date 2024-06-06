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
        [Tooltip("一発に出る弾数")]
        public int shellInBulletCount;
        [Tooltip("弾が広がるときの広さ")]
        public float shotBulletRange;    
        public GameObject weaponPrf;
        public UpgradeIcon upgradeIcon;
    }
}
