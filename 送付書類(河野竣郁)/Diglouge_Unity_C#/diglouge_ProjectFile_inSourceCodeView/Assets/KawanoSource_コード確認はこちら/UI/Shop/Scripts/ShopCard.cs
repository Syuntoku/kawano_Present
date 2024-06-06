using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Syuntoku.DigMode.Shop;
using UnityEngine.UI;

public class ShopCard : MonoBehaviour ,IPointerEnterHandler,IPointerExitHandler,IPointerDownHandler
{
    public int _cardNum;

    int _state;
    Image _image;
    bool _bSold;

    [SerializeField] Sprite[] cardState;
    [SerializeField] GameObject soldOutPrefab;
    [SerializeField] ShopManage shopManage;

    RectTransform _rectTransform;

    GameObject buyObject;

    enum CardState
    {
        NONE,
        OnPointerEnter,
        OnSelected,
        OnSold,
    }

    private void Awake()
    {
        _state = (int)CardState.NONE;
        _rectTransform = GetComponent<RectTransform>();
        _image = GetComponent<Image>();
    }

    public void DataReset()
    {
        _state = (int)CardState.NONE;
        _bSold = false;
        SpriteChange();
        Destroy(buyObject);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
       if (_bSold) return;

        shopManage.SetSelectNum(_cardNum);
        shopManage.PointerEntor(_rectTransform, _cardNum);
        _state = (int)CardState.OnPointerEnter;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_bSold) return;

        _state = (int)CardState.NONE;
        shopManage.Notselecteed();
        SpriteChange();

    }

    void SpriteChange()
    {
        if (_state == (int)CardState.OnSold)
        {
            _bSold = true;

            //選択時のスプライトにする
            _image.sprite = cardState[_state - 1];

            buyObject = Instantiate(soldOutPrefab, transform);
        }
        else
        {
            _image.sprite = cardState[_state];
        }
    }

    public void Buy()
    {
        _state = (int)CardState.OnSold;
        SpriteChange();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_bSold) return;

        shopManage.Selected(_cardNum);
        _state = (int)CardState.OnSelected;
        SpriteChange();
    }
}
