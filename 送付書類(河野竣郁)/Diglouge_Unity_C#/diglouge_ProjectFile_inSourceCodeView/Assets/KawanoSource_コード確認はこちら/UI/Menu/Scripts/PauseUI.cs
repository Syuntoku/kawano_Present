using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Syuntoku.DigMode.UI;
using Syuntoku.DigMode.Player;
using TMPro;
using UnityEngine.UI;
using Syuntoku.DigMode.Tool;
using Syuntoku.DigMode.Inventory;
using Syuntoku.DigMode.Inventory.Juwelry;

namespace Syuntoku.DigMode.UI
{
    public class PauseUI : BaseUI
    {
        #region CashVarible
        [Header("共通データ")]
        [SerializeField] GameObject _iconSetPrf;
        [SerializeField] TMP_Text[] _fillterText;
        [SerializeField] Image _activeImage;
        [SerializeField] GameObject _digFillter;
        [SerializeField] GameObject _parkFilter;
        [SerializeField] GameObject _resourceFillter;
        [SerializeField] Color _textNormalColor;
        [SerializeField] Color _textActiveColor;
        [SerializeField] GameObject _settingUi;

        [Header("採掘・戦闘画面")]
        [SerializeField] Sprite _defaultSprite;
        [SerializeField] Image[] _drawIcon;
        [SerializeField] GameObject _rightArrow;
        [SerializeField] GameObject _leftArrow;
        [SerializeField] DetailManagerInfo _detailManagerInfo;
        [SerializeField] RectTransform _selectObjectTrans;
        [SerializeField] GameObject _selectObject;
        [SerializeField] GameObject _defaultDetail;
        [SerializeField] GameObject _infoObject;
        [SerializeField] GameObject _equipmentMode;
        [SerializeField] GameObject _changeEquipmentFirst;
        [SerializeField] GameObject _changeEquipmentSecond;
        [SerializeField] GameObject _nowEquipmentObjectFirst;
        [SerializeField] GameObject _nowEquipmentObjectSecond;
        [SerializeField] GameObject _euipmentButton;

        [Header("パーク画面")]
        [SerializeField] GameObject _digScrollContents;
        [SerializeField] GameObject _battleScrollContents;
        [Header("インベントリ画面")]
        [SerializeField] TMP_Text _weight;
        [SerializeField] TMP_Text _maxWeight;
        [SerializeField] GameObject _infoContent;
        [SerializeField] GameObject _infoPrf;
        [SerializeField] Image _weightLine;
        [SerializeField] Image _weightBag;
        [SerializeField] Image _weightIcon;
        [SerializeField] Sprite _defaultWeightLine;
        [SerializeField] Sprite _defaultWeightBag;
        [SerializeField] Sprite _defaultWeightIcon;
        [SerializeField] Sprite _overWeightLine;
        [SerializeField] Sprite _overWeightBag;
        [SerializeField] Sprite _overWeightIcon;

        [SerializeField] JuwelryCounter _juwelryCounter;
        #endregion

        Player.Player _player;
        PlayerInventory _playerInventory;
        InventoryManage _inventoryManage;
        const int NEXTPAGE_COUNT = 6;
        Vector2 AJUST_TRANS_SELECT_OBJECT = new Vector3(5f, -5f);
        readonly Vector3 TOOL_SELECT_SIZE = new Vector3(1.52f, 1.50f, 1.52f);
        int _changeId;
        bool _bEquipmentBattle;
        bool _bHold;

        //採掘・戦闘
        int _drawPageCount;
        bool bDigfilter;
        const int LOWWEST_INFO_COUNT = 10;
        
        public enum SelectState
        {
            FIRST,
            SECOND,
            CANSEL,
        }
        bool _bEquipmentMode;

        public enum Fillter
        {
            DIG,
            BATTLE,
            PARK,
            RESOURCE,
        }

        UIManage _uIManage;

