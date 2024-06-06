using System.Collections;
using System.Collections.Generic;
using System.IO;
using Syuntoku.Status;
using UnityEngine;

namespace Syuntoku.DigMode.ParkData
{


    public class Park_014_Tide : ParkBase
    {
        float healPowerUpMagnification;
        public override void Loading(StringReader reader)
        {
            base.Loading(reader);

            healPowerUpMagnification = float.Parse(LoadLine(reader));
        }

        public override void Start(StatusManage baseStatus, ParkConditionsManage parkConditionsManage)
        {
            baseStatus.battleModeStatus.HealPowerMagnification += healPowerUpMagnification;
        }
        public override void Disable(StatusManage baseStatus, ParkConditionsManage parkConditionsManage)
        {
            baseStatus.battleModeStatus.HealPowerMagnification -= healPowerUpMagnification;
        }

    }
}
