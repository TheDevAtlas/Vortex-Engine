using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAudioManager : MonoBehaviour
{
    public AudioClip[] music;

    public AudioSource musicSource;
    public AudioSource buttonSelectSource;
    public AudioSource buttonPressSource;

    public void Start()
    {
        musicSource.clip = music[Random.Range(0, music.Length - 1)];
        musicSource.Play();
    }

    public void PlaySelect()
    {
        buttonSelectSource.Play();
    }

    public void PlayPress()
    {
        buttonPressSource.Play();
    }
}
