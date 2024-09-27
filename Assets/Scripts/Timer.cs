using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;

    float timeRunning = 0;

    void Update()
    {
        timeRunning += Time.deltaTime;
        //Debug.Log(timeRunning);

        timerText.text = FormatTime(timeRunning);
    }
    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        return string.Format("{0:0}:{1:00}", minutes, seconds);
    }
}
