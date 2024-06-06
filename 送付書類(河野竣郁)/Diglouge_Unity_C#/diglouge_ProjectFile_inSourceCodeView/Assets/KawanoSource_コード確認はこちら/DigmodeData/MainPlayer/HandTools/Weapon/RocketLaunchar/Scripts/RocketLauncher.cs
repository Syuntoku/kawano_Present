using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Syuntoku.DigMode.Tool;

namespace Syuntoku.DigMode.Weapon
{
    public class RocketLauncher : WeaponBase
    {
        RocketLaucherInfo _rocketLaucherInfo;

        public RocketLauncher()
        {
        }

        public override void SetStatus(WeaponInfo weaponInfo)
        {
            base.SetStatus(weaponInfo);
            _rocketLaucherInfo = (RocketLaucherInfo)weaponInfo;
        }

        public override void SetHoldTool()
        {
            base.SetHoldTool();
            _rocketLaucherInfo._weaponUpgrade.ChangeStatus(base._statusManage, _rocketLaucherInfo._weaponBaseStatus);
        }

        public override void PutItAway()
        {
            base.PutItAway();
            _rocketLaucherInfo._weaponUpgrade.DisableStatus(base._statusManage, _rocketLaucherInfo._weaponBaseStatus);
        }
    }
}
