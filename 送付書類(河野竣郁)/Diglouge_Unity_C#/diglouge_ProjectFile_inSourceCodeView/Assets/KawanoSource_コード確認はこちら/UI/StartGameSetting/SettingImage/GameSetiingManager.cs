using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSetiingManager : MonoBehaviour
{
    [SerializeField] Image _audioSetting;
    [SerializeField] Image _ControlSetting;

    float _seVolume;
    float _bgmVolume;

    private void Start()
    {
        
    }

    void VolumeChangeSE(float amount)
    {
        _seVolume = amount;
    }

    void VolumeChangeBGM(float amount)
    {
        _bgmVolume = amount;
    }
}
