using System.Collections;
using System.Collections.Generic;
using System.IO;
using Syuntoku.Status;
using UnityEngine;

namespace Syuntoku.DigMode.ParkData
{

    /// <summary>
    /// �A�z
    /// 10�b���Ƃɍ̌@���[�h�ƈړ����[�h���؂�ւ��B
    /// �̌@���[�h���̌@�_���[�W�i30%/40%/50%�jUP�{�ړ����x(40%/60%/80%)Down
    /// �ړ����[�h���ړ����x�i40%/60%/80%�jUP�{�̌@�_���[�W(70%/80%/90%)Down
    /// </summary>
    public class Park_018_YinAndYang : ParkBase
    {
        float _time;
        float _modeChangeTime;
        bool _modeDig;
        float _modeDigDamageUp;
        float _modeDigSpeedDown;
        float _modeMoveDamageDown;
        float _modeMoveSpeedUp;
        bool _enterState = true;

        public override void Loading(StringReader reader)
        {
            base.Loading(reader);

            _modeChangeTime = float.Parse(LoadLine(reader));
            LoadLine(reader, true);
            _modeDigDamageUp = float.Parse(LoadLine(reader));
            _modeDigSpeedDown = float.Parse(LoadLine(reader));
            LoadLine(reader, true);
            _modeMoveDamageDown = float.Parse(LoadLine(reader));
            _modeDigSpeedDown = float.Parse(LoadLine(reader));
        }

        public override void Update(StatusManage baseStatus, ParkConditionsManage parkConditionsManage, float addTimier)
        {
            _time += addTimier;
            if (_time >= _modeChangeTime)
            {
                _modeDig = !_modeDig;
                _time = 0f;
                _enterState = true;
            }

            if (_modeDig)
            {
                if (!_enterState) return;

                baseStatus.digmodeStatus.AddDigDamagePlibility(-_modeMoveDamageDown);
                baseStatus.digmodeStatus.AddSpeedMagnification(-_modeMoveSpeedUp);

                baseStatus.digmodeStatus.AddDigDamagePlibility(_modeDigDamageUp);
                baseStatus.digmodeStatus.AddSpeedMagnification(_modeDigSpeedDown);
                _enterState = false;
            }
            else
            {
                if (!_enterState) return;
                baseStatus.digmodeStatus.AddDigDamagePlibility(-_modeDigDamageUp);
                baseStatus.digmodeStatus.AddSpeedMagnification(-_modeDigSpeedDown);

                baseStatus.digmodeStatus.AddDigDamagePlibility(_modeMoveDamageDown);
                baseStatus.digmodeStatus.AddSpeedMagnification(_modeMoveSpeedUp);
                _enterState = false;
            }
        }

        public override void Disable(StatusManage baseStatus, ParkConditionsManage parkConditionsManage)
        {
            if (_modeDig)
            {
                baseStatus.digmodeStatus.AddDigDamagePlibility(-_modeDigDamageUp);
                baseStatus.digmodeStatus.AddSpeedMagnification(-_modeDigSpeedDown);
            }
            else
            {
                baseStatus.digmodeStatus.AddDigDamagePlibility(-_modeMoveDamageDown);
                baseStatus.digmodeStatus.AddSpeedMagnification(-_modeMoveSpeedUp);
            }

        }
    }
}
