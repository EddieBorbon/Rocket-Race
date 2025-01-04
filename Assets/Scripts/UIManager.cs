using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour
{
    // References to UI elements
    public TMP_Text roundText; // Text to display the round number
    public TMP_Text timerText; // Text to display the timer
    public GameObject playSoundButton; // "Play Sound" button
    public Button[] answerButtonsChords; // Answer buttons for Chords mode ("Major" and "Minor")
    public Button[] answerButtonsInstruments; // Answer buttons for Instruments mode (e.g., "Violin", "Piano")
    public GameObject nextTeamButton; // "Next Team" button
    public TMP_Text teamNameText; // Text to display the current team's name
    public GameObject errorPanel; // Panel to display error messages
    public TMP_Text errorMessageText; // Text to display the error message inside the panel

    // Show an error message in the error panel
    public void ShowErrorMessage(string message)
    {
        if (errorPanel != null && errorMessageText != null)
        {
            errorMessageText.text = message;
            errorPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("errorPanel or errorMessageText is not assigned in the inspector.");
        }
    }

    // Hide the error panel
    public void HideErrorMessage()
    {
        if (errorPanel != null)
        {
            errorPanel.SetActive(false);
        }
    }

    // Update the round text
    public void UpdateRoundText(string text)
    {
        if (roundText != null)
        {
            roundText.text = text;
        }
        else
        {
            Debug.LogError("roundText is not assigned in the inspector.");
        }
    }

    // Update the timer text
    public void UpdateTimerText(float time)
    {
        if (timerText != null)
        {
            timerText.text = Mathf.CeilToInt(time).ToString();
        }
        else
        {
            Debug.LogError("timerText is not assigned in the inspector.");
        }
    }

    // Activate or deactivate the "Play Sound" button
    public void SetPlaySoundButtonActive(bool isActive)
    {
        if (playSoundButton != null)
        {
            playSoundButton.SetActive(isActive);
        }
        else
        {
            Debug.LogError("playSoundButton is not assigned in the inspector.");
        }
    }

    // Activate or deactivate the answer buttons based on the selected mode
    public void SetAnswerButtonsActive(bool isActive)
    {
        int modeIndex = GetCurrentModeIndex();

        if (modeIndex == 0) // Chords mode
        {
            SetButtonsActive(answerButtonsChords, isActive, "answerButtonsChords");
            SetButtonsActive(answerButtonsInstruments, false, "answerButtonsInstruments");
        }
        else if (modeIndex == 1) // Instruments mode
        {
            SetButtonsActive(answerButtonsInstruments, isActive, "answerButtonsInstruments");
            SetButtonsActive(answerButtonsChords, false, "answerButtonsChords");
        }
        else
        {
            Debug.LogError("Invalid mode index.");
        }
    }

    // Helper method to activate or deactivate a set of buttons
    private void SetButtonsActive(Button[] buttons, bool isActive, string buttonSetName)
    {
        if (buttons != null && buttons.Length > 0)
        {
            foreach (Button button in buttons)
            {
                if (button != null)
                {
                    button.gameObject.SetActive(isActive);
                }
            }
        }
        else
        {
            Debug.LogError($"{buttonSetName} are not assigned in the inspector.");
        }
    }

    // Activate or deactivate the "Next Team" button
    public void SetNextTeamButtonActive(bool isActive)
    {
        if (nextTeamButton != null)
        {
            nextTeamButton.SetActive(isActive);
        }
        else
        {
            Debug.LogError("nextTeamButton is not assigned in the inspector.");
        }
    }

    // Show the round start text ("Round X" and "GO!")
    public IEnumerator ShowRoundStartText(int roundNumber)
    {
        UpdateRoundText($"Round {roundNumber}");
        yield return new WaitForSeconds(2f); // Show "Round X" for 2 seconds
        UpdateRoundText("GO!");
        yield return new WaitForSeconds(1f); // Show "GO!" for 1 second
        UpdateRoundText(""); // Clear the text
    }

    // Update the team name text
    public void UpdateTeamNameText(string teamName)
    {
        if (teamNameText != null)
        {
            teamNameText.text = teamName;
        }
        else
        {
            Debug.LogError("teamNameText is not assigned in the inspector.");
        }
    }

    // Switch between Chords and Instruments modes
    public void SetMode(int modeIndex)
    {
        if (modeIndex == 0) // Chords mode
        {
            SetButtonsActive(answerButtonsChords, true, "answerButtonsChords");
            SetButtonsActive(answerButtonsInstruments, false, "answerButtonsInstruments");
        }
        else if (modeIndex == 1) // Instruments mode
        {
            SetButtonsActive(answerButtonsInstruments, true, "answerButtonsInstruments");
            SetButtonsActive(answerButtonsChords, false, "answerButtonsChords");
        }
        else
        {
            Debug.LogError("Invalid mode index.");
        }
    }

    // Helper method to get the current mode index
    private int GetCurrentModeIndex()
    {
        GameManager gameManager = FindAnyObjectByType<GameManager>();
        if (gameManager != null)
        {
            return gameManager.modeDropdown.value; // 0 = Chords, 1 = Instruments
        }
        else
        {
            Debug.LogError("GameManager not found.");
            return -1;
        }
    }
}