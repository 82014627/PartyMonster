using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleAudioManager : MonoSingleton<BattleAudioManager>
{
    public AudioClip[] PlayerClips = new AudioClip[3];
    public Dictionary<int, AudioClip[]> HeroClips = new Dictionary<int, AudioClip[]>();
    public AudioClip[] PropClips = new AudioClip[5];
    public AudioClip[] EndClips = new AudioClip[2];
    public AudioSource audioSource;
    /// <summary>
    /// 初始化
    /// </summary>
    void Start()
    {
        audioSource = transform.gameObject.AddComponent<AudioSource>();

        PlayerClips[0] = Resources.Load<AudioClip>("Audio/Effect-audio/Hero/Jump");
        PlayerClips[1] = Resources.Load<AudioClip>("Audio/Effect-audio/Hero/Hurt");
        PlayerClips[2] = Resources.Load<AudioClip>("Audio/Effect-audio/Hero/Dead");

        for (int i = 0; i < 5; i++)
        {
            int heroID = 1001;
            heroID += i;
            AudioClip[] audioClips = new AudioClip[3];
            audioClips[0] = Resources.Load<AudioClip>($"Audio/Effect-audio/Hero/{heroID}/J");
            audioClips[1] = Resources.Load<AudioClip>($"Audio/Effect-audio/Hero/{heroID}/K");
            audioClips[2] = Resources.Load<AudioClip>($"Audio/Effect-audio/Hero/{heroID}/L");
            HeroClips.Add(heroID, audioClips);
        }

        PropClips[0] = Resources.Load<AudioClip>("Audio/Effect-audio/Props/Heal");
        PropClips[1] = Resources.Load<AudioClip>("Audio/Effect-audio/Props/Powerup");
        PropClips[2] = Resources.Load<AudioClip>("Audio/Effect-audio/Props/Powerup");
        PropClips[3] = Resources.Load<AudioClip>("Audio/Effect-audio/Props/Invincible");
        PropClips[4] = Resources.Load<AudioClip>("Audio/Effect-audio/Props/Bomb");

        EndClips[0] = Resources.Load<AudioClip>("Audio/Effect-audio/End/Win");
        EndClips[1] = Resources.Load<AudioClip>("Audio/Effect-audio/End/Lose");
    }

    public void Jump()
    {
        audioSource.PlayOneShot(PlayerClips[0]);
    }
    public void Hurt()
    {
        audioSource.PlayOneShot(PlayerClips[1]);
    }
    public void Dead()
    {
        audioSource.PlayOneShot(PlayerClips[2]);
    }
    public void Heal()
    {
        audioSource.PlayOneShot(PropClips[0]);
    }
    public void PowerUp()
    {
        audioSource.PlayOneShot(PropClips[1]);
    }
    public void Invincible()
    {
        audioSource.PlayOneShot(PropClips[3]);
    }
    public void Bomb()
    {
        audioSource.PlayOneShot(PropClips[4]);
    }
    public void Win()
    {
        audioSource.PlayOneShot(EndClips[0]);
    }
    public void Lose()
    {
        audioSource.PlayOneShot(EndClips[1]);
    }
}
