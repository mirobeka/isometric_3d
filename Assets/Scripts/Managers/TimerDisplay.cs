using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MText;

public class TimerDisplay : MonoBehaviour
{

    public CasovacDoOdchodu casovac;
    private string currentTime;
    private Modular3DText modularText;

    void Start(){
        modularText = GetComponent<Modular3DText>();
    }

    // Update is called once per frame
    void Update()
    {
        string time = casovac.StrfTime();
        if (time != currentTime){
            UpdateTimeDisplay(time);
            currentTime = time;
        }
    }

    public void UpdateTimeDisplay(string time){
        modularText.UpdateText(time);
    }
}
