using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Syuntoku.DigMode.Inventory;
using Syuntoku.DigMode.ParkData;
using System;
using System.Collections.Generic;
using Syuntoku.Status;

namespace Syuntoku.DEBUG
{

    public class ParkDebugPresenter : MonoBehaviour
    {
        [SerializeField] private Button _applyButton;
        [SerializeField] private TMP_InputField _inputField;


        public LoadParkData _loadParkData;
        public InventoryManage _inventoryManage;

        private StatusManage _statusManage;

        private void Start()
        {
            _statusManage = GameObject.FindObjectOfType<StatusManage>();
            _inventoryManage = GameObject.FindObjectOfType<InventoryManage>();
            _loadParkData = GameObject.FindObjectOfType<LoadParkData>();


            info = _inventoryManage.info;
            _digmode = _statusManage.digmodeStatus;
            _battleMode = _statusManage.battleModeStatus;
        }

        [SerializeField] private TextMeshProUGUI[] _texts;
        private int _preCount;

        public List<Park> info;


        [SerializeField] private Transform _parkPanelroot;
        [SerializeField] private ParkView _parkViewPrefab;
        private List<ParkView> _views = new List<ParkView>();


        private void OnParkCountChanged()
        {
            foreach (var view in _views)
            {
                Destroy(view.gameObject);
            }

            _views.Clear();


            for (int i = 0; i < _inventoryManage.info.Count; i++)
            {
                var view = Instantiate(_parkViewPrefab, _parkPanelroot);
                view.Init(_inventoryManage.info[i]);
                _views.Add(view);
            }
        }

        public Digmode _digmode;
        public BattleMode _battleMode;
        [SerializeField] private int _parkID;
        public void AddPark()
        {
            if (_parkID == 0) return;

            var park = _loadParkData.FindPark(_parkID);
            _inventoryManage.AddPark(park);

            _parkID = 0;
        }
    }

}