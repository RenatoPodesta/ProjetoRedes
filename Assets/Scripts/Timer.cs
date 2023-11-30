using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] TMP_Text timerText;
    [SerializeField] float elapsedTime;
    [SerializeField] Canvas cvScore;
    [SerializeField] Canvas hud;

    private void Start()
    {
        cvScore.gameObject.SetActive(false);
        hud.gameObject.SetActive(true);
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    //void StopTimer()
    //{
    //    if (elapsedTime > 0)
    //    {
    //        elapsedTime += Time.deltaTime;
    //    }
    //    else if (elapsedTime < 0)
    //    {
    //        elapsedTime = 0;
    //        Time.timeScale = 0;
    //        ScoreCanvas();
    //    }
        
    //}

    void ScoreCanvas()
    {
        cvScore.gameObject.SetActive(true);
        hud.gameObject.SetActive(false);
    }
}
