using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
namespace Syuntoku.DigMode.UI
{
    public class MainUI : MonoBehaviour
    {
        [SerializeField] TMP_Text _hpText;
        [SerializeField] UnityEngine.UI.Image _hpBar;

        [SerializeField] TMP_Text _equipmentText;
        [SerializeField] TMP_Text _magazineSizeText;

        [SerializeField] GameObject _toolHUD;
        [SerializeField] GameObject _weaponHUD;

        [SerializeField] Transform _itemHolder;
        [SerializeField] GameObject _itemGetWindow;

        [SerializeField] Vector3 _startPos;
        [SerializeField] float _startPosX;
        [SerializeField] int[] _transform_Y;

        List<MainUI.HoldFillter> _getItemHolder;
        const int GET_ITEM_UI_DRAW_MAX = 3;

        private void Start()
        {
            _getItemHolder = new List<HoldFillter>();
        }

        private void Update()
        {
            for (int i = 0; i < _getItemHolder.Count; i++)
            {
                _getItemHolder[i].AddTime(_getItemHolder,_startPos);
            }
        }

        /// <summary>
        /// HPに対応したUIを変更
        /// </summary>
        /// <param name="hp"></param>
        /// <param name="maxHp"></param>
        public void ChangeHpUI(float hp , float maxHp)
        {
            _hpText.SetText(hp.ToString() + "/" + maxHp);
            float pribability = hp / maxHp;
            _hpBar.fillAmount = pribability;
        }

        public void SetMagazineSize(int size)
        {
            _magazineSizeText.text = size.ToString();
        }

        /// <summary>
        /// 装備している名前を設定
        /// </summary>
        /// <param name="equipmentText"></param>
        public void SetEquipmentText(string equipmentText)
        {
            _equipmentText.SetText(equipmentText);
        }

        /// <summary>
        /// 武器のUIを表示
        /// </summary>
        public void ActiveWeaponHUD()
        {
            _weaponHUD.SetActive(true);
            _toolHUD.SetActive(false);
        }

        /// <summary>
        /// ツールのUIを表示
        /// </summary>
        public void ActiveToolHUD()
        {
            _weaponHUD.SetActive(false);
            _toolHUD.SetActive(true);
        }

        public void InstanceGetItemUI(Sprite icon, string pushKey)
        {
            for (int i = 0; i < _getItemHolder.Count; i++)
            {
                if (_getItemHolder[i].fillterName == pushKey)
                {
                    if (_getItemHolder[i].bDestroyMosion) continue;
                    _getItemHolder[i].Reset(); ;
                    return;
                }
            }

            //UI生成
            GameObject generate = Instantiate(_itemGetWindow, Vector3.zero, Quaternion.identity, _itemHolder);
            generate.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
            UnityEngine.UI.Image iconImage = generate.transform.GetChild(1).gameObject.GetComponent<UnityEngine.UI.Image>();
            iconImage.sprite = icon;

            //アイテムゲット情報を生成
            HoldFillter fillter = new HoldFillter();
            fillter.fillterName = pushKey;
            fillter.holdObject = generate;

            //表示する高さを計算
            fillter.index = CheckIndex();

            if (GetUICount() + 1 > GET_ITEM_UI_DRAW_MAX)
            {

                float maximumTime = 0.0f;
                int maxTimeIndex = 0;
                for (int i = 0; i < _getItemHolder.Count; i++)
                {
                    if (_getItemHolder[i].bDestroyMosion) continue;
                    if (_getItemHolder[i].time > maximumTime)
                    {
                        maximumTime = _getItemHolder[i].time;
                        maxTimeIndex = i;
                    }
                }
                _getItemHolder[maxTimeIndex].DestroyUI(_getItemHolder, _startPos);
                fillter.index = maxTimeIndex;
#if UNITY_EDITOR
                Debug.Log("MAXINDEX = " + maxTimeIndex);
#endif
            }

            fillter.Start(_startPos, _transform_Y);
            _getItemHolder.Add(fillter);
        }

        /// <summary>
        /// 表示する高さを計算
        /// </summary>
        /// <returns></returns>
        int CheckIndex()
        {
            int index = 0;
            for (int i = 0; i < _getItemHolder.Count; i++)
            {
                if (_getItemHolder[i].index == index)
                {
                    if (_getItemHolder[i].bDestroyMosion)
                    {
                        break;
                    }
                    index++;
                }
            }
#if UNITY_EDITOR
            Debug.Log("CheckIndex" + index);
#endif
            return index;
        } 

        int GetUICount()
        {
            int count = 0;

            for (int i = 0; i < _getItemHolder.Count; i++)
            {
                if (_getItemHolder[i].bDestroyMosion) continue;
                count++;
            }
            return count;
        }

        class HoldFillter
        {
            public string fillterName;
            public int index;
            public GameObject holdObject;
            public float count = 1;
            public float time;
            public int transition;
            public const int DESTORY = 2;
            public bool bDestroyMosion = false;
            const float DESTROY_TIME = 2.0f;
            const float MAX_SIZE_TIME = 0.05f;
            const float TRANSFORM_TIME = 0.1f;
            readonly Vector3 defaultScale = new Vector3(1.0f, 0.3f, 0.3f);
            readonly Vector3 startAjust = new Vector3(-10, 0.0f, 0.0f);
            Vector3 ajustHeight;
            RectTransform transform;
            TMP_Text text;

            /// <summary>
            /// 初期化
            /// </summary>
            /// <param name="startPos"></param>
            /// <param name="transformData"></param>
            public void Start(Vector3 startPos,int[] transformData)
            {
                transform = holdObject.GetComponent<RectTransform>();
               text = holdObject.transform.GetChild(0).GetComponent<TMP_Text>();
                ajustHeight = new Vector3(0.0f, transformData[index], 0.0f);
                transform.anchoredPosition = startPos+ ajustHeight;
                transform.localScale = Vector3.zero;

                //アニメーション
                transform.DOScale(defaultScale, MAX_SIZE_TIME);
                transform.DOAnchorPos(startPos+ startAjust + ajustHeight, TRANSFORM_TIME);
                text.SetText(count.ToString());
            }

            public void DestroyUI(List<HoldFillter> list,Vector3 startPos)
            {
                bDestroyMosion = true;
                transform.DOAnchorPos(startPos + ajustHeight, 0.1f).OnComplete(()=>
                {
                    transform.DOKill();
                    list.Remove(this);
                    Destroy(holdObject);
                });
            }

            public void AddTime(List<HoldFillter> list,Vector3 startPos)
            {
                time += Time.deltaTime;

                if (time >= DESTROY_TIME)
                {
                    DestroyUI(list,startPos);
                }
            }

            public void Reset()
            {
                if (bDestroyMosion) return;
                count++;
                time = 0.0f;
                text.SetText(count.ToString());
            }
        }
    }
}
