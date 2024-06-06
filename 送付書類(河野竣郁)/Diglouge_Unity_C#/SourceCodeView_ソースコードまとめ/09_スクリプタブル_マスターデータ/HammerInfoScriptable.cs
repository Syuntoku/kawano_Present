using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Syuntoku.DigMode.Tool.Scriptable
{

    [CreateAssetMenu(fileName = "HammmerInfo", menuName = "Scriptable/ToolInfo/Create HammerScriptable")]
    public class HammerInfoScriptable : ScriptableObject
    {
        public ToolStatus tooldata;

        [Header("�}�b�N�X���x���̐������ݒ肵�Ă�������")]
        public DamageInfoHammer[] damageInfos;

        [Header("�A�b�v�O���[�h��ʂ̃A�C�R��")]
        public UpgradeIcon upgradeIcon;
    }

    [System.Serializable]
    public class DamageInfoHammer
    {
        public float centerDamage;
        public float middleLineDamage;
        public float outlineDamage;

        public float interval;
    }
}