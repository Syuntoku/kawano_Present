using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Syuntoku.DigMode.Settings;
using Syuntoku.Status;
using Syuntoku.DigMode.ParkData;
using TMPro;
using DG.Tweening;

namespace Syuntoku.DigMode.UI
{
    public class UI_ParkManage : MonoBehaviour
    {
        #region VARIABLE
        const int MAXPARKCOUNT = 3;

        GameSetting _gameSetting;
        [SerializeField] GameObject[] _iconObjectPoint;
        [SerializeField] ParkIconObjectScritable _parkIcon;
        GameObject[] genrateObject = new GameObject[3];
        [SerializeField] TMP_Text _explainText;
        [SerializeField] GameObject[] outlineObject;
        [SerializeField] GameObject _handObject;

        UIManage _uIManage;
        DigCount _digCount;

        Inventory.InventoryManage _inventoryManage;
        [SerializeField] Material _defaultColor;
        [SerializeField] Material _selectColor;

        Park[] _parks = new Park[3];
        #endregion

        enum SelectNum
        {
            NONE,
            ONE,
            TWO,
            THREE,
        }
        public enum POPMODE
        {
            DIG,
            BATTLE,
        }

        public int _select;
        public int _selected = 0;

        bool _bSelect;
        float _holoGlamInterval;

        bool _bTimerStart = false;
        bool _bSelected = false;
        float timer;
        bool bHold;

        [SerializeField] GameObject _holoGlam;
        [SerializeField] GameObject _canvas;

        const float HOLOGLAM_ACTIVE = 0.4f;
        const float CANVAL_ACTIVE = 0.8f;
        const float SELECT_ACTIVE = 1.5f;

        public void Initialize(GameSetting gameSetting, DigCount digCount)
        {
            _holoGlam.SetActive(false);

            _gameSetting = gameSetting;
            _digCount = digCount;

            _inventoryManage = GameObject.Find("InventoryManage").GetComponent<Inventory.InventoryManage>();

            _holoGlamInterval = 0.2f;
            bHold = Input.InputData._bGetPark;
            _select = 0;
            Selected();
            Reroll();
            SetSelect(0);

            for (int i = 0; i < MAXPARKCOUNT; i++)
            {
                _iconObjectPoint[i].layer = LayerMask.NameToLayer("UI");
                outlineObject[i].layer = LayerMask.NameToLayer("UI");
            }
        }

        private void Start()
        {
            _uIManage = GameObject.Find("UIManage").GetComponent<UIManage>();
        }

        /// <summary>
        /// 出現するパークを設定
        /// </summary>
        public void Reroll()
        {
            if (genrateObject[0] != null) Destroy(genrateObject[0]);
            if (genrateObject[1] != null) Destroy(genrateObject[1]);
            if (genrateObject[2] != null) Destroy(genrateObject[2]);

            for (int i = 0; i < _parks.Length; i++)
            {
                _parks[i] = GetUniqueRandomPark(_parks, i);
            }

            //パークの取得候補をインベントリから受け取る
            GenerateIcon();
        }

        // 被ってないパークを返す
        private Park GetUniqueRandomPark(Park[] currentParks, int currentIndex)
        {
            Park park;
            do
            {
                park = _inventoryManage.GetRandomPark();
            } while (IsDuplicate(park, currentParks, currentIndex));

            return park;
        }

        // 被ってないかを判定
        private bool IsDuplicate(Park parkToCheck, Park[] parks, int upToIndex)
        {
            for (int i = 0; i < upToIndex; i++)
            {
                if (parks[i] == parkToCheck)
                    return true;
            }
            return false;
        }

