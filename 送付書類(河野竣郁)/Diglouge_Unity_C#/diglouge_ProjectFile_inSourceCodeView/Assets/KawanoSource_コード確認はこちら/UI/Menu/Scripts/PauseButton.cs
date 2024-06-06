using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Syuntoku.DigMode.UI
{
    public class PauseButton : MonoBehaviour, IPointerEnterHandler ,IPointerExitHandler ,IPointerClickHandler
    {
        [SerializeField] PauseUI _pauseUI;
        RectTransform _rectTransform;
        public int id;

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void SetPauseUiData(PauseUI pauseUI)
        {
            _pauseUI = pauseUI;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _pauseUI.SetSelectIconActive(true);
            _pauseUI.SetSelectTransform(_rectTransform);
            _pauseUI.EnterButtonID(id);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _pauseUI.SetSelectIconActive(false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _pauseUI.SelectButton(_rectTransform, id);
        }
    }
}
