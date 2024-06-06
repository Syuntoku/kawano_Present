using System.Collections;
using System.Collections.Generic;
using System.IO;
using Syuntoku.Status;
using UnityEngine;

namespace Syuntoku.DigMode.ParkData
{


    public class Park_011_Impatience : ParkBase
    {
        float baseDamage;
        float speedUpPlibility;

        public override void Loading(StringReader reader)
        {
            base.Loading(reader);

            baseDamage = float.Parse(LoadLine(reader));
            speedUpPlibility = float.Parse(LoadLine(reader));
        }

        public override void Start(StatusManage baseStatus, ParkConditionsManage parkConditionsManage)
        {
            baseStatus.digmodeStatus.AddSpeedMagnification(speedUpPlibility);
        }

    }
}
