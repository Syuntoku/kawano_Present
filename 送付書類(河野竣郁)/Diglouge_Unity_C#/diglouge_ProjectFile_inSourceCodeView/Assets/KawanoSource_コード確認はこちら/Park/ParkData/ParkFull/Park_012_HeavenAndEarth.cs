using System.Collections;
using System.Collections.Generic;
using System.IO;
using Syuntoku.Status;
using UnityEngine;

namespace Syuntoku.DigMode.ParkData
{


    public class Park_012_HeavenAndEarth : ParkBase
    {
        float groundSpeedUpMagnification;
        float FlySpeedUpMagnification;

        public override void Loading(StringReader reader)
        {
            base.Loading(reader);

            groundSpeedUpMagnification = float.Parse(LoadLine(reader));
            FlySpeedUpMagnification = float.Parse(LoadLine(reader));
        }

        public override void Start(StatusManage baseStatus, ParkConditionsManage parkConditionsManage)
        {
            baseStatus.digmodeStatus.AddGroundSpeedMagnification(groundSpeedUpMagnification);
            baseStatus.digmodeStatus.AddAirSpeedMagnification(FlySpeedUpMagnification);
        }

        public override void Disable(StatusManage baseStatus, ParkConditionsManage parkConditionsManage)
        {
            baseStatus.digmodeStatus.AddGroundSpeedMagnification(groundSpeedUpMagnification);
            baseStatus.digmodeStatus.AddAirSpeedMagnification(FlySpeedUpMagnification);
        }

    }
}
