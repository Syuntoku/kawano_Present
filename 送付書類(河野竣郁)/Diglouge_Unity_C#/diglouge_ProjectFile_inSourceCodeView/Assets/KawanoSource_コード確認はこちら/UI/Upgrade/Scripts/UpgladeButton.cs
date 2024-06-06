using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Syuntoku.DigMode.UI
{

    public class UpgladeButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {
        #region CashVariable
        [SerializeField] UpgladeUI _upgladeUI;
        [SerializeField] Sprite[] _stateImage;
        #endregion
        public int ButtonNum;
        bool _bActive;
        Image _sprite;
        State _state;
        RectTransform rectTransform;

        enum State
        {
            NOTSELECT = 0x00,
            IDLE = 0x01,
            ENTER = 0x02,
            SELECT = 0x04,
        }
        enum ImageState
        {
            NOTSELECT,
            IDLE,
            ENTER,
            SELECT,
        }

        //==================================
        //Unity
        //==================================
        void Start()
        {
            rectTransform = GetComponent<RectTransform>();
            _sprite = GetComponent<Image>();
        }

        //==================================
        //public
        //==================================
        public void SetActive(bool active)
        {
            _sprite = GetComponent<Image>();
            _bActive = active;

            if(!_bActive)
            {
                _sprite.sprite = _stateImage[(int)ImageState.NOTSELECT];
            }
            else
            {
                _sprite.sprite = _stateImage[(int)ImageState.IDLE];
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!_bActive) return;
            _upgladeUI.OnEnter(rectTransform.position, ButtonNum);
            _state = State.ENTER;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!_bActive) return;
            _state = State.IDLE;
            _sprite.sprite = _stateImage[(int)State.IDLE];
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!_bActive) return;
            _upgladeUI.OnSelect(ButtonNum);
            _state = State.SELECT;
            _sprite.sprite = _stateImage[(int)State.ENTER];
        }
    }
}
