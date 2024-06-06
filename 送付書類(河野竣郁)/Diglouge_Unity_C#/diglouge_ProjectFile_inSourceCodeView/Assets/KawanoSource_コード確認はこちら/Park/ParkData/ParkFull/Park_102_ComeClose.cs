using System.Collections;
using System.Collections.Generic;
using System.IO;
using Syuntoku.Status;
using UnityEngine;

namespace Syuntoku.DigMode.ParkData
{


    public class Park_102_ComeClose : ParkBase
    {
        float LengthAmount;
        float damageUpMagnification;

        public override void Loading(StringReader reader)
        {
            base.Loading(reader);

            LengthAmount = float.Parse(LoadLine(reader));
            damageUpMagnification = float.Parse(LoadLine(reader));
        }
        public override void WeaponUpdate(StatusManage baseStatus, ParkConditionsManage parkConditionsManage)
        {
            if (parkConditionsManage.damageEnemyLength <= LengthAmount)
            {
                baseStatus.battleModeStatus.AttackPowerMagnification += damageUpMagnification;
            }
        }
    }
}
