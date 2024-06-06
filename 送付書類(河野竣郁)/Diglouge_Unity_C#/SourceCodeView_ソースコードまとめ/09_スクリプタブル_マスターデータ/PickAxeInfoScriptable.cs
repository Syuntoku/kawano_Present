using UnityEngine;

namespace Syuntoku.DigMode.Tool.Scriptable
{
    [CreateAssetMenu(fileName ="PickAxeInfo",menuName = "Scriptable/ToolInfo/Create PickAxeScriptable")]
    public class PickAxeInfoScriptable : ScriptableObject
    {
        public ToolStatus tooldata;

        [Header("マックスレベルの数だけ設定してください")]
        public DamageInfoPickAxe[] damageInfos;

        [Header("アップグレード画面のアイコン")]
        public UpgradeIcon upgradeIcon;
    }

    [System.Serializable]
   public class DamageInfoPickAxe
    {
       public float damage;
       public float Interval;
    }
}
