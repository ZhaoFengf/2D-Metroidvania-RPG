using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UIVolumeSlider : MonoBehaviour
{
    public Slider slider;
    public string parameter;

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private float multiplier;

    public void SliderValue(float _val)
    {
        audioMixer.SetFloat(parameter, Mathf.Log10(_val) * multiplier);
    }

    public void LoadSlider(float _val)
    {
        if(_val >= 0.001f)
        {
            slider.value = _val;
        }
    }
}
