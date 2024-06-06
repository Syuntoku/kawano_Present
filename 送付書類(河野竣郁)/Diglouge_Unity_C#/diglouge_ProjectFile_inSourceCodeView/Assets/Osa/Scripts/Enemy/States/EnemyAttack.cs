using UnityEngine;
using Battle.Game;

namespace Battle.Enemies
{

    internal class EnemyAttack : MonoBehaviour, IEnemyState
    {
        protected Enemy _enemy;
        protected Animator _animator;
        protected EnemyStateManager _stateManager;

        protected static readonly int HashAttack = Animator.StringToHash("Attack");

        public virtual void Init()
        {
            _enemy = GetComponent<Enemy>();
            _animator = GetComponent<Animator>();
            _stateManager = GetComponent<EnemyStateManager>();
        }

        public void OnStateEnter()
        {
            _animator.SetTrigger(HashAttack);
        }

        public void OnStateExit()
        {

        }

        public void OnUpdate()
        {

        }

        public virtual void Attack()
        {
            Player.Instance.ApplyDamage(new Damage(_enemy.EnemyParameter.ATK, gameObject, false));
        }

        public virtual void OnFinishAttackAnimation()
        {
            _stateManager.ChangeState(EnemyStates.Idle);
        }
    }

}