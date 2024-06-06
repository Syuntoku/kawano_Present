using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

using DG.Tweening;

namespace Syuntoku.DigMode.UI
{
    public class DetailManager : MonoBehaviour , IPointerExitHandler ,IPointerEnterHandler
    {
        #region CashVariable
        [SerializeField] TMP_Text _infoText;
        [SerializeField] TMP_Text _amountText;
        #endregion
        Upglade2 _upglade2;
        Upglade2Button _button;
        Upglade2.Step _step;
        public bool _bOnCursor;
        bool _bEndDraw;
        float _destroyTiemer;
        const float DESTROY_TIME = 0.2f;

        const float SCAL_DELAY = 0.1f;
        const float SELECTED_SCALE = 1.05f;

        //=========================================
        //Unity
        //=========================================
        private void Update()
        {
            if (_bOnCursor || _button._onCursor)
            {
                _destroyTiemer = 0.0f;
            }

            if (_bEndDraw)
            {
                _destroyTiemer += Time.deltaTime;
                if(_destroyTiemer >= DESTROY_TIME)
                {
                    EndDraw();
                }
            }
        }
        //=========================================
        //public
        //=========================================
        public void Initialize(Upglade2 upglade2,Upglade2Button upglade2Button,string detailText,Upglade2.Step step)
        {
           string[] data = detailText.Split(",");
            _infoText.SetText(data[0]);
            _amountText.SetText(data[1]);
            _upglade2 = upglade2;
            _button = upglade2Button;
            _step = step;
        }

        public void UpgladeTool()
        {
            _upglade2.UpgladeTool(_step);
            _button._iconManage.SetState(IconManage.State.ONNOSELECT);
            _button._iconManage.LockChangeFlame();
            gameObject.SetActive(false);
            _upglade2._bDeailDraw = false;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _bOnCursor = false;
        }

        public void EndDraw()
        {
            if (_bOnCursor || _button._onCursor) return;
            _upglade2.SetOutlineTransform(Vector3.zero, false);
            transform.DOScale(Vector3.right, SCAL_DELAY).OnComplete(() => { gameObject.SetActive(false); });

            _upglade2._bDeailDraw = false;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.DOScale(new Vector3(SELECTED_SCALE, SELECTED_SCALE, SELECTED_SCALE), SCAL_DELAY);
            _bOnCursor = true;
        }
    }
}
