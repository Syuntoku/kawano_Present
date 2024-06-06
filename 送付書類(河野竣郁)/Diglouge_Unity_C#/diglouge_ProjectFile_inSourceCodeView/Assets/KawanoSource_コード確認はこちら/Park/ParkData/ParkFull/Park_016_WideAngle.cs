using System.Collections;
using System.Collections.Generic;
using System.IO;
using Syuntoku.Status;
using UnityEngine;

namespace Syuntoku.DigMode.ParkData
{


    public class Park_016_WideAngle : ParkBase
    {
        float _wideAngle;
        public override void Loading(StringReader reader)
        {
            base.Loading(reader);

            _wideAngle = float.Parse(LoadLine(reader));
        }

        public override void Start(StatusManage baseStatus, ParkConditionsManage parkConditionsManage)
        {
        }

    }
}
