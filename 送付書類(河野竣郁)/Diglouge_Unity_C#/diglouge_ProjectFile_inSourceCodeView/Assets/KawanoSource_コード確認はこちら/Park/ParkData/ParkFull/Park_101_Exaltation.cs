using System.Collections;
using System.Collections.Generic;
using System.IO;
using Syuntoku.Status;
using UnityEngine;

namespace Syuntoku.DigMode.ParkData
{


    public class Park_101_Exaltation : ParkBase
    {
        float damageUpAmount;
        float damageUphold;
        const float MAX_DAMAGEUP_PRIBILITY = 100.0f;
        public override void Loading(StringReader reader)
        {
            base.Loading(reader);
            damageUpAmount = float.Parse(LoadLine(reader));
        }

        public override void WeaponUpdate(StatusManage baseStatus, ParkConditionsManage parkConditionsManage)
        {
            damageUphold = damageUpAmount * parkConditionsManage.wave_enmyKill;

            if(damageUphold >= MAX_DAMAGEUP_PRIBILITY)
            {
                damageUphold = MAX_DAMAGEUP_PRIBILITY;
            }

            baseStatus.battleModeStatus.AttackPowerMagnification = damageUphold;
        }

    }
}
