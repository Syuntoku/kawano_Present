using System.Collections;
using System.Collections.Generic;
using System.IO;
using Syuntoku.Status;
using UnityEngine;

namespace Syuntoku.DigMode.ParkData
{


    public class Park_019_Brilliant : ParkBase
    {
        float _addBrilliantPower;

        public override void Loading(StringReader reader)
        {
            base.Loading(reader);

            _addBrilliantPower = float.Parse(LoadLine(reader));
        }

        public override void Start(StatusManage baseStatus, ParkConditionsManage parkConditionsManage)
        {
            baseStatus.digmodeStatus.AddSuitPowerMagnification(_addBrilliantPower);
        }

        public override void Disable(StatusManage baseStatus, ParkConditionsManage parkConditionsManage)
        {
            baseStatus.digmodeStatus.AddSuitPowerMagnification(-_addBrilliantPower);
        }
    }
}
