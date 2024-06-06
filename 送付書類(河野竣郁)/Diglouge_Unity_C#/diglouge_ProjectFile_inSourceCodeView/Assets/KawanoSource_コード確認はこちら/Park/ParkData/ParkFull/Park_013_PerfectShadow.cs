using System.Collections;
using System.Collections.Generic;
using System.IO;
using Syuntoku.Status;
using UnityEngine;

namespace Syuntoku.DigMode.ParkData
{


    public class Park_013_PerfectShadow : ParkBase
    {
        float SpeedUpMagnification;
        const float LOWEST_VALUE = 0.01f;
        const int HALF = 2;

        public override void Loading(StringReader reader)
        {
            base.Loading(reader);

            SpeedUpMagnification = float.Parse(LoadLine(reader));
        }

        public override void Start(StatusManage baseStatus, ParkConditionsManage parkConditionsManage)
        {
            baseStatus.digmodeStatus.AddSpeedMagnification(SpeedUpMagnification);

        }

        public override void Disable(StatusManage baseStatus, ParkConditionsManage parkConditionsManage)
        {
            baseStatus.digmodeStatus.AddSpeedMagnification(-SpeedUpMagnification);
        }

        public void HealAction(StatusManage baseStatus)
        {

            baseStatus.digmodeStatus.AddSpeedMagnification(-SpeedUpMagnification);

            SpeedUpMagnification /= HALF;

            if (SpeedUpMagnification <= LOWEST_VALUE)
            {
                SpeedUpMagnification = LOWEST_VALUE;
            }
            baseStatus.digmodeStatus.AddSpeedMagnification(-SpeedUpMagnification);

        }
    }
}