        //====================================
        //Unity
        //====================================
        private void Start()
        {
            _player = GameObject.Find("Player").GetComponent<Player.Player>();
            _playerInventory = _player.GetPlayerInventory();
            _inventoryManage = GameObject.Find("InventoryManage").GetComponent<InventoryManage>();
            ChangeFilter(Fillter.DIG);
            _bHold = true;
            //手持ちのステータス変更をなくす
            _player.NeutralHand();
        }

        private void Update()
        {
            if(Input.InputData._bMenu)
            {
                if (_bHold) return;

                if(_bEquipmentMode)
                {
                    SelectChangeState((int)SelectState.CANSEL);
                    _bHold = true;
                    return;
                }

                BackUI();
            }
            else
            {
                _bHold = false;
            }

            _juwelryCounter.JuwelryCountTextUpdate(_playerInventory.GetjuwelryInventory());
        }

        //=================================
        //public
        //=================================
        public override void Initialize(UIManage uIManage)
        {
            _uIManage = uIManage;
        }

        public void ChangeFilter(Fillter fillter)
        {
            _digFillter.SetActive(false);
            _parkFilter.SetActive(false);
            _resourceFillter.SetActive(false);
            _selectObject.SetActive(false);
            switch (fillter)
            {
                case Fillter.DIG:
                    _digFillter.SetActive(true);
                    ToolFillterSet();
                    _selectObject.SetActive(true);
                    _selectObjectTrans.transform.localScale = TOOL_SELECT_SIZE;
                    bDigfilter = true;
                    break;
                case Fillter.BATTLE:
                    _digFillter.SetActive(true);
                    WeaponFillterSet();
                    _selectObject.SetActive(true);
                    _selectObjectTrans.transform.localScale = TOOL_SELECT_SIZE;
                    bDigfilter = false;
                    break;
                case Fillter.PARK:
                    _parkFilter.SetActive(true);
                    SetIconData();
                    _selectObject.SetActive(false);
                    _selectObjectTrans.transform.localScale = Vector3.one;
                    break;
                case Fillter.RESOURCE:
                    _resourceFillter.SetActive(true);
                    SetInventoryData();
                    break;
            }

            //選択されている時の色変化の画像の場所を変更
            _activeImage.rectTransform.anchoredPosition = _fillterText[(int)fillter].rectTransform.anchoredPosition;

            //フィルターのテキストの色を暗くする
            foreach (TMP_Text item in _fillterText)
            {
                item.color = _textNormalColor;
            }

            _fillterText[(int)fillter].color = _textActiveColor;

            _selectObject.SetActive(false);
        }
        public void ChangeFilter(int fillter)
        {
            _digFillter.SetActive(false);
            _parkFilter.SetActive(false);
            _resourceFillter.SetActive(false);
            switch (fillter)
            {
                case (int)Fillter.DIG:
                    _digFillter.SetActive(true);
                    ToolFillterSet();
                    _selectObject.SetActive(true);
                    _selectObjectTrans.transform.localScale = TOOL_SELECT_SIZE;
                    bDigfilter = true;
                    break;
                case (int)Fillter.BATTLE:
                    _digFillter.SetActive(true);
                    WeaponFillterSet();
                    _selectObject.SetActive(true);
                    _selectObjectTrans.transform.localScale = TOOL_SELECT_SIZE;
                    bDigfilter = false;
                    break;
                case (int)Fillter.PARK:
                    _parkFilter.SetActive(true);
                    SetIconData();
                    _selectObject.SetActive(false);
                    _selectObjectTrans.transform.localScale = Vector3.one;
                    break;
                case (int)Fillter.RESOURCE:
                    _resourceFillter.SetActive(true);
                    _selectObject.SetActive(false);
                    SetInventoryData();
                    break;
            }

            //選択されている時の色変化の画像の場所を変更
            _activeImage.rectTransform.anchoredPosition = _fillterText[(int)fillter].rectTransform.anchoredPosition;

            //フィルターのテキストの色を暗くする
            foreach (TMP_Text item in _fillterText)
            {
                item.color = _textNormalColor;
            }

            _fillterText[(int)fillter].color = _textActiveColor;
        }

