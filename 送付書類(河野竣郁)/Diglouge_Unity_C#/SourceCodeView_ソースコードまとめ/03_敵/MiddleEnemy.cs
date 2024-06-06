using UnityEngine;

namespace Syuntoku.DigMode.Enemy
{
    /// <summary>
    /// 中型の敵
    /// </summary>
    public class MiddleEnemy : EnemyBase
    {
        [SerializeField,Header("設定した高さ以上で追従する")] int _trackPlayerHeight;
        [SerializeField,Header("設定した高さ以下で下に採掘する")] int _downMosionHeight;

        [Header("----------------中サイズ敵ステータス-----------------")] 
        [SerializeField] Vector3 _forwardRayAjust;
        [SerializeField] float _forwardRayRange;
        [SerializeField] float _forwardRayLength;
        [SerializeField] float _forwardInterval;
        [SerializeField] Vector3 _underRayAjust;
        [SerializeField] float _underRayRange;
        [SerializeField] float _underRayLength;

        public override void StartRayHitUpdate(ThreadTransform transform)
        {
            base.StartRayHitUpdate(transform);

            if (_enemyMoveState == EnemyState.FORWARD_BREAK || _enemyMoveState == EnemyState.TRACK_PLAYER)
            {
                transform.forwardHits = Physics.SphereCastAll(_ray, _forwardRayRange, _forwardRayLength, _blockMask);
            }
            if (_enemyMoveState == EnemyState.BOTTON_BREAK)
            {
                transform.downRayHits = Physics.SphereCastAll(_worldUnderRay, _underRayRange, _underRayLength);
            }
        }

        protected override void ForwardBreak(ThreadTransform transform, ref Vector3 rdVelosity)
        {
            TrackPlayer(transform, ref rdVelosity);
            _ray.origin = _ray.origin + _forwardRayAjust;

            if (transform.forwardHits != null)
            {
                SetBlockBreak(transform.forwardHits, _forwardInterval);
            }
        }

        protected override void Botton(ThreadTransform transform, float toPlayer, ref Vector3 rdVelosity)
        {
            _worldUnderRay.origin = _worldUnderRay.origin + _underRayAjust;

#if UNITY_EDITOR
            Debug.DrawRay(_worldUnderRay.origin, _worldUnderRay.direction, Color.blue);
#endif
            SetBlockBreak(transform.downRayHits, _enemyStatus.bottonBreakInterval);
        }

        protected override void ChangeState(ThreadTransform transform, float toPlayer)
        {
            if(toPlayer >= 0)
            {
                _enemyMoveState = EnemyState.FORWARD_BREAK;
            }
            else
            {
                _enemyMoveState = EnemyState.BOTTON_BREAK;
            }
            
            if(DifferenceToPlayerHorizontalSqrMag(transform) < _enemyStatus.attackPlayerLength * _enemyStatus.attackPlayerLength)
            {
                _enemyMoveState = EnemyState.ATTACK;
            }
        }
    }
}
