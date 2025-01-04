using UnityEngine;
using UnityEngine.UI;

public class PlaySoundButton : MonoBehaviour
{
    // Reference to the AudioManager for playing sounds
    public AudioManager audioManager;

    // Reference to the GameManager to access selectedRoundTimer and mode
    public GameManager gameManager;

    // Reference to the Timer
    public Timer timer;

    // Method called when the button is clicked
    public void OnPlaySoundButtonClicked()
    {
        // Validate references
        if (audioManager == null)
        {
            Debug.LogError("AudioManager is not assigned in the inspector.");
            return;
        }

        if (gameManager == null)
        {
            Debug.LogError("GameManager is not assigned in the inspector.");
            return;
        }

        if (timer == null)
        {
            Debug.LogError("Timer is not assigned in the inspector.");
            return;
        }

        // Play a sound based on the selected mode (0 = Chords, 1 = Instruments)
        int modeIndex = gameManager.modeDropdown.value;
        audioManager.PlaySound(modeIndex);

        // Start the timer with the user-selected duration
        timer.StartTimer(gameManager.selectedRoundTimer);
    }
}