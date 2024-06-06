using Cysharp.Threading.Tasks;
using Syuntoku.DigMode.Enemy.Scriptable;
using UnityEngine;

namespace Syuntoku.DigMode.Enemy
{
    /// <summary>
    /// 飛行敵
    /// </summary>
    public class FlyEnemydata : EnemyBase
    {
        [SerializeField] FlyEnemyScriptable _flyEnemyScriptable;
        float _nearTime;
        float _breakTimer;

        RotateKind _rotateKind;
        enum RotateKind
        {
            RIGHT,
            LEFT,
            MAX,
        }

        void Start()
        {
            _rotateKind = (RotateKind)Random.Range((int)RotateKind.RIGHT, (int)RotateKind.MAX);
            Initialize();
            SetEndStopToTransitionState(EnemyState.SPECIFIC_BEHAVIOR_2);
        }

        protected override void SpecificBehavior_1(ThreadTransform transform,float height, ref Vector3 rdVelosity)
        {
            //プレイヤーを向く
            transform.quaternion = Quaternion.LookRotation(_playerTrans.transform.position - GetPivot(transform), Vector3.up);

            float angle = _flyEnemyScriptable.rotatePower * Mathf.Rad2Deg;
            //回転方向で角度の正負が変わる
            switch (_rotateKind)
            {
                case RotateKind.RIGHT:
                    angle *= 1;
                    break;
                case RotateKind.LEFT:
                    angle *= -1;
                    break;
            }

           this.transform.RotateAround(_playerTrans.transform.position, Vector3.up, angle * Time.deltaTime);
            transform.position = this.transform.position;

            _nearTime += Time.deltaTime;
            _breakTimer += Time.deltaTime;

            //周りのブロックを破壊する行動
            if (_breakTimer >= _flyEnemyScriptable.breakInterval)
            {
#if UNITY_EDITOR
                Debug.Log("RangeBreak");
#endif
                _breakTimer = 0;

                StayEnemy(_flyEnemyScriptable.waitTime);
                RangeDestruction(transform,_flyEnemyScriptable.breakRange,bBreakUnder:false);
            }

            //プレイヤーに少しずつ近づいていく
            if (_nearTime > _flyEnemyScriptable.nearTime)
            {
                Vector3 toPlayer = _playerTrans.transform.position - GetPivot(transform);
                transform.position += toPlayer.normalized * _flyEnemyScriptable.nearPower;
                StayEnemy(_flyEnemyScriptable.waitTime);
                _nearTime = 0.0f;
            }
        }

        /// <summary>
        /// プレイヤーに追従する
        /// </summary>
        protected override void SpecificBehavior_2(ThreadTransform transform, float height, ref Vector3 rdVelosity)
        {
            Vector3 toPlayer = _playerTrans.transform.position - GetPivot(transform);

            transform.position += toPlayer.normalized * _enemyStatus.speed * Time.deltaTime;

            if(height <= 0)
            {
                transform.position += Vector3.up * Time.deltaTime;
            }

            RaycastHit hit = LineOfSight();
            SetBlockBreak(hit);
            LookPlayer(transform, toPlayer);
        }

        protected override void ChangeState(ThreadTransform transform, float toPlayer)
        {
            float horizontalLength = DifferenceToPlayer(transform);

            if (horizontalLength <= _enemyStatus.attackPlayerLength * _enemyStatus.attackPlayerLength)
            {
                _enemyMoveState = EnemyState.ATTACK;
                return;
            }

            //ある一定近づくと回転を始める
            if (horizontalLength < _flyEnemyScriptable.rotateStayLength)
            {
                _enemyMoveState = EnemyState.SPECIFIC_BEHAVIOR_1;
            }
            else
            {
                _enemyMoveState = EnemyState.SPECIFIC_BEHAVIOR_2;
            }
        }
    }
}
