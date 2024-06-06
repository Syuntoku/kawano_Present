using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Syuntoku.DigMode.UI;
using Syuntoku.DigMode.ParkData;
using TMPro;
using Syuntoku.DigMode.Settings;

namespace Syuntoku.DigMode.UI
{
    public class DigCount : MonoBehaviour
    {
        #region CashVarible
        [SerializeField] Image _image;
        [SerializeField] UIManage _uIManage;
        [SerializeField] ParkConditionsManage _parkConditionsManage;
        [SerializeField] TMP_Text _text;
        [SerializeField] GameSetting _gameSetting;
        [SerializeField] TMP_Text _breakCount;
        [SerializeField] TutorialManage _tutorialManage;
        [SerializeField] DigBreakLevelSpriptable _digBreakLevelSpriptable;
        #endregion
        const float ONE_AMOUNT_ADD_GAUGE = 0.08f;
        const float MAX_GAUGE_AMOUNT = 1.0f;
        public static int _parkCounter;
        public int breakCount;
        float _gauge;
        int _nextCount;
        bool _bHold;
        bool _bTutorial;
        int _digLevel;

        //============================
        //Unity
        //============================
        private void Start()
        {
            _image.fillAmount = _parkConditionsManage.GetParkToBreakCount / _parkConditionsManage.MaxGetParkToBreakCount;
            _nextCount = 0;
            _parkCounter = 0;
            _parkConditionsManage.MaxGetParkToBreakCount = (uint)_digBreakLevelSpriptable.digCount[_digLevel];
            _bHold =true;
        }

        //================================
        //public
        //================================
        public void HoldReset()
        {
            _bHold = true;
        }

        //================================
        //private
        //================================
        private void Update()
        {
            if (_gameSetting.bStopGameAction) return;

            if (_bHold && !Input.InputData._bGetPark)
            {
                _bHold = false;
            }

            //パークを取得できるようにする
            if (_parkConditionsManage.GetParkToBreakCount >= _parkConditionsManage.MaxGetParkToBreakCount)
            {
                _nextCount++;
                _digLevel++;
                _parkConditionsManage.GetParkToBreakCount = 0;
                _parkConditionsManage.MaxGetParkToBreakCount = (uint)_digBreakLevelSpriptable.digCount[_digLevel];
            }

            //パークの画面を表示する
            if (_parkCounter > 0)
            {
                if (!_bHold && Input.InputData._bGetPark)
                {
                    if(_bTutorial)
                    {
                        if (_tutorialManage == null) return;
                        _tutorialManage.SetTutorialInfo(TutorialManage.TutorialState.PARK);
                        _bTutorial = true;
                    }
                    _uIManage.DrawParkCanvas((int)POPKIND.DIG, _parkCounter);
                }
            }

            _text.SetText(_parkCounter.ToString());
            _gauge = (float)_parkConditionsManage.GetParkToBreakCount / (float)_parkConditionsManage.MaxGetParkToBreakCount;

            if(_image.fillAmount < _gauge || _nextCount > 0)
            {
                _image.fillAmount += ONE_AMOUNT_ADD_GAUGE;
            }
            
            if(_image.fillAmount >= MAX_GAUGE_AMOUNT)
            {
                if(_nextCount > 0)
                {
                    _nextCount--;
                    _parkCounter++;
                    _image.fillAmount = 0.0f;
                    return;
                }
                _parkCounter++;
                _image.fillAmount = _gauge;
            }
            _breakCount.SetText(_parkConditionsManage.BlockBreakCount.ToString());
        }
    }
}