using System.Collections;
using System.Collections.Generic;
using System.IO;
using Syuntoku.Status;
using UnityEngine;

namespace Syuntoku.DigMode.ParkData
{


    public class Park_108_Whirlwind : ParkBase
    {
        float explosionDamageUpMagnification;
        public override void Loading(StringReader reader)
        {
            base.Loading(reader);
            explosionDamageUpMagnification = float.Parse(LoadLine(reader));
        }

        public override void Start(StatusManage baseStatus, ParkConditionsManage parkConditionsManage)
        {
            baseStatus.battleModeStatus.ExplosionDamageMagnification = explosionDamageUpMagnification;
        }

    }
}
