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

        [Header("マックスレベルの数だけ設定してください")]
        public DamageInfoHammer[] damageInfos;

        [Header("アップグレード画面のアイコン")]
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