        void Update()
        {
            if (bHold && !Input.InputData._bGetPark)
            {
                bHold = false;
            };

            if (Input.InputData._bGetPark && _bSelected && !bHold)
            {
                Back();
            }

            if (_bSelected) return;

            timer += Time.deltaTime;
            if (timer >= HOLOGLAM_ACTIVE)
            {
                //ホログラムが点滅する
                if (timer % _holoGlamInterval <= 0.01f)
                {
                    _holoGlamInterval /= 2;
                    _holoGlam.SetActive(!_holoGlam.activeSelf);
                }
            }

            if (timer >= CANVAL_ACTIVE && _bTimerStart)
            {
                //パークの画面を出現
                _canvas.SetActive(true);
                _canvas.transform.localScale = Vector3.right;
                _canvas.transform.DOScale(Vector3.one, 0.4f);
                _holoGlam.SetActive(true);
                _bTimerStart = false;
            }

            if (timer >= SELECT_ACTIVE)
            {
                _bSelected = true;
            }

        }

        public void Selected()
        {
            _selected = _select;

            for (int i = 0; i < MAXPARKCOUNT; i++)
            {
                _iconObjectPoint[i].layer = LayerMask.NameToLayer("UI");
                outlineObject[i].layer = LayerMask.NameToLayer("UI");
                outlineObject[i].SetActive(false);
                _iconObjectPoint[i].GetComponent<MeshRenderer>().materials[1] = _defaultColor;
            }
            //選択されているオブジェクトにアウトラインを付ける
            _iconObjectPoint[_selected].layer = LayerMask.NameToLayer("Outline");
            outlineObject[_selected].layer = LayerMask.NameToLayer("Outline");
            _iconObjectPoint[_selected].GetComponent<MeshRenderer>().materials[1] = _selectColor;

            outlineObject[_selected].SetActive(true);
        }
        void GenerateIcon()
        {
            if (genrateObject[0] != null) Destroy(genrateObject[0]);
            if (genrateObject[1] != null) Destroy(genrateObject[1]);
            if (genrateObject[2] != null) Destroy(genrateObject[2]);

            //パークの取得候補をインベントリから受け取る
            for (int i = 0; i < _parks.Length; i++)
            {
                int random = Random.Range(0, 9);
                //アイコンを生成
                GameObject generateObject = _parkIcon.parkIconData[random].IconObject;
                genrateObject[i] = Instantiate(generateObject, _iconObjectPoint[i].transform.position + new Vector3(0.0f, 0.0f, -0.2f), Quaternion.Euler(_iconObjectPoint[i].transform.forward), _iconObjectPoint[i].transform);
                genrateObject[i].transform.localScale = new Vector3(0.23f, 0.19f, 0.27f);
                genrateObject[i].layer = LayerMask.NameToLayer("UI");

                Material _material = null;
                switch (_parks[i].nowLevel + 1)
                {
                    case 0:
                        _material = _parkIcon.level1Material;
                        break;
                    case 1:
                        _material = _parkIcon.level2Material;
                        break;
                    case 2:
                        _material = _parkIcon.level3Material;
                        break;
                }
                genrateObject[i].GetComponent<MeshRenderer>().material = _material;
            }
        }

        public void SetSelect(int num)
        {
            _select = num;
            ChangeText();
        }

        public void GetPark()
        {
            if (_parks[_selected] == null) return;

            _inventoryManage.AddPark(_parks[_select]);

            DigCount._parkCounter--;

            if (DigCount._parkCounter < 0)
            {
                DigCount._parkCounter = 0;
            }

            if (DigCount._parkCounter == 0)
            {
                Back();
            }
            else
            {
                Reroll();
            }
        }

        /// <summary>
        /// 画面を消す
        /// </summary>
        public void Back()
        {
            _handObject.transform.DOMove(_handObject.transform.position + new Vector3(0.0f, -2.0f, 0.0f), 0.2f);
            _canvas.transform.DOScale(Vector3.right, 0.1f).SetLink(gameObject);
            _uIManage.OutUiMode(gameObject);
            _digCount.HoldReset();
            _holoGlam.SetActive(false);
        }

        void ChangeText()
        {
            if (_parks[_select] == null)
            {
                _explainText.SetText("取得できるパークがありません");
                return;
            }
            _explainText.SetText(_parks[_select].GetNextLevelExplain());
        }
    }
}
