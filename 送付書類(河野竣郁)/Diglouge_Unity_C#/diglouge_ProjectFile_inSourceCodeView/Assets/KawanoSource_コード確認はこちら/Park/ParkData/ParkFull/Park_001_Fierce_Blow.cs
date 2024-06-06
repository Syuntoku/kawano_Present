using System.Collections;
using System.Collections.Generic;
using System.IO;
using Syuntoku.DigMode.Inventory;
using Syuntoku.Status;
using UnityEngine;
namespace Syuntoku.DigMode.ParkData
{

    public class Park_001_Fierce_Blow : ParkBase
    {
        const int MULTI_PULL = 2;
        float damageUpPlibility;
        bool _bActive;

        public override void Loading(StringReader reader)
        {
            base.Loading(reader);

            damageUpPlibility = float.Parse(LoadLine(reader));
        }

        public override void Start(StatusManage baseStatus, ParkConditionsManage parkConditionsManage)
        {
        }

        public override void Update(StatusManage baseStatus,ParkConditionsManage parkConditionsManage, float addTimier)
        {
            if(parkConditionsManage._oneSwingDamage >= MULTI_PULL)
            {
                if (_bActive) return;
                _bActive = true;
                baseStatus.digmodeStatus.AddDigDamagePlibility(damageUpPlibility);
            }
        }

        public override void EndSwing(StatusManage baseStatus)
        {
            if(_bActive)
            {
                _bActive = false;
                baseStatus.digmodeStatus.AddDigDamagePlibility(-damageUpPlibility);
            }
        }
    }
}
