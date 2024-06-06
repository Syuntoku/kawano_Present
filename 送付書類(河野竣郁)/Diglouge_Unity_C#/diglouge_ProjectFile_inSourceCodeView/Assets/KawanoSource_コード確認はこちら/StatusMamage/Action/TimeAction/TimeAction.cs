using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Syuntoku.Status
{

    /// <summary>
    /// ŠÔ‚ÅÁ‚¦‚éˆ—
    /// </summary>
    class TimeAction : Action
    {
        bool _bStartTimer;
        float _timer;
        float _enableTime;

        public void Setup(PublicStatus.KeyAction keyAction, PublicStatus.ActiveTrigger activeAction, Digmode.StatusKind statusKind, float amount, float enableTime,bool bTemporary = false, bool inactive = false)
        {
            base.SetAction(keyAction, activeAction, statusKind, amount, bTemporary, inactive);

            _enableTime = enableTime;
            _timer = 0.0f;
        }

        public override void ActiveTrigger(StatusManage statusManage, PublicStatus.ActiveTrigger activeTrigger)
        {
            if (_activeTrigger != activeTrigger) return;

            if (!_bStartTimer)
            {
                statusManage.digmodeStatus.KindToAddStatus(_changeStatusKind, _amount);
            }

            _bStartTimer = true;
            _timer = 0.0f;
        }

        public override void Update(StatusManage statusManage)
        {
            if (!_bStartTimer) return;
            _timer += Time.deltaTime;

            if (_timer >= _enableTime)
            {
                //Œø‰Ê‚ªÁ‚¦‚È‚¢
                if (!_inactive)
                {
                    _bActive = false;
                }

                _bStartTimer = false;
                _timer = 0.0f;
                DisableAction(statusManage);
            }
        }
    }
}
