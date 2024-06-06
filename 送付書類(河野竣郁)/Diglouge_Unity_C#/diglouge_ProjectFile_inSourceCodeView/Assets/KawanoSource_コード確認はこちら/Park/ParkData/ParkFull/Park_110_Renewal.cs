using System.Collections;
using System.Collections.Generic;
using System.IO;
using Syuntoku.Status;
using UnityEngine;

namespace Syuntoku.DigMode.ParkData
{


    public class Park_110_Renewal : ParkBase
    {
        float damageUpTime;
        float damageUpmagnification;
        public override void Loading(StringReader reader)
        {
            base.Loading(reader);
            damageUpTime =float.Parse(LoadLine(reader));
            damageUpmagnification =float.Parse(LoadLine(reader));
        }

        public override void WeaponUpdate(StatusManage baseStatus, ParkConditionsManage parkConditionsManage)
        {
            if(parkConditionsManage.weaponUseTime <= damageUpTime)
            {
                baseStatus.battleModeStatus.AttackPowerMagnification += damageUpmagnification;
            }
        }

    }
}
