using UnityEngine;
using Syuntoku.Status;

namespace Syuntoku.DigMode.Tool.Scriptable
{
    [System.Serializable]
    public class ToolStatus
    {
        public ToolGenerater.ToolName toolKind;
        public string toolName;

        public int startLevel;
        public int maxLevel;

        public bool bUseBlock;
        public float interval;
        public float toolReach;

        public float enemyAttack;
        public float knockBackPower;

        public DamageManager damageManager;
        public int weight;

        public int addDrawLineCount;

        [Header("�������")]
        public string explanation;

        [Header("����̐��\���e�L�X�g�ɂ�������")]
        public string perfomanceExplanation;
        public Sprite toolIcon;
        public Sprite toolSquareIcon;

        [Header("SE")]
        [SerializeField]public AudioClip hitSound;
        [SerializeField]public AudioClip hitcriticalSound;

        [Header("Efect")]
        public GameObject hitContactPointEfect;
        public GameObject hitBreakPointEfect;

        [Header("�c�[���I�u�W�F�N�g")]
        public GameObject toolObject;
    }
}