        public void SelectImageTransformChange(RectTransform rectTransform)
        {
            _activeImage.rectTransform.anchoredPosition = rectTransform.position;
        }

        public void ActiveSettingUI(bool active)
        {
            _settingUi.SetActive(active);
        }

        //========================================
        //道具・武器画面
        //========================================
        void ToolFillterSet()
        {
            List<ToolInfo> workToolData = _playerInventory.GetToolInventory();
            _rightArrow.SetActive(false);
            _leftArrow.SetActive(false);

            foreach (Image item in _drawIcon)
            {
                item.sprite = _defaultSprite;
            }

            if (workToolData.Count >= NEXTPAGE_COUNT)
            {
                _rightArrow.SetActive(true);
            }

            SetIconDraw(workToolData, 0);
        }
        void WeaponFillterSet()
        {
            List<WeaponInfo> weaponInfos = _playerInventory.GetWeaponInventory();
            _rightArrow.SetActive(false);
            _leftArrow.SetActive(false);

            foreach (Image item in _drawIcon)
            {
                item.sprite = _defaultSprite;
            }

            if (weaponInfos.Count >= NEXTPAGE_COUNT)
            {
                _rightArrow.SetActive(true);
            }

            SetIconDraw(weaponInfos, 0);
        }

        /// <summary>
        /// Iconを設定する
        /// </summary>
        /// <param name="workToolData"></param>
        /// <param name="startindex"></param>
        void SetIconDraw(List<ToolInfo> workToolData, int startindex)
        {
            //６枠ツールのデータを設定する
            for (int i = startindex; i < startindex + NEXTPAGE_COUNT; i++)
            {
                if (i >= workToolData.Count) continue;
                ToolInfo toolData = workToolData[i];
                _drawIcon[i].sprite = toolData._toolStatus.toolSquareIcon;
                _drawIcon[i].transform.GetChild(1).gameObject.SetActive(false);

                if (toolData._isEquipmet)
                {
                    _drawIcon[i].transform.GetChild(1).gameObject.SetActive(true);
                }

                _detailManagerInfo.SetToolData(toolData);
            }

            _rightArrow.SetActive(false);
            _leftArrow.SetActive(false);

            if (workToolData.Count >= startindex + NEXTPAGE_COUNT)
            {
                _rightArrow.SetActive(true);
            }

            if (_drawPageCount != 0)
            {
                _leftArrow.SetActive(true);
            }
        }
        /// <summary>
        /// Iconを設定する
        /// </summary>
        /// <param name="workToolData"></param>
        /// <param name="startindex"></param>
        void SetIconDraw(List<WeaponInfo> workWeaponData, int startindex)
        {
            //６枠ツールのデータを設定する
            for (int i = startindex; i < startindex + NEXTPAGE_COUNT; i++)
            {
                if (i >= workWeaponData.Count) continue;
                WeaponInfo toolData = workWeaponData[i];
                _drawIcon[i].sprite = toolData._weaponBaseStatus.icon;
                _drawIcon[i].transform.GetChild(1).gameObject.SetActive(false);
                if (toolData._bEquipment)
                {
                    _drawIcon[i].transform.GetChild(1).gameObject.SetActive(true);
                }
                _detailManagerInfo.SetWeaponData(toolData);
            }

            _rightArrow.SetActive(false);
            _leftArrow.SetActive(false);

            if (workWeaponData.Count >= startindex + NEXTPAGE_COUNT)
            {
                _rightArrow.SetActive(true);
            }

            if (_drawPageCount != 0)
            {
                _leftArrow.SetActive(true);
            }
        }

        public void NextPase()
        {
            _drawPageCount++;
        }

        public void BackPase()
        {
            if (_drawPageCount <= 0) return;
            _drawPageCount++;
            List<ToolInfo> workToolData = _playerInventory.GetToolInventory();
            SetIconDraw(workToolData, _drawPageCount * NEXTPAGE_COUNT);
        }

