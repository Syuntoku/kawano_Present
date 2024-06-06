using System.Collections;
using System.Collections.Generic;
using System.IO;
using Syuntoku.Status;
using UnityEngine;

namespace Syuntoku.DigMode.ParkData
{


    public class Park_008_Mole : ParkBase
    {

        float time;
        float enableTime;
        float speedUpPribility;
        bool bActive;

        public override void Loading(StringReader reader)
        {
            base.Loading(reader);

            enableTime = float.Parse(LoadLine(reader));
            speedUpPribility = float.Parse(LoadLine(reader));
        }

        public override void SwingUpdate(StatusManage baseStatus, ParkConditionsManage parkConditionsManage)
        {
            if (bActive) return;
            baseStatus.digmodeStatus.AddSpeedMagnification(speedUpPribility);
            bActive = true;
        }

        public override void Update(StatusManage baseStatus, ParkConditionsManage parkConditionsManage, float addTimier)
        {
            if (!bActive) return;

            time += addTimier;

            if (time >= enableTime)
            {
                baseStatus.digmodeStatus.AddSpeedMagnification(-speedUpPribility);
                time = 0.0f;
                bActive = false;
            }
        }

        public override void Disable(StatusManage baseStatus, ParkConditionsManage parkConditionsManage)
        {
            baseStatus.digmodeStatus.AddSpeedMagnification(-speedUpPribility);
        }
    }
}
