using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundObject : MonoBehaviour
{
    AudioSource _audioSource;

    private void Update()
    {
        if (!_audioSource.isPlaying) Destroy(gameObject);
    }

    public void SetAudio(AudioClip audioClip, Vector3 position)
    {
        _audioSource = GetComponent<AudioSource>();
        transform.position = position;
        _audioSource.clip = audioClip;
        _audioSource.Play();
    }
}