        public void BackUI()
        {
            _player.UpdateToolEquipment();
            _player.EquipmentUpdate();
            _player.ChangeHandUpdate();
            _uIManage.OutUiMode(gameObject);
        }
        //=========================================
        //パークの画面
        //=========================================
        void SetIconData()
        {
            foreach (Transform item in _digScrollContents.transform)
            {
                foreach (Transform icons in item.transform)
                {
                    icons.GetComponent<ParkIconButton>().SetPauseUiData(this);
                }
            }
            foreach (Transform item in _battleScrollContents.transform)
            {
                foreach (Transform icons in item.transform)
                {
                    icons.GetComponent<ParkIconButton>().SetPauseUiData(this);
                }
            }
            List<ParkData.Park> parkList = new List<ParkData.Park>(_inventoryManage.activeParks.Values);

            if (parkList.Count == 0) return;

            int count = 0;
            foreach (Transform item in _digScrollContents.transform)
            {
                foreach (Transform icons in item.transform)
                {
                    if (count >= parkList.Count) return;
                    count++;
                    icons.GetComponent<ParkIconButton>().SetParkData(parkList[count]);
                }
            }
            foreach (Transform item in _battleScrollContents.transform)
            {
                foreach (Transform icons in item.transform)
                {
                    if (count >= parkList.Count) return;
                    count++;
                    icons.GetComponent<ParkIconButton>().SetParkData(parkList[count]);
                }
            }
        }
        //=========================================
        //インベントリ
        //=========================================
        void SetInventoryData()
        {
            _weight.SetText(_playerInventory.WeightCheck().ToString());
            _maxWeight.SetText(_playerInventory.GetMaxWeight().ToString());

            if(_playerInventory.WeightCheck() >= _playerInventory.GetMaxWeight())
            {
                _weightBag.sprite = _overWeightBag;
                _weightLine.sprite = _overWeightLine;
                _weightIcon.sprite = _overWeightIcon;
            }
            else
            {
                _weightBag.sprite = _defaultWeightBag;
                _weightLine.sprite = _defaultWeightLine;
                _weightIcon.sprite = _defaultWeightIcon;
            }

            int objectCount = 0;
            objectCount += (int)JuwelryInventory.JUWELRY_KIND.JUWELRY_MAX;
            objectCount += _playerInventory.GetToolInventory().Count;
            objectCount += _playerInventory.GetWeaponInventory().Count;
            if(objectCount <= LOWWEST_INFO_COUNT) { objectCount = LOWWEST_INFO_COUNT; }

            //フィールドの枠が表示する数より低い場合は枠をリストに追加する
            if(_infoContent.transform.childCount * 2 <= objectCount)
            {
                //枠の生成は2つで一個のため子供のが数を二倍にする
                int reftInfoCount = objectCount - _infoContent.transform.childCount * 2;

                for (int i = 0; i < reftInfoCount; i++)
                {
                    Instantiate(_infoPrf, _infoContent.transform);
                    objectCount++;
                    objectCount++;
                }
            }

            int count = 0;
            GameObject[] infoObject = new GameObject[objectCount];

            foreach (Transform item in _infoContent.transform)
            {
                foreach (Transform icon in item.transform)
                {
                    infoObject[count] = icon.gameObject;
                    count++;
                }
            }

            //データを設定する
            int countIndex = 0;     
            countIndex = AddJuwelryInfo(infoObject, countIndex);
            countIndex = AddToolInfo(infoObject, countIndex);
            //後々フィルター機能を設定するため無駄な戻り値を置いておく
            countIndex = AddWeaponInfo(infoObject, countIndex);
        }

