using UnityEngine;
using DG.Tweening;
using Syuntoku.DigMode.Enemy.Scriptable;

namespace Syuntoku.DigMode.Enemy
{
    public class BombEnemy : GroundEnemy
    {
        [Header("îöíeÇÃê›íË")]
        [SerializeField] GameObject _bombObject;
        [SerializeField] GameObject _explosionEfect;
        [SerializeField] BombEnemyScriptable _bombEnemyScriptable;

        public override void DownEnemy()
        {
            Explosion();
            base.DownEnemy();
        }

        void Explosion()
        {
            Instantiate(_explosionEfect, _bombObject.transform.position, Quaternion.identity);

            //îÕàÕì‡Ç…É_ÉÅÅ[ÉWÇó^Ç¶ÇÈ
            RaycastHit[] explosionHits = Physics.SphereCastAll(_bombObject.transform.position, _bombEnemyScriptable.explosionRange, Vector3.up, _bombEnemyScriptable.explosionLength);

            foreach (RaycastHit hit in explosionHits)
            {
                Vector3 toBomb = hit.transform.position - _bombObject.transform.position;

                if (hit.collider.CompareTag(Player.Player.PLAYER_TAG))
                {
                    Player.Player player = hit.collider.gameObject.GetComponent<Player.Player>();
                    player.SendDamage(_bombEnemyScriptable.playerDamage, _bombObject.transform);
                }

                if (hit.collider.CompareTag(ENEMY_TAG))
                {
                    EnemyBase enemy = hit.collider.gameObject.GetComponent<EnemyBase>();
                    enemy.SendDamage(_bombEnemyScriptable.enemyDamage * GetWaveAttackPowerMagnification(), _bombEnemyScriptable.explosionKnokBackPower, toBomb);
                }
            }
            Destroy(_bombObject);
        }
    }
}
