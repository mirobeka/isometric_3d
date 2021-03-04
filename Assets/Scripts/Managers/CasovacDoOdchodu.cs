using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CasovacDoOdchodu : MonoBehaviour
{
    public float timeRemaining = 10f;
    public bool timerIsRunning = true;
    public GameObject autobus;

    public TMPro.TextMeshProUGUI timeLabel;

    // Start is called before the first frame update
    void Start()
    {
        // neštartuj timer hneď, radšej počkaj na signál
        //timerIsRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUITimer();

        if (timerIsRunning){
            DoCountdown();
        }else{
            // Ak časovač nebeží, tak ho vypni
            timeLabel.text = "";
        }

    }

    void DoCountdown(){
        if (timeRemaining > 0){
            timeRemaining -= Time.deltaTime;
        }

        if (timeRemaining <= 0){
            timeRemaining = 0;
            timerIsRunning = false;
            ExecuteTimerTrigger();
        }
    }

    // funkcia ktorá sa vykoná keď dobehne timer
    void ExecuteTimerTrigger(){
        // naštartuj autobus
        autobus.SetActive(true);
    }
    
    void UpdateUITimer(){
        string time = StrfTime();
        timeLabel.text = time;
    }

    public void StartTimer(){
        timerIsRunning = true;
    }

    public string StrfTime(){
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

}
