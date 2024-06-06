using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Syuntoku.DigMode.UI
{
    public class EquipmentButton : MonoBehaviour, IPointerClickHandler
    {
        InfoManage _infoManage;
        [SerializeField] PauseUI pauseUI;
        UnityEngine.UI.Button _button;
        public void OnPointerClick(PointerEventData eventData)
        {
            if (!_button.enabled) return;
            pauseUI.ChangeFillterEquipmentMode(_infoManage.GetIdData());
        }

        private void Start()
        {
            _infoManage = transform.parent.GetComponent<InfoManage>();
            _button = GetComponent<UnityEngine.UI.Button>();
        }
    }
}
