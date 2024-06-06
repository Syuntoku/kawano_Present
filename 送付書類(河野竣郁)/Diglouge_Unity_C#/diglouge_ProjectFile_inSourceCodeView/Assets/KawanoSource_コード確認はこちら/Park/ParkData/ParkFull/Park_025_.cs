using System.Collections;
using System.Collections.Generic;
using System.IO;
using Syuntoku.Status;
using UnityEngine;

namespace Syuntoku.DigMode.ParkData
{

    /// <summary>
    /// 25
    /// 敵にダメージを受けた後の8秒間、採掘ダメージが(40/60/80)%UPする。
    /// </summary>
    public class Park_025_ : ParkBase
    {
        float powerUpDuration;
        float damageUpPlibility;

        bool isDamaged;
        float timer;

        public override void Loading(StringReader reader)
        {
            base.Loading(reader);
            powerUpDuration = float.Parse(LoadLine(reader));
            damageUpPlibility = float.Parse(LoadLine(reader));
        }

        public override void Start(StatusManage baseStatus, ParkConditionsManage parkConditionsManage)
        {

        }

        public override void Update(StatusManage baseStatus, ParkConditionsManage parkConditionsManage, float addTimier)
        {
            if (!isDamaged) return;

            timer += addTimier;

            if (timer >= powerUpDuration)
            {
                timer = 0;
                isDamaged = false;
                baseStatus.digmodeStatus.AddDigDamagePlibility(-damageUpPlibility);
            }
        }

        public override void OnDamage(StatusManage baseStatus, ParkConditionsManage parkConditionsManage, float addTimier)
        {
            if (!isDamaged)
            {
                isDamaged = true;

                baseStatus.digmodeStatus.AddDigDamagePlibility(damageUpPlibility);
            }
        }

    }
}
