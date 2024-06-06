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
        [Tooltip("�ꔭ�ɏo��e��")]
        public int shellInBulletCount;
        [Tooltip("�e���L����Ƃ��̍L��")]
        public float shotBulletRange;    
        public GameObject weaponPrf;
        public UpgradeIcon upgradeIcon;
    }
}
