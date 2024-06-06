using Syuntoku.DigMode.Wave;
using UnityEngine;

namespace Syuntoku.DigMode.Settings
{
    /// <summary>
    /// BGMÇä«óùÇ∑ÇÈ
    /// </summary>
    public class BGMManage : MonoBehaviour
    {
        [SerializeField] AudioSource _bgmSource;
        [SerializeField] AudioClip _digmodeBgmClip;
        [SerializeField] AudioClip _battleModeBgmClip;

        void Start()
        {
            _bgmSource.Play();
            _bgmSource.loop = true;
        }

        public void SetClip()
        {
            if (WaveManage._bDigmode)
            {
                _bgmSource.clip = _digmodeBgmClip;
            }
            else
            {
                _bgmSource.clip = _battleModeBgmClip;
            }
            _bgmSource.Play();
        }
    }
}
