using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Syuntoku.DigMode.UI
{

    public class GetPark : MonoBehaviour, IPointerClickHandler
    {
     [SerializeField] UI_ParkManage _parkSystem;

        public void OnPointerClick(PointerEventData eventData)
        {
            _parkSystem.GetPark();
        }
    } 
}