        /// <summary>
        /// 宝石のデータを枠に記載する
        /// </summary>
        /// <param name="gameObjects"></param>
        int AddJuwelryInfo(GameObject[] gameObjects ,int nextIndex)
        {
            JuwelryInventory juwelryInventory = _playerInventory.GetjuwelryInventory();
            Sprite[] icons = juwelryInventory.GetIconDataFull();

            for (int i = 0; i < (int)JuwelryInventory.JUWELRY_KIND.JUWELRY_MAX; i++)
            {
                JuwelryIconButton juwelryData = gameObjects[i].GetComponent<JuwelryIconButton>();
                if (juwelryData == null) continue;
                juwelryData.SetIcon(icons[i]);
                juwelryData.SetTextCount((int)juwelryInventory.GetjuwelryData(i), juwelryInventory.GetJuwelryWeight((JuwelryInventory.JUWELRY_KIND)i));
            }
            return nextIndex + (int)JuwelryInventory.JUWELRY_KIND.JUWELRY_MAX -1;
        }

        /// <summary>
        /// 武器のデータを枠に記載する
        /// </summary>
        /// <param name="gameObjects"></param>
        /// <param name="nextIndex"></param>
        /// <returns></returns>
        int AddWeaponInfo(GameObject[] gameObjects, int nextIndex)
        {
            List<WeaponInfo> weaponInfos = _playerInventory.GetWeaponInventory();

            foreach (WeaponInfo item in weaponInfos)
            {
                nextIndex++;
                JuwelryIconButton juwelryData = gameObjects[nextIndex].GetComponent<JuwelryIconButton>();
                if (juwelryData == null) continue;
                juwelryData.SetIcon(item._weaponBaseStatus.icon);
                juwelryData.SetTextCount(1, item._weaponBaseStatus.weight);
            }
            return nextIndex;
        }

        /// <summary>
        /// 道具のデータを枠に記載する
        /// </summary>
        /// <param name="gameObjects"></param>
        /// <param name="nextIndex"></param>
        /// <returns></returns>
        int AddToolInfo(GameObject[] gameObjects, int nextIndex)
        {
            List<ToolInfo> toolInfos = _playerInventory.GetToolInventory();

            foreach (ToolInfo item in toolInfos)
            {
                nextIndex++;
                JuwelryIconButton juwelryData = gameObjects[nextIndex].GetComponent<JuwelryIconButton>();
                if (juwelryData == null) continue;
                juwelryData.SetIcon(item._toolStatus.toolSquareIcon);
                juwelryData.SetTextCount(1, item._toolStatus.weight);
            }
            return nextIndex;
        }

        //=========================================
        //選択アイコン
        //=========================================
        public void SetSelectTransform(RectTransform rectTransform)
        {
            _selectObjectTrans.anchoredPosition = rectTransform.anchoredPosition + AJUST_TRANS_SELECT_OBJECT;
        }

        public void SetSelectIconActive(bool active)
        {
            _selectObject.SetActive(active);
        }

        public void EnterButtonID(int id)
        {
            _selectObject.SetActive(true);

            if (bDigfilter)
            {
                List<ToolInfo> workToolData = _playerInventory.GetToolInventory();
                if(workToolData.Count <= id + _drawPageCount * NEXTPAGE_COUNT)
                {
                    _detailManagerInfo.ResetText();
                    return;
                }
                _detailManagerInfo.SetToolData(workToolData[id + _drawPageCount * NEXTPAGE_COUNT]);
            }
            else
            {
                List<WeaponInfo> weaponInfos = _playerInventory.GetWeaponInventory();
                if (weaponInfos.Count <= id + _drawPageCount * NEXTPAGE_COUNT)
                {
                    _detailManagerInfo.ResetText();
                    return;
                }
                _detailManagerInfo.SetWeaponData(weaponInfos[id + _drawPageCount * NEXTPAGE_COUNT]);
            }
        }

