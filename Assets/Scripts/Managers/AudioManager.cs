using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer _MainMixer;
    public float volume = 0f;
    public float fadeRate = 0.5f;

    // fade Toggle:
    //          -> true = fade in
    //          -> false = fade out
    //
    public bool fadeToggle = false;


    // Update is called once per frame
    void Update()
    {
        if(fadeToggle){
            // fade in
            volume = Mathf.Lerp(volume, 0f, fadeRate);
        }else{
            volume = Mathf.Lerp(volume, -80f, fadeRate);
        }

        // nastav volume
        _MainMixer.SetFloat("MasterVolume", volume);
    }

    public void FadeOutVolume(){
        fadeToggle = false;
    }

    public void FadeInVolume(){
        fadeToggle = true;

    }
}
