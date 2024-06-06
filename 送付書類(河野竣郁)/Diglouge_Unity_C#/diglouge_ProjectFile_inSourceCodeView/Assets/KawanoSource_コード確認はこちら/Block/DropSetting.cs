using UnityEngine;
using Syuntoku.DigMode.Inventory.Juwelry;

namespace Syuntoku.DigMode
{
    [System.Serializable]
    public class DropSetting
    {
        [Header("�`�F�b�N��t����Ɛݒ肵���I�u�W�F�N�g���h���b�v")]
        [SerializeField] public bool bSetDropItem;

        [Header("SetDropItem: �� ��΂��h���b�v")]
        [SerializeField]  public JuwelryInventory.JUWELRY_KIND dropJuwelryKind;
        [Tooltip("�h���b�v�����")]
        [SerializeField]  public int dropCount;
        [Tooltip("�h���b�v����m��")]
        [SerializeField]  public int dropPlibability;

        [Header("SetDropItem: �`�F�b�N �ݒ肵���I�u�W�F�N�g���h���b�v")]
        [SerializeField]  public GameObject setDropObject;
        [Tooltip("�h���b�v�����")]
        [SerializeField]  public int setDropCount;
        [Tooltip("�h���b�v����m��")]
        [SerializeField]  public int setDropPlibability;
    }
}
