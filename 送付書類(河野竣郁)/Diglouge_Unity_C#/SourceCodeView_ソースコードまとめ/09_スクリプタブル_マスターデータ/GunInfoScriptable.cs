using Syuntoku.DigMode.Inventory.Juwelry;
using UnityEngine;

namespace Syuntoku.DigMode.Tool.Scriptable
{
    [CreateAssetMenu(fileName = "GunInfo", menuName = "Scriptable/ToolInfo/Create GunScriptable")]
    public class GunInfoScriptable : ScriptableObject
    {
        public ToolStatus tooldata;

        public float shotSpeed;
        public int shotCount;
        public float destroyTime;
        public GameObject shotPrefab;
        public int reflectCount;
        public LayerMask reflectMask;
      
        [Header("マックスレベルの数だけ設定してください")]
        public DamageInfo_Gun[] damageInfos;
        [Header("アップグレード画面のアイコン")]
        public UpgradeIcon upgradeIcon;

        public JuwelryCost[] firstUpgladeCost;
        public JuwelryCost[] secondUpgladeCost;
        public JuwelryCost[] thirdUpgladeCost;
    }

    [System.Serializable]
    public class DamageInfo_Gun
    {
        public float damage;
        public float interval;
    }
}
