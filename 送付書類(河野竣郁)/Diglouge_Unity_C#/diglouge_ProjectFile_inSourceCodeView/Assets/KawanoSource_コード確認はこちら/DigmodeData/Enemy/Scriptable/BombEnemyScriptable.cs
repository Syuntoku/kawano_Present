using UnityEngine;

namespace Syuntoku.DigMode.Enemy.Scriptable
{
    /// <summary>
    /// eGΜΗΑέθ
    /// </summary>
    [CreateAssetMenu(menuName ="Scriptable/Enemy/DetailSetting/Create BombEnemySetting")]
    public class BombEnemyScriptable : ScriptableObject
    {
        [Tooltip("­ΝΝ")]
        public float explosionRange;
        [Tooltip("RayπςΞ·£@Default = 0.01f")]
        public float explosionLength;
        [Tooltip("vC[ΦΜ­_[W")]
        public float playerDamage;
        [Tooltip("GΦΜ­_[W")]
        public float enemyDamage;
        [Tooltip("GΦqbgΜmbNobNΝ")]
        public float explosionKnokBackPower;
        public LayerMask explosinMask;
    }
}
