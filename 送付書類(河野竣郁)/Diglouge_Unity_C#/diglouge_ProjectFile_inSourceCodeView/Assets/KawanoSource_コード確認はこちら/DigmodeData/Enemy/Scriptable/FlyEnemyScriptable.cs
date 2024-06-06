using UnityEngine;

namespace Syuntoku.DigMode.Enemy.Scriptable
{
    [CreateAssetMenu(menuName = "Scriptable/Enemy/DetailSetting/Create FlyEnemySetting")]
    public class FlyEnemyScriptable : ScriptableObject
    {
        [Tooltip("‰ñ“]‚µn‚ß‚é‹——£")]
        public float rotateStayLength;
        [Tooltip("‰ñ“]‚µ‚Ä‚¢‚éÛ‚Ì”j‰ó”ÍˆÍ")]
        public float breakRange;
        [Tooltip("‰ñ“]‚µ‚Ä‚¢‚éÛ‚Ì”j‰óŠÔŠu")]
        public float breakInterval;
        [Tooltip("‰ñ“]‚·‚éŠp“x")]
        public float rotatePower;

        [Tooltip("‰ñ“]‚ğ‚â‚ß‚é‹——£")]
        public float endRotateModeLength;
        [Tooltip("‹ß‚Ã‚­—Í")]
        public float nearPower;
        [Tooltip("‹ß‚Ã‚­ŠÔŠÔŠu")]
        public float nearTime;
        [Tooltip("s“®‚²‚Æ‚Ìˆê’â~ŠÔ")]
        public float waitTime;
        [Tooltip("ƒvƒŒƒCƒ„[‚É’Ç]‚·‚é‚Æ‚«‚Ì–Ú•W‚Ì’²®")]
        public Vector3 trackPlayerAjust;
    }
}
