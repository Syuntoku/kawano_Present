using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Syuntoku.Status;

namespace Syuntoku.DigMode.Player.MainSkill
{

    public class MainSkill_OverHeat : MainSkillBase
    {
       public const float DEFAULT_COOL_TIME = 90.0f;

        FirstPerson _firstParson;
        const float SPEED_UP_MAGNIFICATION = 1.0f;
        const float VIRTICAL_SPEED_UP_MAGNIFICATION = 1.0f;

        float _overHeatTimer;
        float MAX_ACTIVE_TIME = 6.0f;


        public override bool Active(GameObject playerObject)
        {
            if (!IsUse() || IsActive()) return false;
            _firstParson = playerObject.GetComponent<FirstPerson>();
            _firstParson._firstParsonStatus.AddSpeedMagnification(SPEED_UP_MAGNIFICATION);
            _firstParson._firstParsonStatus.AddVirticalSpeedMagnification(VIRTICAL_SPEED_UP_MAGNIFICATION);
            Actived();
            return true;
        }

        public override void Update()
        {
            base.Update();

            if (IsActive() || IsUse()) return;

            _overHeatTimer += Time.deltaTime;

            if(_overHeatTimer >= MAX_ACTIVE_TIME)
            {
                ActiveEnd();

                SetCoolTime(DEFAULT_COOL_TIME);
                _firstParson._firstParsonStatus.AddSpeedMagnification(-SPEED_UP_MAGNIFICATION);
                _firstParson._firstParsonStatus.AddVirticalSpeedMagnification(-VIRTICAL_SPEED_UP_MAGNIFICATION);
            }
        }
    }
}
