using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Syuntoku.DigMode.Tool;

namespace Syuntoku.DigMode.UI
{
    public class InfoManage : MonoBehaviour ,IPointerClickHandler
    {
        PauseUI _pauseUI;
        int _selectId;
        bool _bDigmode;

        public enum HoldData
        {
         WEAPON,
         TOOL,
        }

        public void SetData(PauseUI pauseUI,bool digmode,int id)
        {
            _selectId = id;
            _pauseUI = pauseUI;
            _bDigmode = digmode;
        }
        public int GetIdData()
        {
            return _selectId;
        }

        public bool IsDigmode()
        {
            return _bDigmode;
        }

        public void DropItem()
        {
        }

        private void Reset()
        {
            _selectId = 0;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _pauseUI.ChangeFillterEquipmentMode(_selectId);
        }
    }
}
