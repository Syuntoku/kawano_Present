using UnityEngine;

namespace Syuntoku.DigMode.Tool.Scriptable
{
    [CreateAssetMenu(fileName ="PickAxeInfo",menuName = "Scriptable/ToolInfo/Create PickAxeScriptable")]
    public class PickAxeInfoScriptable : ScriptableObject
    {
        public ToolStatus tooldata;

        [Header("�}�b�N�X���x���̐������ݒ肵�Ă�������")]
        public DamageInfoPickAxe[] damageInfos;

        [Header("�A�b�v�O���[�h��ʂ̃A�C�R��")]
        public UpgradeIcon upgradeIcon;
    }

    [System.Serializable]
   public class DamageInfoPickAxe
    {
       public float damage;
       public float Interval;
    }
}
