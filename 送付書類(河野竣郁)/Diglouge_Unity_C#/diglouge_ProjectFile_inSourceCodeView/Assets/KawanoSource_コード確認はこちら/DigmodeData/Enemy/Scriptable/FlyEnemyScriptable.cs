using UnityEngine;

namespace Syuntoku.DigMode.Enemy.Scriptable
{
    [CreateAssetMenu(menuName = "Scriptable/Enemy/DetailSetting/Create FlyEnemySetting")]
    public class FlyEnemyScriptable : ScriptableObject
    {
        [Tooltip("回転し始める距離")]
        public float rotateStayLength;
        [Tooltip("回転している際の破壊範囲")]
        public float breakRange;
        [Tooltip("回転している際の破壊間隔")]
        public float breakInterval;
        [Tooltip("回転する角度")]
        public float rotatePower;

        [Tooltip("回転をやめる距離")]
        public float endRotateModeLength;
        [Tooltip("近づく力")]
        public float nearPower;
        [Tooltip("近づく時間間隔")]
        public float nearTime;
        [Tooltip("行動ごとの一時停止時間")]
        public float waitTime;
        [Tooltip("プレイヤーに追従するときの目標の調整")]
        public Vector3 trackPlayerAjust;
    }
}
