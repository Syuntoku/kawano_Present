using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;

namespace Battle.Enemies
{

    internal class 爆弾敵_攻撃 : EnemyAttack, IMatchTarget
    {
        public Vector3 TargetPosition => _targetCollider.ClosestPoint(transform.position);

        private Player _target;
        private Collider _targetCollider;

        [SerializeField] private Volume _volume;
        public override void Init()
        {
            base.Init();

            _animator.keepAnimatorControllerStateOnDisable = true;


        }

        // アニメーションイベント
        public override void Attack()
        {
            _enemy.Dead();
        }

    }

}