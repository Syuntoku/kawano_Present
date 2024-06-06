using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace Syuntoku.DigMode.UI
{
    /// <summary>
    ///　ダイアログのUI
    /// </summary>
    public class DialogUi : MonoBehaviour
    {
        #region CashVariable
        [SerializeField] TMP_Text _drawtext;
        [SerializeField] TMP_Text _pasetext;
        [SerializeField] TMP_Text _titleText;
        [SerializeField] GameObject _imagePrf;
        [SerializeField] GameObject _scaleObject;
        #endregion
        UIManage _uIManage;
        List<string> _pasedraw;
        int _nowDrawCount;
        int _maxdrawCount;
        const float END_DRAW_ANIMATION = 0.2f;

        DialogUi()
        {
            _pasedraw = new List<string>();
        }

        //============================
        //Unity
        //============================
        public void Start()
        {
            _scaleObject.transform.localScale = Vector3.right;
            _scaleObject.transform.DOScale(Vector3.one, END_DRAW_ANIMATION);
        }
 
        //======================================
        //public
        //======================================
        public void Initialize(UIManage uIManage)
        {
            _uIManage = uIManage;
        }

        public void AddPaseText(string data)
        {
            _pasedraw.Add(data);
            _maxdrawCount++;
            RssetPaseText();

            if (_pasedraw.Count == 1)
            {
                NextPase();
            }
        }

        public void SetTitleText(string data)
        {
            _titleText.SetText(data);
        }

        public void BackUI()
        {
            _uIManage.OutUiMode(gameObject);
        }

        /// <summary>
        /// ダイアログに画像を追加する
        /// </summary>
        public void InstanceImage(Vector3 position, Sprite sprite)
        {
            GameObject generateObject = Instantiate(_imagePrf, position, Quaternion.identity, _scaleObject.transform);
            generateObject.GetComponent<RectTransform>().anchoredPosition = position;
            Image image = generateObject.GetComponent<Image>();
            image.sprite = sprite;
            image.SetNativeSize();
        }

        //======================================
        //private
        //======================================
        void NextPase()
        {
            _drawtext.SetText(_pasedraw[_nowDrawCount]);
        }

        void RssetPaseText()
        {
            _pasetext.SetText((_nowDrawCount + 1).ToString() + "/" + _maxdrawCount);
        }
    }
}
