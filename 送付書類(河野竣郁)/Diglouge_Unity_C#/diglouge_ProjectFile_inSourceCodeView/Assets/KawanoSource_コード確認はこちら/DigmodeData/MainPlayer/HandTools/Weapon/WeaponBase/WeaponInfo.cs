using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Syuntoku.DigMode.Weapon;

namespace Syuntoku.DigMode.Tool
{
    [System.Serializable]
    public class WeaponInfo
    {
        public WeaponBaseStatus _weaponBaseStatus;
        public WeaponUpgrade _weaponUpgrade;
        public bool _bEquipment;

        public WeaponInfo()
        {

        }
        public WeaponInfo(WeaponInfo weaponInfo)
        {
            _weaponBaseStatus = weaponInfo._weaponBaseStatus;
            _weaponUpgrade = weaponInfo._weaponUpgrade;
        }

        public virtual string GetToolInfoText()
        {
            return "";
        }
    }

}
