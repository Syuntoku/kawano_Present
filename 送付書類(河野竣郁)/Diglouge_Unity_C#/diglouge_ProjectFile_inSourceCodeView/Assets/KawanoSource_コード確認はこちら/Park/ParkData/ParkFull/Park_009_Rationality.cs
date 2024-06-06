using System.Collections;
using System.Collections.Generic;
using System.IO;
using Syuntoku.Status;
using UnityEngine;

namespace Syuntoku.DigMode.ParkData
{


    public class Park_009_Rationality : ParkBase
    {
        float damageUpPribility;
        float enebleLength;
        bool bSecondSwing;

        public override void Loading(StringReader reader)
        {
            base.Loading(reader);

            damageUpPribility = float.Parse(LoadLine(reader));
            enebleLength = float.Parse(LoadLine(reader));
        }

        public override void SwingUpdate(StatusManage baseStatus, ParkConditionsManage parkConditionsManage)
        {
            bSecondSwing = false;
            if (parkConditionsManage.blockState_Distance >= enebleLength)
            {
                bSecondSwing = true;
                baseStatus.digmodeStatus.AddDigDamagePlibility(damageUpPribility);
            }
        }

        public override void EndSwing(StatusManage baseStatus)
        {
            if(bSecondSwing)
            {
                baseStatus.digmodeStatus.AddDigDamagePlibility(-damageUpPribility);
            }
        }

    }
}
