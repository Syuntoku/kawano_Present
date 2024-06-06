using UnityEngine;

namespace Syuntoku.DigMode
{
    [CreateAssetMenu(menuName = "Scriptable/Block/ExplosionBlockSetting")]
    public class ExplosionScriptable : ScriptableObject
    {
        [Tooltip("�j��͈�")]
        public float breakRange;
        public int explosionKnockBackPower;
        [Tooltip("�����܂ł̒x�����鎞��")]
        public int explosionDelay;
        [Tooltip("�G�̃_���[�W��")]
        public float enemyDamage;
        [Tooltip("�v���C���[�̃_���[�W")]
        public float playerDamage;
    }
}
