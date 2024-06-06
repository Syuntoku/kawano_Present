using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Syuntoku.DigMode.UI
{ 
    public class ParkIconButton : MonoBehaviour , IPointerEnterHandler,IPointerExitHandler ,IPointerClickHandler
    {
        [SerializeField] PauseUI _pauseUI;
        RectTransform _rectTransform;
        [SerializeField] Image _image;
        [SerializeField] Sprite[] _checkActiveIcon;
        ParkData.Park _cashPark;

        enum State
        {
            INVAILD,
            ACTIVE,
        }

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void SetParkData(ParkData.Park park)
        {
            if (park == null) return;
            _cashPark = park;
            SetActiveIcon(_cashPark.bValid);
        }

        public void SetActiveIcon(bool active)
        {
            if (_cashPark == null) return;
            _image.sprite = _checkActiveIcon[_cashPark.bValid == true ?  (int)State.ACTIVE : (int)State.ACTIVE];
        }

        public void SetPauseUiData(PauseUI pauseUI)
        {
            _pauseUI = pauseUI;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_pauseUI == null) return;
            _pauseUI.SetSelectTransform(_rectTransform);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_pauseUI == null) return;
            _pauseUI.SetSelectIconActive(false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_cashPark == null) return;
            _cashPark.bValid = !_cashPark.bValid;
            SetActiveIcon(_cashPark.bValid);
        }
    }
}
