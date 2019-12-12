using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOption : MonoBehaviour
{
    [SerializeField]
    private Slider totalSlider;

    [SerializeField]
    private Slider backgroundSlider;

    [SerializeField]
    private Slider efcSlider;

    [SerializeField]
    public Toggle particleToggle;

    [SerializeField]
    private ParticleSystem particle;

    public GameObject panel;

    [SerializeField]
    private int saveParticle;
    public int SaveParticle
    {
        get { return saveParticle; }
        set
        {
            saveParticle = value;
            PlayerPrefs.SetInt("IsParticle", value);
        }
    }

    bool initParticle;


    private void Start()
    {
        totalSlider.value = SoundManager.sm.TotalVolume;
        backgroundSlider.value = SoundManager.sm.BackgroundVolume;
        efcSlider.value = SoundManager.sm.EfcVolume;
        SaveParticle = PlayerPrefs.GetInt("IsParticle", 1);

        initParticle = SaveParticle == 1 ? true : false;
        particleToggle.isOn = initParticle;
        SetParticle();
    }

    public void OnPanel()
    {
        if (panel.activeSelf)
            return;

        panel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void SetTotalSider()
    {
        SoundManager.sm.TotalVolume = totalSlider.value;
    }
    public void SetBGMSlider()
    {
        SoundManager.sm.BackgroundVolume = backgroundSlider.value;
    }
    public void SetEfcSlider()
    {
        SoundManager.sm.EfcVolume = efcSlider.value;
    }

    public void ResetParticleToggle()
    {
        SaveParticle = particleToggle.isOn == true ? 1 : 0;
        SetParticle();
    }

    public void SetParticle()
    {
        if (particle != null)
        {
            if (SaveParticle == 1)
                particle.Play();
            else
                particle.Stop();
        }
    }
}