        public void SelectButton(RectTransform rectTransform, int id)
        {
            _infoObject.SetActive(true);
            _infoObject.GetComponent<RectTransform>().anchoredPosition = rectTransform.anchoredPosition;
            _infoObject.GetComponent<InfoManage>().SetData(this, bDigfilter, id);

            //装備中の道具は装備変更ボタンを無効にする
            if (bDigfilter)
            {
                if (id == _playerInventory.GetToolInventory().Count) return;
                ToolInfo tooldata = _playerInventory.GetToolInventory()[id];
                if (tooldata._isEquipmet)
                {
                    Image image = _infoObject.transform.GetChild(1).GetComponent<Image>();
                    UnityEngine.UI.Button button = _infoObject.transform.GetChild(1).GetComponent<UnityEngine.UI.Button>();
                    button.enabled = false;
                    image.color = Color.red;
                    image.transform.GetChild(0).GetComponent<TMP_Text>().SetText("装備中");
                }
                else
                {
                    Image image = _infoObject.transform.GetChild(1).GetComponent<Image>();
                    UnityEngine.UI.Button button = _infoObject.transform.GetChild(1).GetComponent<UnityEngine.UI.Button>();
                    button.enabled = true;
                    image.color = Color.white;
                    image.transform.GetChild(0).GetComponent<TMP_Text>().SetText("装備");
                }
            }
            else
            {
                WeaponInfo weaponInfo = _playerInventory.GetWeaponInventory()[id];
                if (weaponInfo._bEquipment)
                {
                    Image image = _infoObject.transform.GetChild(1).GetComponent<Image>();
                    UnityEngine.UI.Button button = _infoObject.transform.GetChild(1).GetComponent<UnityEngine.UI.Button>();
                    button.enabled = false;
                    image.color = Color.red;
                    image.transform.GetComponent<TMP_Text>().SetText("装備中");
                }
                else
                {
                    Image image = _infoObject.transform.GetChild(1).GetComponent<Image>();
                    UnityEngine.UI.Button button = _infoObject.transform.GetChild(1).GetComponent<UnityEngine.UI.Button>();
                    button.enabled = true;
                    image.color = Color.white;
                    image.transform.GetChild(0).GetComponent<TMP_Text>().SetText("装備");
                }
            }
        }

        public void DropItem(InfoManage infoManage)
        {
        }

        public void ChangeFillterEquipmentMode(int id)
        {
            if(bDigfilter)
            {
                ChangeDigFillterEquipmentMode(id);
            }
            else
            {
                ChangeBattleFillterEquipmentMode(id);
            }
        }

        /// <summary>
        /// ツールの装備変更
        /// </summary>
        /// <param name="id"></param>
        void ChangeDigFillterEquipmentMode(int id)
        {
            _defaultDetail.SetActive(false);
            _euipmentButton.SetActive(true);
            _equipmentMode.SetActive(true);
            _nowEquipmentObjectFirst.SetActive(true);
            _nowEquipmentObjectSecond.SetActive(true);
            //現在の装備欄
            _nowEquipmentObjectFirst.GetComponent<DetailManagerInfo>().SetToolData(_playerInventory.GetEquipmentTools()[0]);
            _nowEquipmentObjectSecond.GetComponent<DetailManagerInfo>().SetToolData(_playerInventory.GetEquipmentTools()[1]);
            //変更後の装備
            _changeEquipmentFirst.transform.GetChild(1).GetComponent<DetailManagerInfo>().SetToolData(_playerInventory.GetEquipmentTools()[0]);
            _changeEquipmentSecond.transform.GetChild(1).GetComponent<DetailManagerInfo>().SetToolData(_playerInventory.GetEquipmentTools()[1]);
            _changeEquipmentFirst.transform.GetChild(0).GetComponent<DetailManagerInfo>().SetToolData(_playerInventory.GetToolInventory()[id]);
            _changeEquipmentSecond.transform.GetChild(0).GetComponent<DetailManagerInfo>().SetToolData(_playerInventory.GetToolInventory()[id]);
            _bEquipmentMode = true;
            _changeId = id;
        }


