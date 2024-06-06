using System.Collections;
using System.Collections.Generic;
using System.IO;
using Syuntoku.Status;
using UnityEngine;

namespace Syuntoku.DigMode.ParkData
{


    public class Park_112_GiantWhale : ParkBase
    {
        float damageUpMagnification;
        float oneHpSell;
        public override void Loading(StringReader reader)
        {
            base.Loading(reader);
            damageUpMagnification = float.Parse(LoadLine(reader));
            oneHpSell = float.Parse(LoadLine(reader));
        }

        public override void Start(StatusManage baseStatus, ParkConditionsManage parkConditionsManage)
        {
            baseStatus.battleModeStatus.AttackPowerMagnification = damageUpMagnification * (baseStatus.battleModeStatus.playerStatus.MaxHP / oneHpSell);
        }

    }
}
