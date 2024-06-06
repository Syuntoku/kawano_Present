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
        /// �A�N�V������o�^ �ŏ��̐ݒ肪�K�{
        /// </summary>
        /// <param name="keyAction">�A�N�V�����̃g���K�[</param>
        /// <param name="statusKind">�X�e�[�^�X��ύX������</param>
        /// <param name="amount">�ω��ʁ@����Ȃ��^�̏ꍇ�̓L���X�g����</param>
        /// <param name="bTemporary">�ꎞ�I�ɑ��݂��邩�@���Z�b�g�̃��\�b�h�Ō��ʂ�������</param>
        /// <param name="inactive">���ʂ������Ȃ��@�f�t�H���g�͈����ʂ𔭓�����Ə�����</param>
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
        /// �󂯎�����g���K�[
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
        /// �ύX�����X�e�[�^�X������
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
