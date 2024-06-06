using UnityEngine;

namespace Syuntoku.DigMode
{
    [CreateAssetMenu(menuName = "Scriptable/Block/ExplosionBlockSetting")]
    public class ExplosionScriptable : ScriptableObject
    {
        [Tooltip("破壊範囲")]
        public float breakRange;
        public int explosionKnockBackPower;
        [Tooltip("爆発までの遅延する時間")]
        public int explosionDelay;
        [Tooltip("敵のダメージ量")]
        public float enemyDamage;
        [Tooltip("プレイヤーのダメージ")]
        public float playerDamage;
    }
}
