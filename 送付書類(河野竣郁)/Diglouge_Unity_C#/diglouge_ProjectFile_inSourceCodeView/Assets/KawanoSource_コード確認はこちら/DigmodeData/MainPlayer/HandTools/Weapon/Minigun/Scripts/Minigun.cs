using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Syuntoku.DigMode.Tool;

namespace Syuntoku.DigMode.Weapon
{
    public class Minigun : WeaponBase
    {
        MinigunInfo _minigunInfo;

        public Minigun()
        {
        }

        public override void SetStatus(WeaponInfo weaponInfo)
        {
            base.SetStatus(weaponInfo);
            _minigunInfo = (MinigunInfo)weaponInfo;
        }

        public override void SetHoldTool()
        {
            base.SetHoldTool();
            _minigunInfo._weaponUpgrade.ChangeStatus(base._statusManage, _minigunInfo._weaponBaseStatus);
        }

        public override void PutItAway()
        {
            base.PutItAway();
            _minigunInfo._weaponUpgrade.DisableStatus(base._statusManage, _minigunInfo._weaponBaseStatus);
        }
    }
}
