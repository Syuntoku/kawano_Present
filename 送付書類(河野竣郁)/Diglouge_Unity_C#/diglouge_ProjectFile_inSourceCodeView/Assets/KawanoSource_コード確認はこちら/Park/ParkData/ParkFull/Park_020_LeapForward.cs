using System.Collections;
using System.Collections.Generic;
using System.IO;
using Syuntoku.Status;
using UnityEngine;

namespace Syuntoku.DigMode.ParkData
{

    /// <summary>
    /// 20:飛躍
    /// 空中でブロックに採掘ダメージを与えている際に、移動速度と採掘ダメージが(4/8/12)%UPする。
    /// </summary>
    public class Park_020_LeapForward : ParkBase
    {
        float damageUpMagnification;
        float speedUpMagnification;

        bool isPowerUp;
        public override void Loading(StringReader reader)
        {
            base.Loading(reader);
            damageUpMagnification = float.Parse(LoadLine(reader));
            speedUpMagnification = float.Parse(LoadLine(reader));
        }

        public override void Start(StatusManage baseStatus, ParkConditionsManage parkConditionsManage)
        {
        }

        public override void Update(StatusManage baseStatus, ParkConditionsManage parkConditionsManage, float addTimier)
        {
            if (!isPowerUp) return;

            if (!parkConditionsManage.IsJump)
            {
                isPowerUp = false;

                baseStatus.digmodeStatus.AddDigDamagePlibility(-damageUpMagnification);
                baseStatus.digmodeStatus.AddAirSpeedMagnification(-speedUpMagnification);
            }
        }

        // ブロックを殴ったとき
        public override void OnHitBlock(StatusManage baseStatus, ParkConditionsManage parkConditionsManage, float addTimier)
        {
            if (parkConditionsManage.IsJump && !isPowerUp)
            {
                isPowerUp = true;

                baseStatus.digmodeStatus.AddDigDamagePlibility(damageUpMagnification);
                baseStatus.digmodeStatus.AddAirSpeedMagnification(speedUpMagnification);
            }
        }

    }
}
