using System.Collections;
using System.Collections.Generic;
using System.IO;
using Syuntoku.Status;
using UnityEngine;

namespace Syuntoku.DigMode.ParkData
{


    public class Park_017_GradualIncrease : ParkBase
    {
        float _digDaamageupMagnification;
        float _speedMagnification;
        int _activeHielalcy;

        const int HIERARCY1 = 10;
        const int HIERARCY2 = 20;
        const int HIERARCY3 = 30;
        const int HIERARCY4 = 50;
        const int HIERARCY5 = 80;

        public override void Loading(StringReader reader)
        {
            base.Loading(reader);

            _speedMagnification = float.Parse(LoadLine(reader));
            _digDaamageupMagnification = float.Parse(LoadLine(reader));
        }

        public override void Update(StatusManage baseStatus, ParkConditionsManage parkConditionsManage, float addTimier)
        {
            Disable(baseStatus, parkConditionsManage);

            if (parkConditionsManage.hierarchy >= HIERARCY1)
            {
                _activeHielalcy = 1;
                baseStatus.digmodeStatus.AddDigDamagePlibility(_digDaamageupMagnification);
                baseStatus.digmodeStatus.AddSpeedMagnification(_speedMagnification);
            }
            if (parkConditionsManage.hierarchy >= HIERARCY2)
            {
                _activeHielalcy = 2;
                baseStatus.digmodeStatus.AddDigDamagePlibility(_digDaamageupMagnification);
                baseStatus.digmodeStatus.AddSpeedMagnification(_speedMagnification);
            }
            if (parkConditionsManage.hierarchy >= HIERARCY3)
            {
                _activeHielalcy = 3;
                baseStatus.digmodeStatus.AddDigDamagePlibility(_digDaamageupMagnification);
                baseStatus.digmodeStatus.AddSpeedMagnification(_speedMagnification);
            }
            if (parkConditionsManage.hierarchy >= HIERARCY4)
            {
                _activeHielalcy = 4;
                baseStatus.digmodeStatus.AddDigDamagePlibility(_digDaamageupMagnification);
                baseStatus.digmodeStatus.AddSpeedMagnification(_speedMagnification);
            }
            if (parkConditionsManage.hierarchy >= HIERARCY5)
            {
                _activeHielalcy = 5;
                baseStatus.digmodeStatus.AddDigDamagePlibility(_digDaamageupMagnification);
                baseStatus.digmodeStatus.AddSpeedMagnification(_speedMagnification);
            }
        }

        public override void Disable(StatusManage baseStatus, ParkConditionsManage parkConditionsManage)
        {
            switch (_activeHielalcy)
            {
                case 1:
                    baseStatus.digmodeStatus.AddDigDamagePlibility(-_digDaamageupMagnification);
                    baseStatus.digmodeStatus.AddSpeedMagnification(-_speedMagnification);
                    break;
                case 2:
                    baseStatus.digmodeStatus.AddDigDamagePlibility(-_digDaamageupMagnification);
                    baseStatus.digmodeStatus.AddSpeedMagnification(-_speedMagnification);
                    break;
                case 3:
                    baseStatus.digmodeStatus.AddDigDamagePlibility(-_digDaamageupMagnification);
                    baseStatus.digmodeStatus.AddSpeedMagnification(-_speedMagnification);
                    break;
                case 4:
                    baseStatus.digmodeStatus.AddDigDamagePlibility(-_digDaamageupMagnification);
                    baseStatus.digmodeStatus.AddSpeedMagnification(_speedMagnification);
                    break;
                case 5:
                    baseStatus.digmodeStatus.AddDigDamagePlibility(-_digDaamageupMagnification);
                    baseStatus.digmodeStatus.AddSpeedMagnification(-_speedMagnification);
                    break;
            }
        }
    }
}
