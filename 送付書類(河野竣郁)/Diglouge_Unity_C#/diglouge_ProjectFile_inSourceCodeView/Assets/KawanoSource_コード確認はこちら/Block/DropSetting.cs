using UnityEngine;
using Syuntoku.DigMode.Inventory.Juwelry;

namespace Syuntoku.DigMode
{
    [System.Serializable]
    public class DropSetting
    {
        [Header("チェックを付けると設定したオブジェクトをドロップ")]
        [SerializeField] public bool bSetDropItem;

        [Header("SetDropItem: ■ 宝石をドロップ")]
        [SerializeField]  public JuwelryInventory.JUWELRY_KIND dropJuwelryKind;
        [Tooltip("ドロップする個数")]
        [SerializeField]  public int dropCount;
        [Tooltip("ドロップする確率")]
        [SerializeField]  public int dropPlibability;

        [Header("SetDropItem: チェック 設定したオブジェクトをドロップ")]
        [SerializeField]  public GameObject setDropObject;
        [Tooltip("ドロップする個数")]
        [SerializeField]  public int setDropCount;
        [Tooltip("ドロップする確率")]
        [SerializeField]  public int setDropPlibability;
    }
}