        /// <summary>
        /// 武器の装備変更
        /// </summary>
        /// <param name="id"></param>
        void ChangeBattleFillterEquipmentMode(int id)
        {
            _defaultDetail.SetActive(false);
            _euipmentButton.SetActive(true);
            _equipmentMode.SetActive(true);
            _nowEquipmentObjectFirst.SetActive(true);
            _nowEquipmentObjectSecond.SetActive(true);
            //現在の装備欄
            _nowEquipmentObjectFirst.GetComponent<DetailManagerInfo>().SetWeaponData(_playerInventory.GetEquipmentWeapons()[0]);
            _nowEquipmentObjectSecond.GetComponent<DetailManagerInfo>().SetWeaponData(_playerInventory.GetEquipmentWeapons()[1]);
            //変更後の装備
            _changeEquipmentFirst.transform.GetChild(1).GetComponent<DetailManagerInfo>().SetWeaponData(_playerInventory.GetEquipmentWeapons()[0]);
            _changeEquipmentSecond.transform.GetChild(1).GetComponent<DetailManagerInfo>().SetWeaponData(_playerInventory.GetEquipmentWeapons()[1]);
            _changeEquipmentFirst.transform.GetChild(0).GetComponent<DetailManagerInfo>().SetWeaponData(_playerInventory.GetWeaponInventory()[id]);
            _changeEquipmentSecond.transform.GetChild(0).GetComponent<DetailManagerInfo>().SetWeaponData(_playerInventory.GetWeaponInventory()[id]);
            _bEquipmentMode = true;
            _changeId = id;
            _bEquipmentBattle = true;
        }

        public void SelectChangeState(int num)
        {
            if (!_bEquipmentMode) return;
            switch ((SelectState)num)
            {
                case SelectState.FIRST:
                    _changeEquipmentFirst.SetActive(true);
                    _changeEquipmentSecond.SetActive(false);
                    _nowEquipmentObjectFirst.SetActive(false);
                    _nowEquipmentObjectSecond.SetActive(true);
                    break;
                case SelectState.SECOND:
                    _changeEquipmentFirst.SetActive(false);
                    _changeEquipmentSecond.SetActive(true);
                    _nowEquipmentObjectFirst.SetActive(true);
                    _nowEquipmentObjectSecond.SetActive(false);
                    break;
                case SelectState.CANSEL:
                    _defaultDetail.SetActive(true);
                    _euipmentButton.SetActive(false);
                    _infoObject.SetActive(false);
                    _changeEquipmentFirst.SetActive(false);
                    _changeEquipmentSecond.SetActive(false);
                    _nowEquipmentObjectFirst.SetActive(false);
                    _nowEquipmentObjectSecond.SetActive(false);
                    _bEquipmentMode = false;
                    break;
                default:
                    break;
            }
        }

        public void FirstEquipment()
        {
            if (!_bEquipmentMode) return;
            if (_bEquipmentBattle)
            {
                _euipmentButton.SetActive(false);
                WeaponInfo weaponInfo = _playerInventory.GetWeaponInventory()[_changeId];
                _playerInventory.ChangeEquipmentWeapon(weaponInfo, 1);
                SelectChangeState((int)SelectState.CANSEL);
                WeaponFillterSet();
                return;
            }
            _euipmentButton.SetActive(false);
            ToolInfo toolInfo = _playerInventory.GetToolInventory()[_changeId];
            _playerInventory.ChangeEquipmentTool(toolInfo, 0);
            SelectChangeState((int)SelectState.CANSEL);
            ToolFillterSet();
        }

        public void SecondEquipment()
        {
            if (!_bEquipmentMode) return;
            if(_bEquipmentBattle)
            {
                _euipmentButton.SetActive(false);
                WeaponInfo weaponInfo = _playerInventory.GetWeaponInventory()[_changeId];
                _playerInventory.ChangeEquipmentWeapon(weaponInfo, 1);
                SelectChangeState((int)SelectState.CANSEL);
                WeaponFillterSet();
                return;
            }

            _euipmentButton.SetActive(false);
            ToolInfo toolInfo = _playerInventory.GetToolInventory()[_changeId];
            _playerInventory.ChangeEquipmentTool(toolInfo, 1);
            SelectChangeState((int)SelectState.CANSEL);
            ToolFillterSet();
        }
    }
}
