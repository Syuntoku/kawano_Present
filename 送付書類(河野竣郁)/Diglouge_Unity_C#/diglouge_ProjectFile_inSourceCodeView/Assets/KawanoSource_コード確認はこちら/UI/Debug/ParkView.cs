using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Syuntoku.DigMode.ParkData;
using UniRx;

namespace Syuntoku.DEBUG
{

    public class ParkView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _parkName;
        [SerializeField] private Toggle _activeToggle;

        public IObservable<bool> ToggleValue => _activeToggle.onValueChanged.AsObservable();


        public void Init(Park park)
        {
            _parkName.text = park.name;

            _activeToggle.OnValueChangedAsObservable().Subscribe(x =>
            {
                if (x)
                {
                    //park.levelParkData[park.nowLevel].EndSwing
                }
                else
                {

                }
            }).AddTo(this);
        }
    }

}