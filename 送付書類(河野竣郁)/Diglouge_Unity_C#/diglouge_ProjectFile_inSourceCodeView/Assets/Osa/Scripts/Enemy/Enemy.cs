using UnityEngine;
using Battle.Game;
using UnityEngine.Events;
using Battle.Enemies;
using Cysharp.Threading.Tasks;

namespace Battle
{
    public class Enemy : MonoBehaviour
    {
        public EnemyParameter EnemyParameter => _enemyParameter;
        [SerializeField] private EnemyParameter _enemyParameter;

        public UnityEvent<Damage> OnDamage;

        public bool IsAlive => _isAlive;
        private bool _isAlive = true;

        public UniTask OnEnemyDeadAsync => _deadUTS.Task;
        private UniTaskCompletionSource _deadUTS = new UniTaskCompletionSource();

        public int MaxHP => _enemyParameter.MAXHP;
        public int HP => _health;
        private int _health;

        public int Exp => MaxHP;

        private EnemyStateManager _enemyStateManager;

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            _health = EnemyParameter.MAXHP;
            _enemyStateManager = GetComponent<EnemyStateManager>();
        }

        public void TakeDamage(Damage damage)
        {
            if (!_isAlive) return;

            _health -= damage.Value;

            _enemyStateManager.ChangeState(EnemyStates.TakeDamage);

            OnDamage.Invoke(damage);

            if (_health > 0 || !_isAlive) return;

            Dead();
        }

        public void Dead()
        {
            _isAlive = false;
            _enemyStateManager.ChangeState(EnemyStates.Die);
            _deadUTS.TrySetResult();
        }

        public void OnDestroy()
        {
            if (IsAlive) _deadUTS.TrySetCanceled();
        }
    }

}
