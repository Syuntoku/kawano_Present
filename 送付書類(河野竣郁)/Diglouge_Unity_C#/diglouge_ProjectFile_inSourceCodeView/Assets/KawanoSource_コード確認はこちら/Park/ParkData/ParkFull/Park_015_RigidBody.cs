using System.Collections;
using System.Collections.Generic;
using System.IO;
using Syuntoku.Status;
using UnityEngine;

namespace Syuntoku.DigMode.ParkData
{


    public class Park_015_RigidBody : ParkBase
    {
        float PossibleWeightAmount;
        public override void Loading(StringReader reader)
        {
            base.Loading(reader);

            PossibleWeightAmount = float.Parse(LoadLine(reader));
        }

        public override void Start(StatusManage baseStatus, ParkConditionsManage parkConditionsManage)
        {
            //baseStatus.digmodeStatus.CarryWeight += parkConditionsManage.game_MoveDistance * PossibleWeightAmount;
        }

    }
}
