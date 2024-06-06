using System.Collections;
using System.Collections.Generic;
using System.IO;
using Syuntoku.Status;
using UnityEngine;

namespace Syuntoku.DigMode.ParkData
{


    public class Park_007_Dedication : ParkBase
    {
        const int ONE_BREAK_COUNT = 1;
        float damageUpPribility;
        float damageDownPribility;
        bool bOneBreak;

        public override void Loading(StringReader reader)
        {
            base.Loading(reader);

            damageUpPribility = float.Parse(LoadLine(reader));
            damageDownPribility =   float.Parse(LoadLine(reader));
        }


        public override void SwingUpdate(StatusManage baseStatus, ParkConditionsManage parkConditionsManage)
        {
            if(parkConditionsManage._oneSwingDamage == ONE_BREAK_COUNT)
            {
                baseStatus.digmodeStatus.AddDigDamagePlibility(damageUpPribility);
                bOneBreak = true;
            }
            else
            {
                baseStatus.digmodeStatus.AddDigDamagePlibility(damageDownPribility);
                bOneBreak = false;

            }
        }

        public override void EndSwing(StatusManage baseStatus)
        {
            if(bOneBreak)
            {
                baseStatus.digmodeStatus.AddDigDamagePlibility(-damageUpPribility);

            }
            else
            {
                baseStatus.digmodeStatus.AddDigDamagePlibility(-damageDownPribility);
            }
        }
    }
}
