using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfigBox : MonoBehaviour
{
    public Button CancleBtn;
    public Toggle SoundToggle;
    public Slider VolumeSlider;

    public Button OkBtn;

    private void Start()
    {
        if (CancleBtn != null)
            CancleBtn.onClick.AddListener(()=> { Destroy(this.gameObject); });

        if (SoundToggle != null)
            SoundToggle.onValueChanged.AddListener(SoundOnOff);

        bool a_SoundOnOff = System.Convert.ToBoolean(PlayerPrefs.GetInt("SoundOnOff"));
        if (SoundToggle != null)
            SoundToggle.isOn = a_SoundOnOff;

        if (VolumeSlider != null)
            VolumeSlider.onValueChanged.AddListener(ValumSliderCheck);

        float a_SoundVolume = PlayerPrefs.GetFloat("SoundVolume");
        if (VolumeSlider != null)
            VolumeSlider.value = a_SoundVolume;
    }

    private void SoundOnOff(bool value)
    {
        if (value == true)
        {

            PlayerPrefs.SetInt("SoundOnOff", 1);
        }
        else
        {
            PlayerPrefs.SetInt("SoundOnOff", 0);
        }

    }
    private void ValumSliderCheck(float value)
    {
        PlayerPrefs.SetFloat("SoundVolume", value);
    }

}
