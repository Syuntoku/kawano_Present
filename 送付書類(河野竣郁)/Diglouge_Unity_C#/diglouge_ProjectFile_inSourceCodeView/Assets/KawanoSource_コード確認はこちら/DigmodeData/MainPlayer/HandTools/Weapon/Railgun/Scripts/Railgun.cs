using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Syuntoku.DigMode.Tool;

namespace Syuntoku.DigMode.Weapon
{
    public class Railgun : WeaponBase
    {
        RailgunInfo _railgunInfo;

        public Railgun()
        {
        }

        public override void SetStatus(WeaponInfo weaponInfo)
        {
            base.SetStatus(weaponInfo);
            _railgunInfo = (RailgunInfo)weaponInfo;
        }

        public override void SetHoldTool()
        {
            base.SetHoldTool();
            _weaponInfo._weaponUpgrade.ChangeStatus(base._statusManage,_weaponInfo._weaponBaseStatus);
        }

        public override void PutItAway()
        {
            base.PutItAway();
            _weaponInfo._weaponUpgrade.DisableStatus(base._statusManage, _weaponInfo._weaponBaseStatus);
        }
    }
}
