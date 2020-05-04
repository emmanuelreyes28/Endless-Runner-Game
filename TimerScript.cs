using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerScript : MonoBehaviour
{
    public static float timer;
    float minutes;
    float seconds;
    float mseconds;
    public TextMeshProUGUI timerText;

    // Update is called once per frame
    void Update()
    {
        if(!PlayerController.playerDied)
        {
            //start timer
            timer += Time.deltaTime;

            minutes = (int)(timer) / 60;
            seconds = (int)(timer) % 60;
            mseconds = timer * 1000;
            mseconds = mseconds % 1000;        
    
            //update time text during runtime
            timerText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, (int)(mseconds));
            
        }
    }
}
