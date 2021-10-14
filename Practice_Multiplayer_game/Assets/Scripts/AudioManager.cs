﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoSingleton<AudioManager>
{
    int i;
    public AudioClip[] BGMs= new AudioClip[4];
    public AudioClip[] BtnClips = new AudioClip[2];
    public AudioClip[] PlayerClips = new AudioClip[3];
    public Dictionary<int, AudioClip[]> HeroClips = new Dictionary<int, AudioClip[]>();
    public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        i = 0;
        BGMs[0] = Resources.Load<AudioClip>("Audio/BGM/BGM-Start");
        BGMs[1] = Resources.Load<AudioClip>("Audio/BGM/BGM-Login");
        BGMs[2] = Resources.Load<AudioClip>("Audio/BGM/BGM-Lobby");
        BGMs[3] = Resources.Load<AudioClip>("Audio/BGM/BGM-Battle");
        BtnClips[0] = Resources.Load<AudioClip>("Audio/Effect-audio/Button/OkBtn");
        BtnClips[1] = Resources.Load<AudioClip>("Audio/Effect-audio/Button/BackBtn");
 
        audioSource = transform.gameObject.AddComponent<AudioSource>();
        audioSource.volume = 0.1f;
        audioSource.clip = BGMs[0];
        audioSource.Play();
        audioSource.loop = true;
        DontDestroyOnLoad(transform.gameObject);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Login")
        {
            audioSource.Pause();
            audioSource.clip = BGMs[1];
            audioSource.Play();
        }
        else if (SceneManager.GetActiveScene().name == "Lobby")
        {
            audioSource.Pause();
            audioSource.clip = BGMs[2];
            audioSource.Play();
        }
        else if (SceneManager.GetActiveScene().name == "Battle")
        {
            audioSource.Pause();
            audioSource.clip = BGMs[3];
            audioSource.Play();
        }
    }
    public void OKBtn()
    {
        audioSource.PlayOneShot(BtnClips[0]);
    }
    public void BackBtn()
    {
        audioSource.PlayOneShot(BtnClips[1]);
    }
}
