using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Syuntoku.DigMode.UI
{
    public class FillterButton : MonoBehaviour , IPointerClickHandler
    {
        [SerializeField] PauseUI _pauseUI;
        public int filterKind;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (filterKind == 100) return;
            _pauseUI.ChangeFilter(filterKind);
        }
    }
}