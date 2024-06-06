using System.Collections;
using UnityEngine;

namespace Battle.Enemies
{

    internal class Enemy_Die : MonoBehaviour, IEnemyState
    {

        [Header("éÄñSéûÇÃóéâ∫ë¨ìxÅAîjâÛÇ‹Ç≈ÇÃéûä‘")]
        [SerializeField] private float _fallSpeedOnDeath = 1.0f;
        [SerializeField] private float _fallTimeOnDeath = 10f;

        [SerializeField] private GameObject _vfx;

        private Animator _animator;
        private static readonly int HashDie = Animator.StringToHash("Die");

        private Gem[] _gems;
        private Enemy _enemy;

        public virtual void Init()
        {
            TryGetComponent(out _enemy);
            TryGetComponent(out _animator);

            _gems = gameObject.GetComponentsInChildren<Gem>();
        }

        public virtual void OnStateEnter()
        {
            StartCoroutine(Die());
        }

        public virtual void OnStateExit()
        {

        }

        protected IEnumerator Die()
        {
            _animator.SetTrigger(HashDie);

            // var effectObj = Instantiate(_vfx, transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
            //Destroy(effectObj.gameObject, effectObj.main.duration);

            Destroy(this.gameObject, 2f);

            int exp = _enemy.Exp / _gems.Length;
            int modExp = _enemy.Exp % _gems.Length;

            int e = exp + modExp;
            foreach (var gem in _gems)
            {
                gem.transform.parent = null;
                gem.FollowStart(e);

                e = exp;
            }

            if (BattleManager.Instance != null)
            {
                BattleManager.Instance.KillCount++;
            }

            var timer = 0f;
            while (timer <= _fallTimeOnDeath)
            {
                timer += Time.deltaTime;
                transform.position += Vector3.down * _fallSpeedOnDeath * Time.deltaTime;
                yield return null;
            }

            // SE,Effectçƒê∂
        }

        public virtual void OnUpdate()
        {

        }
    }

}