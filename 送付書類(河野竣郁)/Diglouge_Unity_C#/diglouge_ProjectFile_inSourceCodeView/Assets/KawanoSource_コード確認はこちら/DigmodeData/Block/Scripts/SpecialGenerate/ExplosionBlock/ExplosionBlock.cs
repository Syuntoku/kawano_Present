using Syuntoku.DigMode.Enemy;
using UnityEngine;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using static Syuntoku.DigMode.Enemy.EnemyBase;

namespace Syuntoku.DigMode
{
    /// <summary>
    /// 爆発ブロック
    /// </summary>
    public class ExplosionBlock : BlockData
    {
        [SerializeField]ExplosionScriptable _explosionScriptable;
        const float RAY_LENGTH = 0.01f;
        public override void BreakAction()
        {
            //範囲内にダメージを与える
            RaycastHit[] explosionHits = Physics.SphereCastAll(transform.position, _explosionScriptable.breakRange, Vector3.up, RAY_LENGTH);

            foreach (RaycastHit hit in explosionHits)
            {
                Vector3 toBomb = hit.transform.position - transform.position;

                if (hit.collider.CompareTag(Player.Player.PLAYER_TAG))
                {
                    Player.Player player = hit.collider.gameObject.GetComponent<Player.Player>();
                    player.SendDamage(_explosionScriptable.playerDamage, transform);
                }

                if (hit.collider.CompareTag(ENEMY_TAG))
                {
                    EnemyBase enemy = hit.collider.gameObject.GetComponent<EnemyBase>();
                    enemy.SendDamage(_explosionScriptable.enemyDamage, _explosionScriptable.explosionKnockBackPower, toBomb);
                }
            }
        }
    }
}
