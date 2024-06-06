using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Syuntoku.DigMode.UI
{

    public class ParkMemory : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler
    {
        public int ParkNum;

        [SerializeField]
        UI_ParkManage _ParkManage;

        public void OnPointerDown(PointerEventData eventData)
        {
            _ParkManage.Selected();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _ParkManage.SetSelect(ParkNum);
        }
    }
}
