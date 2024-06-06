using UnityEngine;
using Syuntoku.DigMode.Inventory.Juwelry;
using DG.Tweening;
using Syuntoku.DigMode.Drop;

namespace Syuntoku.DigMode.Item
{
    /// <summary>
    /// �A�C�e�����擾
    /// </summary>
    public class GemItem : DropItem
    {
        [SerializeField] JuwelryScriptable _juwelryScriptable;
        public JuwelryInventory.JUWELRY_KIND _itemId;
        Player.PlayerInventory _playerInventory;
        public bool _bDisable;

        public void Start()
        {
            _playerInventory = GameObject.Find(Player.Player.PLAYER_TAG).GetComponent<Player.Player>().GetPlayerInventory();
        }

        public override void GetItem()
        {
            transform.DOKill();
            Destroy(gameObject);
            //��΃f�[�^���X�V
            _playerInventory.AddItem(_itemId);
            base.GetItem();
            BlockManage.InstanceAudio(_juwelryScriptable.GetSE(_itemId), transform.position);
        }
    }
}