using UnityEngine;

namespace Syuntoku.DigMode.Enemy
{
    /// <summary>
    /// 地面敵
    /// </summary>
    public class GroundEnemy : EnemyBase
    {
        [SerializeField] int climbLength;
        [SerializeField] float climbUpperRayLength;
        [SerializeField] float climbPower;
        bool _bClimb;
        Ray wallRay;
        const float FORWARD_ADJUSTMENT_POWER = 10.0f;
        const float CLIMB_START_AJUSTMENT = 5.0f; //壁に沿って上る際に少しだけ上の位置に動かす力の調整
        const float UPPER_RAY_RANGE = 0.2f;
        const float UPPER_RAY_AJUSTMENT_START_POS = 3.0f;

        void Start()
        {
            Initialize();
        }

        protected override void SpecificBehavior_1(ThreadTransform transform ,float toPlayer, ref Vector3 rdVelosity)
        {
            RaycastHit forwordBlock = LineOfSight();

            if (!_bClimb)
            {                
                RaycastHit upHit = CheckUpperBlock();

                if (forwordBlock.collider == null)
                {
                    TrackPlayer(transform, ref rdVelosity);
                    return;
                }

                if (forwordBlock.distance >= _enemyStatus.nearForwordBlockLength)
                {
                    if (forwordBlock.distance >= 0.5f)
                    {
                        SetBlockBreak(CheckUpperBlock());
                    }
                    TrackPlayer(transform,ref rdVelosity);
                    return;
                }

                if (!BlockManage.IsBlock(forwordBlock)) return;
                _bClimb = true;
                wallRay.origin = transform.position;
                wallRay.direction = transform.forward;
                transform.position += Vector3.up / CLIMB_START_AJUSTMENT + -transform.forward / FORWARD_ADJUSTMENT_POWER;
            }
            else
            {
                Vector3 ray = wallRay.origin;
                ray.y += climbPower * Time.deltaTime;
                wallRay.origin = ray;
                rdVelosity = Vector3.zero;
#if UNITY_EDITOR
                Debug.DrawRay(wallRay.origin, wallRay.direction * UNDERBREAKRAYLENGTH, Color.cyan);
                Debug.DrawRay(wallRay.origin, Vector3.up * climbUpperRayLength, Color.grey);
#endif
                RaycastHit hit;

                if (Physics.Raycast(wallRay, out hit, UNDERBREAKRAYLENGTH, _enemyStatus.layerMask))
                {
                    Vector3 position = hit.point;
                    position += transform.quaternion *  _enemyStatus.tall;
                    transform.position = position + -transform.forward / FORWARD_ADJUSTMENT_POWER;

                    RaycastHit[] upperHit = Physics.SphereCastAll(transform.position + -transform.forward / UPPER_RAY_AJUSTMENT_START_POS, UPPER_RAY_RANGE, Vector3.up, climbUpperRayLength);

                    foreach (var item in upperHit)
                    {
                        SetBlockBreak(item);
                    }

                    //プレイヤーとの距離が近い場合は上るのをやめる
                    if (DifferenceToPlayerY(transform) <= 0)
                    {
                        SetBlockBreak(CheckUpperBlock());

                        SetBlockBreak(hit);
                        ReSetClump();
                        
                    }
                }
                else
                {
                    _enemyMoveState = EnemyState.FORWARD_BREAK;
                    rdVelosity = Vector3.zero;
                    ReSetClump();
                }
            }

            if (!BlockManage.IsBlock(forwordBlock))
            {
                if (_bAttack) return;
                TrackPlayer(transform, ref rdVelosity);
                return;
            }
        }

        protected override void ChangeState(ThreadTransform transform, float toPlayer)
        {
            base.ChangeState(transform, toPlayer);

            if (toPlayer >= climbLength || _bClimb)
            {
                _enemyMoveState = EnemyState.SPECIFIC_BEHAVIOR_1;
            }
        }

        /// <summary>
        /// 上る動作を終了する
        /// </summary>
        void ReSetClump()
        {
            _bClimb = false;
            transform.rotation = Quaternion.FromToRotation(Vector3.up, Vector3.up);
            Jump();
        }
    }
}
