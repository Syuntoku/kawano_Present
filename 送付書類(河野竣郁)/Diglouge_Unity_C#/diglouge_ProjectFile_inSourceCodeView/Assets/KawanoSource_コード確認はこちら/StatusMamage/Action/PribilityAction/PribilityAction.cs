using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Syuntoku.Status
{
    public class PribilityAction : Action
    {
        int _plibility;

        public void Setup(PublicStatus.KeyAction keyAction, PublicStatus.ActiveTrigger activeTrigger, Digmode.StatusKind statusKind, float amount,int pribility, bool bTemporary = false, bool inactive = false)
        {
            SetAction(keyAction, activeTrigger, statusKind, amount, bTemporary, inactive);
            _plibility = pribility;
        }

        public override void Update(StatusManage statusManage)
        {
            base.Update(statusManage);
        }

        public override void ActiveTrigger(StatusManage statusManage, PublicStatus.ActiveTrigger activeTrigger)
        {
            if(GameUtility.CheckUnderParsent(_plibility))
            {
                ActiveTrigger(statusManage, activeTrigger);
            }
        }
    }
}
