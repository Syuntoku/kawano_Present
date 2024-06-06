using System.Collections;
using System.Collections.Generic;
using System.IO;
using Syuntoku.Status;
using UnityEngine;

namespace Syuntoku.DigMode.ParkData
{


    public class Park_107_WideAreaStrategy : ParkBase
    {
        float explosionRange;
        public override void Loading(StringReader reader)
        {
            base.Loading(reader);
            explosionRange = float.Parse(LoadLine(reader));
        }

        public override void Start(StatusManage baseStatus, ParkConditionsManage parkConditionsManage)
        {
            baseStatus.battleModeStatus.ExplosionRangeMagnification = explosionRange;
        }

    }
}
