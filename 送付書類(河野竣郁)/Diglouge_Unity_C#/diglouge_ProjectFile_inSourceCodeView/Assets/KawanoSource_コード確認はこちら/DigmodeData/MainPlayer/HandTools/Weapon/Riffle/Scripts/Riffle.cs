using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Syuntoku.DigMode.Tool;
using Syuntoku.Status;

namespace Syuntoku.DigMode.Weapon
{
    public class Riffle : WeaponBase
    {
        RiffleInfo _riffleInfo;

        public Riffle()
        {
        }

        public override void SetStatus(WeaponInfo weaponInfo)
        {
            base.SetStatus(weaponInfo);
            _riffleInfo = (RiffleInfo)weaponInfo;
        }

        public override void SetHoldTool()
        {
            base.SetHoldTool();
            _riffleInfo._weaponUpgrade.ChangeStatus(base._statusManage, _riffleInfo._weaponBaseStatus);
        }

        public override void PutItAway()
        {
            base.PutItAway();
            _riffleInfo._weaponUpgrade.DisableStatus(base._statusManage, _riffleInfo._weaponBaseStatus);
        }
    }
}
