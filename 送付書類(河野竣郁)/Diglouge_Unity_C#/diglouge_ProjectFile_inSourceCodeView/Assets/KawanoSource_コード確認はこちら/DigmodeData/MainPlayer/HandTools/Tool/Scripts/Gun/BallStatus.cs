using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Syuntoku.Status
{
    public class BallStatus
    {
        public enum BallStatusKind
        {
            Damage,
            SpeedMagnification,
            ReflectionCount,
            ReflectionSpeedUp,
        }

        public float _speedMagnification;
        public int _reflectionCount;
        public float _reflectionSpeedUp;
        public float _destoryTime;
        public float _knockBackPower;
        public float _enemyDamage;

        public GunPublicStatus _publicGunStatus;

        public BallStatus()
        {
            _reflectionCount = 1;
            _speedMagnification = 1.0f;
            _reflectionSpeedUp = 1.0f;
            _publicGunStatus = new GunPublicStatus();
        }

        public BallStatus(BallStatus ballStatus)
        {
            _reflectionCount = ballStatus._reflectionCount;
            _speedMagnification = ballStatus._speedMagnification;
            _reflectionSpeedUp = ballStatus._reflectionSpeedUp;
            _publicGunStatus = new GunPublicStatus(ballStatus._publicGunStatus);
        }


        /// <summary>
        /// 反射後のステータス変更をアクティブにする
        /// </summary>
        /// <param name="ballStatus"></param>
        public void ReflectActive(BallStatus ballStatus)
        {
            _publicGunStatus.Active(ballStatus);
        }

        public void KindToAddStatus(BallStatusKind ballStatus , float amount)
        {
            switch (ballStatus)
            {
                case BallStatusKind.SpeedMagnification:
                    _speedMagnification += amount;
                    break;
                case BallStatusKind.ReflectionCount:
                    _reflectionCount += (int)amount;
                    break;
                case BallStatusKind.ReflectionSpeedUp:
                    _speedMagnification += amount;
                    break;
            }
        }

        public void Update()
        {
            _publicGunStatus.Update(this);
        }
    }

    public class GunPublicStatus
    {
        List<GunAction> gunActions;

        public GunPublicStatus()
        {
            gunActions = new List<GunAction>();
        }

        public GunPublicStatus(GunPublicStatus gunPublicStatus)
        {
            gunActions = new List<GunAction>(gunPublicStatus.gunActions);
        }

        public void Active(BallStatus ballStatus)
        {
            foreach (GunAction item in gunActions)
            {
                item.Active(ballStatus);
            }
        }

        public void Update(BallStatus ballStatus)
        {
            foreach (GunAction item in gunActions)
            {
                item.Update(ballStatus);
            }
        }

        public void AddReflectionStatus(BallStatus.BallStatusKind ballStatusKind,float amount,float activeTime)
        {
            GunAction gunAction = new GunAction();

            gunAction.Initialize(ballStatusKind, amount, activeTime);

            gunActions.Add(gunAction);
        }
    }

    public class GunAction
    {
        bool _bStartTime;
        float _time;
        float _activeTime;

        BallStatus.BallStatusKind _changeStatus;
        float _amount;

        public void Initialize(BallStatus.BallStatusKind ballStatus,float  amount , float activeTime)
        {
            _bStartTime = false;
            _changeStatus = ballStatus;
            _amount = amount;
            _activeTime = activeTime;
        }

        public void Active(BallStatus ballStatus)
        {
            if(_bStartTime)
            {
                ballStatus.KindToAddStatus(_changeStatus, _amount);
            }

            _bStartTime = true;
            _time = 0.0f;
        } 

        public void Update(BallStatus ballStatus)
        {
            if (!_bStartTime) return;

            _time += Time.deltaTime;

            if (_time >= _activeTime)
            {
                _bStartTime = false;
                ballStatus.KindToAddStatus(_changeStatus, -_amount);
            }
        }
    }
}
