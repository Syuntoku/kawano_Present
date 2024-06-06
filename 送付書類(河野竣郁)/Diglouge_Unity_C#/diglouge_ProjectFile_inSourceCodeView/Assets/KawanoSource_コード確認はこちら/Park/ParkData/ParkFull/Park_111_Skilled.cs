using System.Collections;
using System.Collections.Generic;
using System.IO;
using Syuntoku.Status;
using UnityEngine;

namespace Syuntoku.DigMode.ParkData
{


    public class Park_111_Skilled : ParkBase
    {

        float reroadShorteningMagnification;

        public override void Loading(StringReader reader)
        {
            base.Loading(reader);

            reroadShorteningMagnification = float.Parse(LoadLine(reader));
        }

        public override void Start(StatusManage baseStatus, ParkConditionsManage parkConditionsManage)
        {
            baseStatus.battleModeStatus.AddReloadSpeedMagnification(-reroadShorteningMagnification);
        }
    }
}
