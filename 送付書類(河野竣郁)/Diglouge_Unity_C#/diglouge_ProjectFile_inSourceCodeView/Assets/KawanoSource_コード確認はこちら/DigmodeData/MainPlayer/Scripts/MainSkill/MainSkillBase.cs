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
        /// アクティブになったときの処理
        /// </summary>
        /// <param name="playerObject"></param>
        /// <returns></returns>
        public virtual bool Active(GameObject playerObject)
        {
            return false;
        }

        /// <summary>
        /// クールタイムを測る
        /// </summary>
        public virtual void Update()
        {
            //スキルを使い終わっている
            if (IsUse() || IsActive()) return;

            _timer += Time.deltaTime;

            if(_timer >= (_coolTime - _subtractionCooltime) * _cooltimeMagnification)
            {
                _bUse = true;
                _timer = 0.0f;
            }
        }

        /// <summary>
        /// このスキルは使用可能か
        /// </summary>
        /// <returns>使用可能：true 使用不可：false</returns>
        public bool IsUse()
        {
            return _bUse;
        }

        /// <summary>
        /// スキルをアクティブにする
        /// </summary>
        public void Actived()
        {
            _bActive = true;
        }

        /// <summary>
        /// スキルがアクティブになっているか
        /// </summary>
        /// <returns>アクティブ:true　非アクティブ：false </returns>
        public bool IsActive()
        {
            return _bActive;
        }

        /// <summary>
        /// スキルの使用が終了した
        /// クールタイムを開始
        /// </summary>
        public void ActiveEnd()
        {
            _bActive = false;
            UsedSkill();
        }

        /// <summary>
        /// スキルを使用
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
