using UnityEngine;

namespace Syuntoku.DigMode.Enemy.Scriptable
{
    [CreateAssetMenu(menuName = "Scriptable/Enemy/DetailSetting/Create FlyEnemySetting")]
    public class FlyEnemyScriptable : ScriptableObject
    {
        [Tooltip("��]���n�߂鋗��")]
        public float rotateStayLength;
        [Tooltip("��]���Ă���ۂ̔j��͈�")]
        public float breakRange;
        [Tooltip("��]���Ă���ۂ̔j��Ԋu")]
        public float breakInterval;
        [Tooltip("��]����p�x")]
        public float rotatePower;

        [Tooltip("��]����߂鋗��")]
        public float endRotateModeLength;
        [Tooltip("�߂Â���")]
        public float nearPower;
        [Tooltip("�߂Â����ԊԊu")]
        public float nearTime;
        [Tooltip("�s�����Ƃ̈ꎞ��~����")]
        public float waitTime;
        [Tooltip("�v���C���[�ɒǏ]����Ƃ��̖ڕW�̒���")]
        public Vector3 trackPlayerAjust;
    }
}
