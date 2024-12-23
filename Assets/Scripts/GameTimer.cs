using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public DynamicDisplayText timerText;
    private float elapsedTime = 0f;
    private bool isTiming = false;

    void Start()
    {
        if (timerText == null)
        {
            Debug.LogError("TimerText is not assigned in the GameTimer.");
        }
        else
        {
            timerText.SetDisplayKey("timer-reset");
            timerText.UpdateText();
        }

        // Start the timer
        isTiming = true;
    }

    void Update()
    {
        if (isTiming)
        {
            elapsedTime += Time.deltaTime;
            string minutes = Mathf.Floor(elapsedTime / 60).ToString("00");
            string seconds = (elapsedTime % 60).ToString("00");

            timerText.SetDisplayKey("timer-countdown");
            timerText.UpdateDynamicText(minutes, seconds);
        }
    }

    public void StopTimer()
    {
        isTiming = false;
    }

    public float GetElapsedTime()
    {
        return elapsedTime;
    }
}
