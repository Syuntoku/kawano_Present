using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Syuntoku.Status
{

    public class Action
    {
        protected bool _bActive;
        protected bool _inactive;
        protected int _activeCount;
        public int _index;
        protected PublicStatus.KeyAction _actionKind;
        protected Digmode.StatusKind _changeStatusKind;
        protected PublicStatus.ActiveTrigger _activeTrigger;
        protected float _amount;
        bool _bTemporary;

        /// <summary>
        /// アクションを登録 最初の設定が必須
        /// </summary>
        /// <param name="keyAction">アクションのトリガー</param>
        /// <param name="statusKind">ステータスを変更する種類</param>
        /// <param name="amount">変化量　合わない型の場合はキャストする</param>
        /// <param name="bTemporary">一時的に存在するか　リセットのメソッドで効果が消える</param>
        /// <param name="inactive">効果が消えない　デフォルトは一回効果を発動すると消える</param>
        protected void SetAction(PublicStatus.KeyAction keyAction, PublicStatus.ActiveTrigger activeTrigger, Digmode.StatusKind statusKind, float amount, bool bTemporary = false , bool inactive = false)
        {
            _actionKind = keyAction;
            _changeStatusKind = statusKind;
            _activeTrigger = activeTrigger;
            _amount = amount;
            _bTemporary = bTemporary;
            _inactive = inactive;
            _bActive = true;
        }

        /// <summary>
        /// 受け取ったトリガー
        /// </summary>
        /// <param name="activeAction"></param>
        public virtual void ActiveTrigger(StatusManage statusManage, PublicStatus.ActiveTrigger activeTrigger)
        {

        }

        public virtual void Update(StatusManage statusManage)
        {

        }

        public void EnableAction(StatusManage statusManage)
        {
            statusManage.digmodeStatus.KindToAddStatus(_changeStatusKind, -_amount);
        }

        /// <summary>
        /// 変更したステータスを消す
        /// </summary>
        /// <param name="statusManage"></param>
        public void DisableAction(StatusManage statusManage)
        {
            statusManage.digmodeStatus.KindToAddStatus(_changeStatusKind, -_amount);
        }

        public PublicStatus.KeyAction GetKeyAction()
        {
            return _actionKind;
        }

        public bool IsActive()
        {
            return _bActive;
        }

        public bool IsTemporary()
        {
            return _bTemporary;
        }
    }
}
