using System.Collections;
using System.Collections.Generic;
using System.IO;
using Syuntoku.Status;
using UnityEngine;

namespace Syuntoku.DigMode.ParkData
{


    public class Park_002_Good_Luck : ParkBase
    {
        int immediateDestruction;

        public override void Loading(StringReader reader)
        {
            base.Loading(reader);

            immediateDestruction = int.Parse(LoadLine(reader));
        }

        public override void Start(StatusManage baseStatus, ParkConditionsManage parkConditionsManage)
        {
            baseStatus.digmodeStatus.AddImmediatelyBreakManification(immediateDestruction);
        }

        public override void Disable(StatusManage baseStatus, ParkConditionsManage parkConditionsManage)
        {
            baseStatus.digmodeStatus.AddImmediatelyBreakManification(-immediateDestruction);
        }
    }
}
