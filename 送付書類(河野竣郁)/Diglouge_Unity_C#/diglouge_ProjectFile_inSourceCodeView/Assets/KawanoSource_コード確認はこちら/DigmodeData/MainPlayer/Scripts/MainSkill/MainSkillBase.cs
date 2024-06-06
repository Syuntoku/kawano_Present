using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Syuntoku.Status;

namespace Syuntoku.DigMode.Player.MainSkill
{

    public class MainSkillBase
    {
        protected int _skillId;
        public float _timer;
        public float _coolTime;

        bool _bUse;
        bool _bActive;
        float _cooltimeMagnification;
        float _subtractionCooltime;

        public MainSkillBase()
        {
            _bActive = false;
            _cooltimeMagnification = 1.0f;
            _subtractionCooltime = 1.0f;
        }

        public virtual void Initialize(GameObject data)
        {

        }

        public void SetCoolTime(float coolTime)
        {
            _coolTime = coolTime;
        }

        /// <summary>
        /// �A�N�e�B�u�ɂȂ����Ƃ��̏���
        /// </summary>
        /// <param name="playerObject"></param>
        /// <returns></returns>
        public virtual bool Active(GameObject playerObject)
        {
            return false;
        }

        /// <summary>
        /// �N�[���^�C���𑪂�
        /// </summary>
        public virtual void Update()
        {
            //�X�L�����g���I����Ă���
            if (IsUse() || IsActive()) return;

            _timer += Time.deltaTime;

            if(_timer >= (_coolTime - _subtractionCooltime) * _cooltimeMagnification)
            {
                _bUse = true;
                _timer = 0.0f;
            }
        }

        /// <summary>
        /// ���̃X�L���͎g�p�\��
        /// </summary>
        /// <returns>�g�p�\�Ftrue �g�p�s�Ffalse</returns>
        public bool IsUse()
        {
            return _bUse;
        }

        /// <summary>
        /// �X�L�����A�N�e�B�u�ɂ���
        /// </summary>
        public void Actived()
        {
            _bActive = true;
        }

        /// <summary>
        /// �X�L�����A�N�e�B�u�ɂȂ��Ă��邩
        /// </summary>
        /// <returns>�A�N�e�B�u:true�@��A�N�e�B�u�Ffalse </returns>
        public bool IsActive()
        {
            return _bActive;
        }

        /// <summary>
        /// �X�L���̎g�p���I������
        /// �N�[���^�C�����J�n
        /// </summary>
        public void ActiveEnd()
        {
            _bActive = false;
            UsedSkill();
        }

        /// <summary>
        /// �X�L�����g�p
        /// </summary>
        public void UsedSkill()
        {
            _bUse = true;
        }

        public void Reset()
        {
            _bUse = false;
            _bActive = true;
        }
    }
}
