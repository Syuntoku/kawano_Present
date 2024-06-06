using UnityEngine;

namespace Syuntoku.DigMode.Enemy.Scriptable
{
    /// <summary>
    /// 爆弾敵の追加設定
    /// </summary>
    [CreateAssetMenu(menuName ="Scriptable/Enemy/DetailSetting/Create BombEnemySetting")]
    public class BombEnemyScriptable : ScriptableObject
    {
        [Tooltip("爆発範囲")]
        public float explosionRange;
        [Tooltip("Rayを飛ばす距離　Default = 0.01f")]
        public float explosionLength;
        [Tooltip("プレイヤーへの爆発ダメージ")]
        public float playerDamage;
        [Tooltip("敵への爆発ダメージ")]
        public float enemyDamage;
        [Tooltip("敵へヒット時のノックバック力")]
        public float explosionKnokBackPower;
        public LayerMask explosinMask;
    }
}
