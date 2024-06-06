using System.Collections;
using System.Collections.Generic;
using System.IO;
using Syuntoku.Status;
using UnityEngine;

namespace Syuntoku.DigMode.ParkData
{


    public class Park_109_Hakhiro : ParkBase
    {
        int damageUpCount;
        float damageUpMagnification;
        public override void Loading(StringReader reader)
        {
            base.Loading(reader);
            damageUpCount = int.Parse(LoadLine(reader));
            damageUpMagnification = float.Parse(LoadLine(reader));
        }

        public override void WeaponUpdate(StatusManage baseStatus, ParkConditionsManage parkConditionsManage)
        {
            if(parkConditionsManage.shotCount % damageUpCount == 0)
            {
                baseStatus.battleModeStatus.AttackPowerMagnification = damageUpMagnification;
            }
        }

    }
}
