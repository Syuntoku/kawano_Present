using System.Collections;
using System.Collections.Generic;
using System.IO;
using Syuntoku.Status;
using UnityEngine;

namespace Syuntoku.DigMode.ParkData
{

    /// <summary>
    /// 23
    /// ブロックを破壊した後の５秒間、クリティカル攻撃発生率が（10/20/30）%UPする。
    /// </summary>
    public class Park_023_ : ParkBase
    {
        float powerUpDuration;
        int immediateDestruction;

        bool isBreaked;
        float timer;

        public override void Loading(StringReader reader)
        {
            base.Loading(reader);
            powerUpDuration = float.Parse(LoadLine(reader));
            immediateDestruction = int.Parse(LoadLine(reader));
        }

        public override void Start(StatusManage baseStatus, ParkConditionsManage parkConditionsManage)
        {
        }

        public override void Update(StatusManage baseStatus, ParkConditionsManage parkConditionsManage, float addTimier)
        {
            if (!isBreaked) return;

            timer += addTimier;

            if (timer >= powerUpDuration)
            {
                timer = 0;
                isBreaked = false;
                baseStatus.digmodeStatus.AddImmediatelyBreakManification(-immediateDestruction);
            }
        }

        public override void BreakUpDate(StatusManage baseStatus, ParkConditionsManage parkConditionsManage)
        {
            if (!isBreaked)
            {
                isBreaked = true;
                baseStatus.digmodeStatus.AddImmediatelyBreakManification(immediateDestruction);
            }

        }

    }
}
