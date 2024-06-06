using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Syuntoku.DigMode.UI.Button
{
    public class Button : MonoBehaviour ,  IPointerClickHandler
    {
        [SerializeField] StartOperationBoard startOperation;

        public int barCount;
        public int amount;

        void Start()
        {

        }

        void Update()
        {

        }

        public void OnPointerClick(PointerEventData eventData)
        {
            startOperation.MoveOneAmount(barCount, amount);
        }
    } 
}
