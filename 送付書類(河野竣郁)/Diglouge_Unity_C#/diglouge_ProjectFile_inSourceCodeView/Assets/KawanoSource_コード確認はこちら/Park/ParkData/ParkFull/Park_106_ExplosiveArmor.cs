using System.Collections;
using System.Collections.Generic;
using System.IO;
using Syuntoku.Status;
using UnityEngine;

namespace Syuntoku.DigMode.ParkData
{


    public class Park_106_ExplosiveArmor : ParkBase
    {
        float occurrenceExplosionPlibility;
        float ExplosionDamage;
        public override void Loading(StringReader reader)
        {
            base.Loading(reader);
            occurrenceExplosionPlibility = float.Parse(LoadLine(reader));
            ExplosionDamage = float.Parse(LoadLine(reader));
        }

        public override void Start(StatusManage baseStatus, ParkConditionsManage parkConditionsManage)
        {
            baseStatus.battleModeStatus.bBaseDamageExplosion = true;
            baseStatus.battleModeStatus.OccurrenceBaseExplosionDamage = occurrenceExplosionPlibility;
            baseStatus.battleModeStatus.BaseExplosionDamage = ExplosionDamage;
        }

    }
}
