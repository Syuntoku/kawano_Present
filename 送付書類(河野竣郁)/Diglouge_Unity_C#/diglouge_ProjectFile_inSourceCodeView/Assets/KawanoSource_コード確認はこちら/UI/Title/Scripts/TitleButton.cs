using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TitleButton : MonoBehaviour, IPointerClickHandler ,IPointerEnterHandler
{
    [SerializeField] TitleUI titleUI;
    public int _buttonNum;
    RectTransform _rectTransform;
    public bool bStatic;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(bStatic)
        {
            return;
        }
        titleUI.OnClick();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(bStatic)
        {
            titleUI.SetTransform(_rectTransform, int.MaxValue);
            return;
        }
        titleUI.SetTransform(_rectTransform, _buttonNum);
    }
}
