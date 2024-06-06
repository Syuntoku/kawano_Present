using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Syuntoku.DigMode.Tool;
using Syuntoku.DigMode.Player;

namespace Syuntoku.DigMode.UI
{
    public class UpgladeUI : BaseUI
    {
        #region　CashVariable
        [SerializeField] UpgladeButton[] _digButton;
        [SerializeField] UpgladeButton[] _battleButton;
        [SerializeField] GameObject _selectImege;
        [SerializeField] TMP_Text[] _textDig;
        [SerializeField] TMP_Text[] _textBattle;
        [SerializeField] Image[] _toolImage;
        [SerializeField] Image[] _weaponImage;
        [SerializeField] TMP_Text _toolPaseCountText;
        [SerializeField] TMP_Text _weaponPaseCountText;
        #endregion
        UIManage _uiManage;
        PlayerInventory _playerInventory;
        int _upGladeNum;
        const int NOTSELECT = 100;
        const int DIG_SELECT_START_ID = 0;
        const int BATTLE_SELECT_START_ID = 10;
        const string NOT_HAVE_TEXT = "NoData";
        const int ONE_DRAWSIZE = 5;
        bool _bHold;

        RectTransform _rectTransform;
        ToolInfo[] _selectToolinfoData;
        WeaponInfo[] _selectWeaponBase;

        int _toolPeseCount;
        int _weaponPeseCount;
        int _toolMaxPese;
        int _weaponMaxPese;

        public UpgladeUI()
        {
            _selectToolinfoData = new ToolInfo[ONE_DRAWSIZE];
            _selectWeaponBase = new WeaponInfo[ONE_DRAWSIZE];
        }

        //===============================================
        //Unity
        //===============================================
        private void Start()
        {
            _playerInventory = GameObject.Find("Player").GetComponent<Player.Player>().GetPlayerInventory();
            ChangeLevelText();
            _selectImege.SetActive(false);
            _bHold = true;
        }

        public void NutoralStatus()
        {
            GameObject.Find("Player").GetComponent<Player.Player>().NeutralHand();
        }

        private void Update()
        {
            if(Input.InputData._bMenu)
            {
                if (_bHold) return;
                _uiManage.OutUiMode(gameObject);
            }
            else
            {
                _bHold = false;
            }
        }

        //===============================================
        //public
        //===============================================
        override public void Initialize(UIManage uIManage)
        {
            _uiManage = uIManage;
            _uiManage.OnUiMode();
            _rectTransform = _selectImege.GetComponent<RectTransform>();
        }

        public void OnOut()
        {
            GameObject.Find("Player").GetComponent<Player.Player>().ChangeHandUpdate();
            _uiManage.OutUiMode(gameObject);
        }

        public void OnEnter(Vector3 rectPosition, int selectNum)
        {
            _selectImege.SetActive(true);
            _rectTransform.position = rectPosition + new Vector3(0.01f, -0.01f, 0.0f);
        }

        public void SetNum(int selectNum)
        {
            if (selectNum == NOTSELECT)
            {
                _selectImege.SetActive(false);
                _upGladeNum = selectNum;
            }
        }

        public void OnSelect(int selectNum)
        {
            //二回クリックした場合は強化画面に
            if (_upGladeNum == selectNum)
            {
                if (selectNum >= DIG_SELECT_START_ID && selectNum < BATTLE_SELECT_START_ID)
                {
                    if (_selectToolinfoData[selectNum] == null) return;
                    _uiManage.OutUiMode(gameObject);
                    _uiManage.DrawUpGrade2UI(_selectToolinfoData[selectNum]._toolUpgladeStatus);
                }
                else
                {
                    selectNum -= BATTLE_SELECT_START_ID;
                    if (_selectWeaponBase[selectNum] == null) return;
                    _uiManage.OutUiMode(gameObject);
                    _uiManage.DrawUpGrade2UI(_selectWeaponBase[selectNum]._weaponUpgrade);
                }
            }
            _upGladeNum = selectNum;
        }

        /// <summary>
        /// ツール枠　次のページに進む
        /// </summary>
        public void NextToolSelect()
        {
            if (_toolPeseCount >= _toolMaxPese) return;
            _toolPeseCount++;
            _toolPaseCountText.SetText(_toolPeseCount.ToString());
        }

        /// <summary>
        /// ツール枠　ページを戻す
        /// </summary>
        public void BackToolSelect()
        {
            if (_toolPeseCount <= 0) return;
            _toolPeseCount--;
            _toolPaseCountText.SetText(_toolPeseCount.ToString());
        }
        /// <summary>
        /// 武器枠　次のページに進む
        /// </summary>
        public void NextWeaponSelect()
        {
            if (_weaponPeseCount >= _weaponMaxPese) return;
            _weaponPeseCount++;
            _weaponPaseCountText.SetText(_weaponMaxPese.ToString());
        }

        /// <summary>
        /// 武器枠　ページを戻す
        /// </summary>
        public void BackWeaponSelect()
        {
            if (_weaponPeseCount <= 0) return;
            _weaponPeseCount--;
            _weaponPaseCountText.SetText(_weaponMaxPese.ToString());
        }

        //===============================================
        //private
        //===============================================
        void ChangeLevelText()
        {
            for (int i = 0; i < _textDig.Length; i++)
            {
                _textDig[i].text = NOT_HAVE_TEXT;
                _digButton[i].SetActive(false);
            }
            for (int i = 0; i < _textBattle.Length; i++)
            {
                _textBattle[i].text = NOT_HAVE_TEXT;
                _battleButton[i].SetActive(false);
            }

            List<ToolInfo> toolData = _playerInventory.GetToolInventory();

            //採掘装備のテキストをセット
            for (int i = _toolPeseCount * ONE_DRAWSIZE; i < ONE_DRAWSIZE; i++)
            {
                if (toolData.Count <= i)
                {
                    _selectToolinfoData[i] = null;
                    continue;
                }

                _selectToolinfoData[i] = toolData[i];
                _textDig[i].SetText(_selectToolinfoData[i]._toolStatus.toolName);
                _toolImage[i].sprite = _selectToolinfoData[i]._toolStatus.toolSquareIcon;
                _digButton[i].SetActive(true);
            }

            List<WeaponInfo> weaponData = _playerInventory.GetWeaponInventory();

            //戦闘装備のテキストをセット
            for (int i = _weaponPeseCount * ONE_DRAWSIZE; i < ONE_DRAWSIZE; i++)
            {
                if (weaponData.Count <= i)
                {
                    _selectWeaponBase[i] = null;
                    continue;
                }

                _selectWeaponBase[i] = weaponData[i];
                _textBattle[i].SetText(_selectWeaponBase[i]._weaponBaseStatus.weaponName);
                _weaponImage[i].sprite = _selectWeaponBase[i]._weaponBaseStatus.icon;
                _battleButton[i].SetActive(true);
            }

            //ページの最大数を設定
            int count = 1;
            while (true)
            {
                if (toolData.Count >= ONE_DRAWSIZE * count)
                {
                    _toolMaxPese++;
                    count++;
                }
                else
                {
                    break;
                }
            }

            while (true)
            {
                if (weaponData.Count >= ONE_DRAWSIZE * count)
                {
                    _toolMaxPese++;
                    count++;
                }
                else
                {
                    break;
                }
            }
        }
    }
}
