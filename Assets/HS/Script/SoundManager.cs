using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager sm;
    public AudioSource efcAudio;
    public AudioSource bgmAudio;

    [Header("Option")]
    [SerializeField]
    private float backgroundVolume;
    public float BackgroundVolume
    {
        get { return backgroundVolume; }
        set
        {
            backgroundVolume = value;
            bgmAudio.volume = backgroundVolume * totalVolume;
            PlayerPrefs.SetFloat("BackgroundVolume", value);
        }
    }
    [SerializeField]
    private float efcVolume;
    public float EfcVolume
    {
        get { return efcVolume; }
        set
        {
            efcVolume = value;
            efcAudio.volume = efcVolume * totalVolume;
            PlayerPrefs.SetFloat("EfcVolume", value);
        }
    }
    [SerializeField]
    private float totalVolume;
    public float TotalVolume
    {
        get { return totalVolume; }
        set
        {
            totalVolume = value;
            bgmAudio.volume = backgroundVolume * totalVolume;
            efcAudio.volume = efcVolume * totalVolume;
            PlayerPrefs.SetFloat("TotalVolume", value);
        }
    }

    [Header("Effect")]
    public AudioClip efcHitSnow;
    public AudioClip efcHitCandy;
    public AudioClip efcGetItem;
    public AudioClip efcCrowdWoo;
    public AudioClip efcCrowdYeah;

    [Header("UI")]
    public AudioClip efcClickButton;

    [Header("Background")]
    public AudioClip bgmChristmas;

    private void Awake()
    {
        if (sm == null)
            sm = this;
        else
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);

        BackgroundVolume = PlayerPrefs.GetFloat("BackgroundVolume", 1f);
        EfcVolume = PlayerPrefs.GetFloat("EfcVolume", 1f);
        TotalVolume = PlayerPrefs.GetFloat("TotalVolume", 1f);

        BGMChristmas();
    }

    public void BGMChristmas()
    {
        bgmAudio.loop = true;
        bgmAudio.clip = bgmChristmas;
        bgmAudio.Play();
    }

    // 눈 소리
    public void EfcHitSnow()
    {
        efcAudio.PlayOneShot(efcHitSnow, EfcVolume * TotalVolume);
    }

    public void EfcHitCandy()
    {
        efcAudio.PlayOneShot(efcHitCandy, EfcVolume * TotalVolume);
    }

    public void EfcGetItem()
    {
        efcAudio.PlayOneShot(efcGetItem, EfcVolume * TotalVolume);
    }

    public void EfcCrowdYeah()
    {
        efcAudio.PlayOneShot(efcCrowdYeah, EfcVolume * TotalVolume);
    }

    public void EfcCrowdWoo()
    {
        efcAudio.PlayOneShot(efcCrowdWoo, EfcVolume * TotalVolume);
    }

    public void EfcClickButton()
    {
        efcAudio.PlayOneShot(efcClickButton, EfcVolume * TotalVolume);
    }
}
