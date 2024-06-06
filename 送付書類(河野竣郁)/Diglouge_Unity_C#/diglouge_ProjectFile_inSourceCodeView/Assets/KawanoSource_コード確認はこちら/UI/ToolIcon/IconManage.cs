using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace Syuntoku.DigMode.UI
{

    public class IconManage : MonoBehaviour
    {
        public bool bActive;
        bool bLock;

       [SerializeField] Image _image;
        [SerializeField] Image _flameImage;
        RectTransform _rectTransform;

        [SerializeField] Sprite _disableFlame;
        [SerializeField] Sprite _ActiveFlame;
        [SerializeField] Sprite _flamePointerEnterSprite;
        [SerializeField] Sprite _flamePointerDownSprite;
        [SerializeField] Sprite _flamePointerExitSprite;
        [SerializeField] Sprite _flamePushedNoSelect;

        [SerializeField] TMP_Text _costText;
        [SerializeField] Image _juwelryIcon;

        public State _nowState;

        public enum State
        {
            INACTIVE = 0x00,
            ACTIVE = 0x01,
            SELECTED = 0x02,
            ONPUSH = 0x04,
            ONNOSELECT = 0x08,
        }

        public void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void SetIconActive(bool active)
        {
            if (bLock) return;

            if(active)
            {
                _flameImage.sprite = _ActiveFlame;
                _nowState = State.ACTIVE;
                bActive = true;
            }
            else
            {
                _flameImage.sprite = _disableFlame;
                bActive = false;

            }
        }

        public void LockChangeFlame()
        {
            bActive = false;
            bLock = true;
        }
        public void UnLockChangeFlame()
        {
            bActive = false;
            bLock = false;
        }

        public void SetMainIcon(Sprite sprite)
        {
            _image.sprite = sprite;
        }
        
        /// <summary>
        /// ステートでアイコンの表示を変える
        /// </summary>
        /// <param name="state"></param>
        public void SetState(State state)
        {
            if (bLock) return;
            switch (state)
            {
                case State.INACTIVE:
                    _flameImage.sprite = _disableFlame;
                    _nowState = State.INACTIVE;

                    break;
                case State.ACTIVE:
                    _flameImage.sprite = _ActiveFlame;
                    _nowState = State.ACTIVE;

                    break;
                case State.SELECTED:
                    _flameImage.sprite = _flamePointerEnterSprite;
                    _nowState = State.SELECTED;

                    break;
                case State.ONPUSH:
                    _flameImage.sprite = _flamePointerDownSprite;
                    _nowState = State.SELECTED;

                    break;
                case State.ONNOSELECT:
                    _flameImage.sprite = _flamePushedNoSelect;
                    _nowState = State.ONNOSELECT;

                    break;
                default:
                    break;
            }
        }


        public Vector3 GetRectPosition()
        {
            return _rectTransform.position;
        }

        public Vector3 GetRectLocalPosition()
        {
            return _rectTransform.localPosition;
        }

        /// <summary>
        /// コスト表記とアイコンをセットする
        /// </summary>
        /// <param name="iconSprite"></param>
        /// <param name="costText"></param>
        public void SetCostText(Sprite iconSprite , string costText)
        {
            _costText.SetText(costText);
            _juwelryIcon.sprite = iconSprite;
        }
    }

}