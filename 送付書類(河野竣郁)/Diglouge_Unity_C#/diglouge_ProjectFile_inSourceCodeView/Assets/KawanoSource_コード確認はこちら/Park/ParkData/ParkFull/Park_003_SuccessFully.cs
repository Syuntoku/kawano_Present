using System.IO;
using Syuntoku.Status;
using UnityEngine;

namespace Syuntoku.DigMode.ParkData
{

    public class Park_003_SuccessFully : ParkBase
    {
        float damageUpPlibility;
        bool _bActive;

        public override void Loading(StringReader reader)
        {
            base.Loading(reader);

            damageUpPlibility = float.Parse(LoadLine(reader));
            _bActive = true;
        }

        public override void Update(StatusManage baseStatus, ParkConditionsManage parkConditionsManage, float addTimier)
        {
            if(_bActive)
            {
                if (!parkConditionsManage.bWaveDamage) return;
                Disable(baseStatus, parkConditionsManage);
            }
        }

        public override void Start(StatusManage baseStatus, ParkConditionsManage parkConditionsManage)
        {
            baseStatus.digmodeStatus.AddDigDamagePlibility(damageUpPlibility);
        }

        public override void Disable(StatusManage baseStatus, ParkConditionsManage parkConditionsManage)
        {
            baseStatus.digmodeStatus.AddDigDamagePlibility(-damageUpPlibility);
        }

    }
}
