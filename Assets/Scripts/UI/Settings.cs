using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    //volume slider on scene
    [SerializeField] private Slider volumeSlider;



    private void Update()
    {
        SoundSet();
    }
    //sound set method
    private void SoundSet()
    {
        AudioListener.volume = volumeSlider.value;
    }
}
