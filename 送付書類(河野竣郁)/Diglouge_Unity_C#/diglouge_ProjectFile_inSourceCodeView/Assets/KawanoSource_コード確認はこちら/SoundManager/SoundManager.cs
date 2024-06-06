using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Syuntoku.DigMode.Sound
{
    public class SoundManager : MonoBehaviour
    {
        static AudioSource _audioSource2D;

        //　GameSoundShot
        [SerializeField] AudioMixerSnapshot gameSoundShot;
        //　OptionSoundShot
        [SerializeField] AudioMixerSnapshot optionSoundShot;

        [SerializeField] AudioMixer audioMixer;

        static AudioClip[] _publicSound;

        const float MUTE_VLUME = -50;
        const float MIN_VOLUME = -80;

        public enum PublicSound
        {
            DECISION,
            RERURN,
        }

        private void Start()
        {
            _audioSource2D = GetComponent<AudioSource>();
        }

        public static void Play2DSound(PublicSound kind)
        {
            _audioSource2D.clip = _publicSound[(int)kind];
            _audioSource2D.Play();
        }

        public void SetMaster(float volume)
        {
            audioMixer.SetFloat("MasterVol", volume);
        }

        public void SetBGM(float volume)
        {
            /*
             ある程度BGMが聞こえなくなるとミュートにする
             オーディオミキサーで音量調節すると音の調整が指数関数になるため
             */
            if(volume <= MUTE_VLUME)
            {
                audioMixer.SetFloat("BGMVol", MIN_VOLUME);
                return;
            }

            audioMixer.SetFloat("BGMVol", volume);
        }

        public void SetSE(float volume)
        {
            /*
             ある程度BGMが聞こえなくなるとミュートにする
             オーディオミキサーで音量調節すると音の調整が指数関数になるため
             */
            if (volume <= MUTE_VLUME)
            {
                audioMixer.SetFloat("SEVol", MIN_VOLUME);
                return;
            }
            audioMixer.SetFloat("SEVol", volume);
        }
    }
}
