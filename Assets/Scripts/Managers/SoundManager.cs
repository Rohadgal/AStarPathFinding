using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region Public
    public static SoundManager s_instance;
    #endregion

    public AudioSource backgroundMusicSource;
    public AudioSource sfxSource;

    // Background music clip
    public AudioClip backgroundMusicClip;

    // Example SFX clips
    public AudioClip sfxClip_Start;
    public AudioClip sfxClip_Mode;
    public AudioClip sfxClip_Credits;
    public AudioClip sfxClip_Victory;
    public AudioClip sfxClip_Caught;

    private void Awake() {
        //if (FindObjectOfType<SoundManager>() != null &&
        //    FindObjectOfType<SoundManager>().gameObject != gameObject) {
        //    Destroy(gameObject);
        //    return;
        //}

        //DontDestroyOnLoad(gameObject);
        s_instance = this;
    }

    void Start() {
        // Play background music
        PlayBackgroundMusic(backgroundMusicClip);
    }

    // Play background music
    public void PlayBackgroundMusic(AudioClip musicClip) {
        backgroundMusicSource.clip = musicClip;
        backgroundMusicSource.loop = true;
        backgroundMusicSource.Play();
    }

    // Play a single SFX clip
    public void PlaySFX(AudioClip sfxClip) {
        sfxSource.PlayOneShot(sfxClip);
    }

    // Example methods to play specific SFX clips
    public void PlaySFXStart() {
        PlaySFX(sfxClip_Start);
    }

    public void PlaySFXMode() {
        PlaySFX(sfxClip_Mode);
    }

    public void PlaySFXCredits() {
        PlaySFX(sfxClip_Credits);
    }

    public void PlaySFXVictory() {
        Debug.Log("Victory");
        PlaySFX(sfxClip_Victory);
    }

    public void PlaySFXCaught() {
        PlaySFX(sfxClip_Caught);
    }
}
