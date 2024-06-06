using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Syuntoku.DigMode.Inventory.Juwelry;
using Syuntoku.DigMode.UI;
using Syuntoku.DigMode.Wave;

namespace Syuntoku.DigMode.Shop
{

    public class ShopCharactor : ActionObject
    {
        #region CashVariable
        [SerializeField] ShopCharactorScritable _shopCharactor;
        [SerializeField] Transform charactorTransform;
        #endregion
        ShopMerChanfdiseSystem _chanfdiseSystem;
        UIManage _uIManage;
        ParkConditionsManage _conditionsManage;
        JuwelryInventory _rerollCost;
        WaveManage _waveManage;
        const int DESTROY_WAVECOUNT = 2;
        const float TRACK_LENGTH = 3;
        const float SPEED = 1.0f;
        bool _bStartTimer;
        float _step;
        GameObject _playerObject;
        ShopData _shopData;
        uint _nowWavecount;
        public uint _buyFlagUp;
        public uint _buyFlagMiddle;
        public uint _buyFlagDown;

        enum CharactorState
        {
            ENABLE,
            NOTFIND,
            INVALID,
        }

        public enum SELECTFLAG
        {
            FIRST = 0x01,
            SECOND = 0x02,
            THIRD = 0x04,
            FORTH = 0x08,
            MAX = 5,
        }

        //=================================
        //Unity
        //=================================
        private void Start()
        {
            _playerObject = GameObject.Find("Player");
            _uIManage = GameObject.Find("UIManage").GetComponent<UIManage>();
            _conditionsManage = GameObject.Find("InventoryManage").GetComponent<ParkConditionsManage>();
            _chanfdiseSystem = GameObject.Find("ShopManage").GetComponent<ShopMerChanfdiseSystem>();
            _waveManage = GameObject.Find("WaveManage").GetComponent<WaveManage>();
            _shopData = _chanfdiseSystem.SetMerchandiseData();
            _rerollCost = new JuwelryInventory(_shopCharactor.RerollCost);
            _shopData.rerollCost = _rerollCost;
            DestoryCheck();
        }

        private void Update()
        {
            Vector3 length = _playerObject.transform.position - transform.position;
            if (length.sqrMagnitude >= TRACK_LENGTH * TRACK_LENGTH) return;
            length.y = 0;
            _step += SPEED * Time.deltaTime;
            charactorTransform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(length.normalized), _step);
        }
        //=================================
        //public
        //=================================
        public override void OnAction(UIManage uIManage)
        {
            SetCount();
            _uIManage.DrawShopUI(_shopData, this);

        }

        public void SetBuyFlag(int step, SELECTFLAG selectFlag)
        {
            switch (step)
            {
                case 0:
                    _buyFlagUp += (uint)selectFlag;
                    break;
                case 1:
                    _buyFlagMiddle += (uint)selectFlag;
                    break;
                case 2:
                    _buyFlagDown += (uint)selectFlag;
                    break;
            }
        }

        public void FlagReset()
        {
            _buyFlagUp = 0x00;
            _buyFlagMiddle = 0x00;
            _buyFlagDown = 0x00;
        }
        //=================================
        //private
        //=================================
        void SetCount()
        {
            _bStartTimer = true;
            _nowWavecount = _waveManage._waveCount;
        }

        void DestoryCheck()
        {
            if (!_bStartTimer) return;
            if (_nowWavecount + DESTROY_WAVECOUNT >= _conditionsManage.waveCount)
            {
                Destroy(gameObject);
            }
        }
    }
}
