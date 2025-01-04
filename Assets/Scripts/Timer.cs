using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TMP_Text timerText; // Reference to the timer UI text
    private float currentTime; // Current time remaining
    private bool isTimerRunning; // Indicates if the timer is active

    // Start the timer with a specified duration
    public void StartTimer(float duration)
    {
        StopTimer(); // Stop the timer if it's already running
        currentTime = duration; // Set the current time
        UpdateTimerText(currentTime); // Update the UI
        isTimerRunning = true; // Start the timer
    }

    // Stop the timer and reset it to 0
    public void StopTimer()
    {
        isTimerRunning = false;
        currentTime = 0;
        UpdateTimerText(currentTime); // Update the UI
    }

    // Reset the timer with a new duration
    public void ResetTimer(float duration)
    {
        StartTimer(duration); // Reuse StartTimer to reset
    }

    // Update the timer UI text
    private void UpdateTimerText(float time)
    {
        if (timerText != null)
        {
            timerText.text = Mathf.CeilToInt(time).ToString(); // Round up and display
        }
        else
        {
            Debug.LogError("timerText is not assigned in the inspector.");
        }
    }

    private void Update()
    {
        if (isTimerRunning)
        {
            currentTime -= Time.deltaTime; // Decrease the time
            UpdateTimerText(currentTime); // Update the UI

            // Stop the timer if time runs out
            if (currentTime <= 0)
            {
                isTimerRunning = false;
                //Debug.Log("Time's up!"); // Optional: Add custom logic here
            }
        }
    }
}