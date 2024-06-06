using System.Collections;
using System.Collections.Generic;
using System.IO;
using Syuntoku.Status;
using UnityEngine;

namespace Syuntoku.DigMode.ParkData
{


    public class Park_103_Boredom : ParkBase
    {
        float damageUpMagnification;
        float damageDownMagnification;
        const int FIRST_ATTACK = 1;
        const int DAMAGE_DOWN_COUNT = 3;

        public override void Loading(StringReader reader)
        {
            base.Loading(reader);
            damageUpMagnification = float.Parse(LoadLine(reader));
            damageDownMagnification = float.Parse(LoadLine(reader));
        }

        public override void WeaponUpdate(StatusManage baseStatus, ParkConditionsManage parkConditionsManage)
        {
            if (parkConditionsManage.enemyDamageCount == FIRST_ATTACK)
            {
                baseStatus.battleModeStatus.AttackPowerMagnification += damageUpMagnification;
            }
            else if (parkConditionsManage.enemyDamageCount >= DAMAGE_DOWN_COUNT)
            {
                baseStatus.battleModeStatus.AttackPowerMagnification -= damageDownMagnification;
            }

        }

    }
}
