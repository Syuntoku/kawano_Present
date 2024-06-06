using UnityEngine;
using System.Collections;

namespace Battle.Enemies
{

    internal class 爆発敵_Die : Enemy_Die
    {
        [SerializeField] private Explosion _explosionPrefab;
        [SerializeField] private GameObject _explosiveObject;

        public override void OnStateEnter()
        {
            Dead();
        }

        private void Dead()
        {
            var explosion = Instantiate(_explosionPrefab, _explosiveObject.transform.position, Quaternion.identity);
            explosion.Explode();

            _explosiveObject.SetActive(false);

            StartCoroutine(base.Die());
        }
    }

}