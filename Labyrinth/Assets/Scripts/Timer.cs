using System;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timeText;
    //private float score;
    float elapsedTime;

    private bool isRunning = false;

    private void Awake()
    {
        isRunning = true;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isRunning = false;
        }
    }

    void Update()
    {
        if (isRunning == true)
        {
            elapsedTime += Time.deltaTime;
            int minutes = Mathf.FloorToInt(elapsedTime / 60);
            int seconds = Mathf.FloorToInt(elapsedTime % 60);
            timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        //score = elapsedTime;
    }
}
