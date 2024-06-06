using System.Collections;
using System.Collections.Generic;
using System.IO;
using Syuntoku.Status;
using UnityEngine;

namespace Syuntoku.DigMode.ParkData
{


    public class Park_010_RippleEffect : ParkBase
    {
        float damagePribility;
        float ripple_range;

        public override void Loading(StringReader reader)
        {
            base.Loading(reader);

            damagePribility = float.Parse(LoadLine(reader));
            ripple_range = float.Parse(LoadLine(reader));
        }

        public override void Start(StatusManage baseStatus, ParkConditionsManage parkConditionsManage)
        {
            
            parkConditionsManage.bRipple = true;
            parkConditionsManage.ripple_Length += ripple_range;
            parkConditionsManage.ripple_damagePribility += damagePribility;
        }

    }
}
