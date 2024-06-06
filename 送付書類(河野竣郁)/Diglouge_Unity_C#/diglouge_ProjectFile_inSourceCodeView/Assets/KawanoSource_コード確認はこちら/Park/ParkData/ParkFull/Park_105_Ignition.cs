using System.Collections;
using System.Collections.Generic;
using System.IO;
using Syuntoku.Status;
using UnityEngine;

namespace Syuntoku.DigMode.ParkData
{
    public class Park_105_Ignition : ParkBase
    {
        float explosionOccurrencePlibility;
        float explosionDamage;
        public override void Loading(StringReader reader)
        {
            base.Loading(reader);
            explosionOccurrencePlibility = float.Parse(LoadLine(reader));
            explosionDamage = float.Parse(LoadLine(reader));
        }

        public override void Start(StatusManage baseStatus, ParkConditionsManage parkConditionsManage)
        {
            baseStatus.battleModeStatus.bShotExplosionAttack = true;
            baseStatus.battleModeStatus.ShotExplosionDamage = explosionDamage * parkConditionsManage.waveCount;
            baseStatus.battleModeStatus.OccurrenceShotExplosionDamage = explosionOccurrencePlibility;
        }
    }
}
