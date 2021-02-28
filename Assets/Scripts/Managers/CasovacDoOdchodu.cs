using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CasovacDoOdchodu : MonoBehaviour
{
    public float timeRemaining = 10f;
    public bool timerIsRunning = true;

    public TMPro.TextMeshProUGUI timeLabel;

    // Start is called before the first frame update
    void Start()
    {
        timerIsRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerIsRunning){
            DoCountdown();
        }
    }

    void DoCountdown(){
        if (timeRemaining > 0){
            timeRemaining -= Time.deltaTime;

        }else{
            Debug.Log("Timer finished");
            timeRemaining = 0;
            timerIsRunning = false;
        }

        UpdateUITimer();
    }
    
    void UpdateUITimer(){
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timeLabel.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

}
