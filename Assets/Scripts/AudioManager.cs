using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioSource effect;
    public AudioSource music;

    public AudioClip gameMusic;
    public AudioClip menuMusic;
    public AudioClip effectClick;
    public AudioClip effectClick2;

    public Slider sliderMusic;
    public Slider sliderSFX;

    public static AudioManager instance;

    void Awake()
    {
        instance = this;
        InitializeVolumen();
    }

    private void InitializeVolumen()
    {
        effect.volume = PlayerPrefs.GetFloat("effectVolumen", 1.0f);
        music.volume = PlayerPrefs.GetFloat("musicVolumen", 0.5f);

        //sliderMusic.value = music.volume;
        //sliderSFX.value = effect.volume;
    }

    public void PlayEffect()
    {
        effect.PlayOneShot(effectClick);
    }
    public void PlayEffect2()
    {
        effect.PlayOneShot(effectClick2);
    }

    public void PlaySong(AudioClip audioClip)
    {
        music.clip = audioClip;
        music.Play();
    }
    public void PlayGame()
    {
        music.clip = gameMusic;
        music.Play();
    }

    public void OnMusicVolumeUpdate()
    {
        music.volume = sliderMusic.value;
        PlayerPrefs.SetFloat("musicVolumen", music.volume);
        PlayerPrefs.Save();
    }

    public void OnSFXVolumeUpdate()
    {
        effect.volume = sliderSFX.value;
        PlayerPrefs.SetFloat("effectVolumen", effect.volume);
        PlayerPrefs.Save();
    }
    
}
