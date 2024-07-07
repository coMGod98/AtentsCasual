using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class BGM : MonoBehaviour
{
    public AudioMixer mixer;
    
    public void SetLevel(float sliderval)
    {
        mixer.SetFloat("BGM", Mathf.Log10(sliderval) * 20);
    }

}
