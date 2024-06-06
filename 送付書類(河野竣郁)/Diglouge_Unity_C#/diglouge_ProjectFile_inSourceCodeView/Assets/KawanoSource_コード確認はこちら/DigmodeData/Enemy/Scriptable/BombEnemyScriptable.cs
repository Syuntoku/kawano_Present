using UnityEngine;

namespace Syuntoku.DigMode.Enemy.Scriptable
{
    /// <summary>
    /// ���e�G�̒ǉ��ݒ�
    /// </summary>
    [CreateAssetMenu(menuName ="Scriptable/Enemy/DetailSetting/Create BombEnemySetting")]
    public class BombEnemyScriptable : ScriptableObject
    {
        [Tooltip("�����͈�")]
        public float explosionRange;
        [Tooltip("Ray���΂������@Default = 0.01f")]
        public float explosionLength;
        [Tooltip("�v���C���[�ւ̔����_���[�W")]
        public float playerDamage;
        [Tooltip("�G�ւ̔����_���[�W")]
        public float enemyDamage;
        [Tooltip("�G�փq�b�g���̃m�b�N�o�b�N��")]
        public float explosionKnokBackPower;
        public LayerMask explosinMask;
    }
}
