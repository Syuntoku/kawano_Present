using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Syuntoku.DigMode.UI
{
    public class Upglade2Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] Upglade2 _upglade2;
        public IconManage _iconManage;
        public int _buttonId;
        public int _levelNum;
        public int step;
        public Upglade2.DetailDrawDirection drawDirectionVirtical;
        public Upglade2.DetailDrawDirection drawDirectionHorizontal;
        RectTransform _rectTransform;
        public bool _onCursor;

        public void Initialize()
        {
            _rectTransform = GetComponent<RectTransform>();
            _iconManage = GetComponent<IconManage>();
        }

        public void SetState(IconManage.State state)
        {
            _iconManage.SetState(state);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_iconManage._nowState == IconManage.State.INACTIVE) return;
            if (_iconManage._nowState == IconManage.State.ONNOSELECT) return;

            _iconManage.SetState(IconManage.State.SELECTED);
            _upglade2.SetOutlineTransform(_iconManage.GetRectLocalPosition());
            _upglade2.DrawDetail(this, _rectTransform.localPosition + transform.parent.localPosition, step, _levelNum, drawDirectionVirtical, drawDirectionHorizontal);
            _onCursor = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_iconManage._nowState == IconManage.State.ONNOSELECT) return;
            if (_iconManage._nowState == IconManage.State.INACTIVE) return;

            _upglade2.SetOutlineTransform(Vector3.zero, false);
            _iconManage.SetState(IconManage.State.ACTIVE);
            _onCursor = false;
        }
    }
}
