using System.Collections;
using System.Collections.Generic;
using System.IO;
using Syuntoku.Status;
using UnityEngine;

namespace Syuntoku.DigMode.ParkData
{


    public class Park_113_Collect : ParkBase
    {
        float _baseMaxHpUpMagnification;
        float _hpUpAmount;

        public override void Loading(StringReader reader)
        {
            base.Loading(reader);
            _baseMaxHpUpMagnification = int.Parse(LoadLine(reader));
            _hpUpAmount = int.Parse(LoadLine(reader));
        }

        public override void EnemyKill(StatusManage baseStatus, ParkConditionsManage parkConditionsManage)
        {
            if(GameUtility.CheckUnderParsent((int)_baseMaxHpUpMagnification))
            {
            }
        }
    }
}
