using System.Collections.Generic;
using UnityEngine;
using System;
using Syuntoku.DigMode.Tool;
using Syuntoku.DigMode.Tool.Unique;
using Syuntoku.DigMode.ParkData;
using Syuntoku.Status;
using Syuntoku.DigMode.Settings;
using Syuntoku.DigMode.Tool.Scriptable;
using Syuntoku.DigMode.Inventory.Juwelry;

//===========================================
//プレイヤーのインベントリ管理
//===========================================

namespace Syuntoku.DigMode.Inventory
{

    [Serializable]
    public class InventoryManage : MonoBehaviour
    {
        #region CashVariables
        [SerializeField] ToolSetHand _toolSetHand;
        [SerializeField] StatusManage _statusManage;
        [SerializeField] LoadParkData _loadParkData;
        [SerializeField] ParkConditionsManage _conditionsManage;
        [SerializeField] GameSetting _gameSetting;
        [SerializeField] BlockManage _blockManage;
        [SerializeField] GameObject _playerObject;
        [SerializeField] ToolSetHand _toolSethand;
        [SerializeField] UniqueCharacteristicsScriptable _uniqueCharacteristicsScriptable;
        [SerializeField] PickAxeInfoScriptable _pickAxeInfoScriptable;
        [SerializeField] HammerInfoScriptable _hammerInfoScriptable;
        [SerializeField] GunInfoScriptable _gunInfoScriptable;
        [SerializeField] JuwelryScriptable _juwelryScriptable;
        [SerializeField] Transform _juwelryParent;
        #endregion
        public JuwelryInventory _juwelryInventoryData = new JuwelryInventory();
        /// <summary>
        /// 宝石所持時SE
        /// </summary>
        static AudioSource audioSource;
        /// <summary>
        /// 所持しているパークのデータ
        /// </summary>
        public Dictionary<int, Park> activeParks = new Dictionary<int, Park>();
        public List<Park> info = new List<Park>();

        public enum ToolKind
        {
            DigTools,
            Weapon,
        }

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            _juwelryInventoryData.Initialize(_juwelryScriptable);
            _statusManage.FullInitialize();
        }

        private void Update()
        {
            if (_gameSetting.bStopGameAction) return;

            if (activeParks.Count != 0)
            {
                ParkUpdate(_statusManage);
            };
        }

        #region JUWELRY


        /// <summary>
        /// 現在の宝石の情報を取得する
        /// </summary>
        /// <returns></returns>
        public JuwelryInventory NowInventoryData()
        {
            return _juwelryInventoryData;
        }

        /// <summary>
        /// 宝石を使います
        /// 戻り値　もらった数値が引けるなら（true）*trueの場合はもらった引数分使います　:
        ///         引けないなら（false)
        /// </summary>
        /// <param name="baseData"></param>
        public bool UseJuwelry(JuwelryInventory useAmount)
        {
            if (!_juwelryInventoryData.UseCheck(useAmount)) return false;

            _juwelryInventoryData -= useAmount;

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseData"></param>
        /// <returns></returns>
        public void AddJuwelry(JuwelryInventory baseData)
        {
            _juwelryInventoryData += baseData;
        }

        public bool Addjuwelry(JuwelryInventory.JUWELRY_KIND juweleyIndex, uint num)
        {
            return _juwelryInventoryData.AddJuwelryData(juweleyIndex, num);
        }

        #endregion

        #region SE
        public static void GetGemSE()
        {
            audioSource.Play();
        }
        #endregion

        #region Park

        /// <summary>
        /// 最初の更新
        /// </summary>
        public void ParkStart()
        {
            foreach (Park item in activeParks.Values)
            {
                item.levelParkData[item.nowLevel].Start(_statusManage, _conditionsManage);
            }
        }

        /// <summary>
        /// パークの道具を振る前の処理
        /// </summary>
        public void SwingUpdate()
        {
            foreach (Park item in activeParks.Values)
            {
                item.levelParkData[item.nowLevel].SwingUpdate(_statusManage, _conditionsManage);
            }
        }

        /// <summary>
        /// パークの道具を振った後の処理
        /// </summary>
        public void EndSwing()
        {
            foreach (Park item in activeParks.Values)
            {
                item.levelParkData[item.nowLevel].EndSwing(_statusManage);
            }
        }

        /// <summary>
        /// パークデータの更新
        /// </summary>
        void ParkUpdate(StatusManage status)
        {
            foreach (Park item in activeParks.Values)
            {
                item.levelParkData[item.nowLevel].Update(status, _conditionsManage, Time.deltaTime);
            }
        }

        /// <summary>
        /// ランダムなパークを一つ受け取る
        /// </summary>
        /// <returns>nullの場合　アクティブなパークがありません</returns>
        public Park GetRandomPark()
        {
            Park park = _loadParkData.GetRandomPark();

            if (park == null) return _loadParkData._notFindData;
            return park;
        }

        /// <summary>
        /// アクティブなパークにパークのデータを追加する
        /// </summary>
        /// <param name="park"></param>
        public void AddPark(Park park)
        {
            _loadParkData.KeyToEnablePark(park.parkNo, _statusManage, _conditionsManage);
            if (park.nowLevel == 0)
            {
                activeParks.Add(park.parkNo, park);
                info.Add(park);
            }
        }

        /// <summary>
        /// アクティブなパークの数を数える
        /// </summary>
        /// <param name="popKind"></param>
        /// <returns></returns>
        public int ParkActiveCount(int popKind)
        {
            return _loadParkData.PopUpCount(popKind);
        }

        public void OnExecuteMainSkill()
        {
            foreach (Park item in activeParks.Values)
            {
                item.levelParkData[item.nowLevel].OnExecuteMainSkill(_statusManage, _conditionsManage, Time.deltaTime);
            }
        }

        public void OnHitBlock()
        {
            foreach (Park item in activeParks.Values)
            {
                item.levelParkData[item.nowLevel].OnHitBlock(_statusManage, _conditionsManage, Time.deltaTime);
            }
        }

        public void BreakUpDate()
        {
            foreach (Park item in activeParks.Values)
            {
                item.levelParkData[item.nowLevel].BreakUpDate(_statusManage, _conditionsManage);
            }
        }

        #endregion

        public UniqueCharacteristics InstanceUniqueData(ToolGenerater.ToolName toolindex)
        {
            return UniqueCharacteristics.InstanceUniqueData(toolindex, _uniqueCharacteristicsScriptable, _conditionsManage, _statusManage);
        }
    }
}