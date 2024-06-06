using System.Collections;
using System.Collections.Generic;
using System.IO;
using Syuntoku.Status;
using UnityEngine;

namespace Syuntoku.DigMode.ParkData
{

    /// <summary>
    /// 22
    /// メインスキルを発動した直後の(4/8/12)秒間、採掘ダメージが20%UPする。
    /// </summary>
    public class Park_022_ : ParkBase
    {
        float powerUpDuration;
        float damageUpPlibility;

        bool isMainSkillExecuted;
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
            if (!isMainSkillExecuted) return;

            timer += addTimier;

            if (timer >= powerUpDuration)
            {
                timer = 0;
                isMainSkillExecuted = false;
                baseStatus.digmodeStatus.AddDigDamagePlibility(-damageUpPlibility);
            }
        }

        public override void OnExecuteMainSkill(StatusManage baseStatus, ParkConditionsManage parkConditionsManage, float addTimier)
        {
            if (isMainSkillExecuted) return;

            isMainSkillExecuted = true;
            baseStatus.digmodeStatus.AddDigDamagePlibility(damageUpPlibility);
        }
    }
}
