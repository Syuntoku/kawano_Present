using System.Collections;
using System.Collections.Generic;
using System.IO;
using Syuntoku.Status;
using UnityEngine;

namespace Syuntoku.DigMode.ParkData
{
    public class Park_104_BigPicture : ParkBase
    {
        float damageDownMagnification;
        float waveDamageUpMagnification;
        public override void Loading(StringReader reader)
        {
            base.Loading(reader);
            damageDownMagnification = float.Parse(LoadLine(reader));
            waveDamageUpMagnification = float.Parse(LoadLine(reader));
        }

        public override void Start(StatusManage baseStatus, ParkConditionsManage parkConditionsManage)
        {
            baseStatus.battleModeStatus.AttackPowerMagnification -= damageDownMagnification;
        }

        public override void WeaponUpdate(StatusManage baseStatus, ParkConditionsManage parkConditionsManage)
        {
            baseStatus.battleModeStatus.AttackPowerMagnification += waveDamageUpMagnification * parkConditionsManage.waveCount;
        }

    }
}
