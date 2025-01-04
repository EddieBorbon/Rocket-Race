using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Team and round configuration
    public TeamManager teamManager;
    public TMP_Dropdown modeDropdown; // Dropdown for selecting mode (Chords or Instruments)
    public TMP_Dropdown teamCountDropdown;
    public TMP_Dropdown roundTimerDropdown;
    public TMP_Text gameOverText;
    public TMP_InputField numberOfRoundsInputField;
    public TMP_Text feedbackText; // Assign this in the Inspector

    public int selectedRoundTimer;
    public int selectedNumberOfRounds;

    // References to other managers
    public TeamInstanceManager teamInstanceManager;
    public PlayerSelectionLogger playerSelectionLogger;
    public AudioManager audioManager;
    public Timer timer;
    public UIManager uiManager;

    // UI panels
    public GameObject settingsPanel; // Settings panel
    public GameObject gameCanvas; // Game canvas
    public GameObject winningShip;

    // Game state
    private int currentTeamIndex;
    private bool isRoundActive;
    private int currentRound = 1;
    public float moveUpSpeed = 30f;

    private Coroutine roundStartCoroutine;

    // List of available instruments
    private List<string> instrumentList = new List<string>
    {
        "Violin", "Viola", "Tuba", "Trumpet", "Trombone", "Triangle", "Synth", "Snare", "Sax", "Organ",
        "Oboe", "Guitar", "Piano", "Kick", "Hat", "Harpsichord", "Flute", "ElectricGuitar", "ElectricPiano",
        "DrumKit", "DoubleBass", "Cymbal", "Conga", "Clarinet", "Cello", "Brass", "Bassoon", "Bansuri", "Accordion"
    };

    public Button[] instrumentButtons; // Assign the 4 buttons in the Inspector
    public TextMeshProUGUI[] instrumentButtonTexts;
    private string correctInstrument; // Variable to store the correct instrument
    private int correctInstrumentIndex; // Index of the correct instrument
    private List<string> currentInstrumentOptions; // List of current instrument options

    public Button restartButton;

    private void Start()
    {
        // Set up listeners for dropdowns and input field
        teamCountDropdown.onValueChanged.AddListener(OnTeamCountChanged);
        roundTimerDropdown.onValueChanged.AddListener(OnRoundTimerChanged);
        numberOfRoundsInputField.onValueChanged.AddListener(OnNumberOfRoundsChanged);
        modeDropdown.onValueChanged.AddListener(OnModeChanged); // Add listener for mode dropdown

        // Initialize teams and settings
        teamManager.LoadTeams();
        OnTeamCountChanged(teamCountDropdown.value);
        OnRoundTimerChanged(roundTimerDropdown.value);
        OnNumberOfRoundsChanged(numberOfRoundsInputField.text);

        // Initialize mode (default to Chords)
        OnModeChanged(modeDropdown.value);

        // Show the settings panel at the start
        settingsPanel.SetActive(true);
        gameCanvas.SetActive(false);
    }

    private void OnNumberOfRoundsChanged(string inputText)
    {
        if (int.TryParse(inputText, out int numberOfRounds))
        {
            selectedNumberOfRounds = numberOfRounds;
        }
        else
        {
            selectedNumberOfRounds = 0; // Default to 0 if input is invalid
        }
    }

    // Method to start the game (assigned to the "Play" button in the settings panel)
    public void OnPlayButtonClicked()
    {
        // Validate the number of rounds
        if (selectedNumberOfRounds <= 0)
        {
            Debug.LogError("The number of rounds is invalid.");
            uiManager.ShowErrorMessage("The number of rounds must be greater than 0.");
            return;
        }

        // Validate that teams have been created
        if (teamManager.teamsData == null || teamManager.teamsData.Count == 0)
        {
            uiManager.ShowErrorMessage("No teams have been created. Please create at least one team.");
            return;
        }

        // Hide the settings panel
        settingsPanel.SetActive(false);

        // Show the game canvas
        gameCanvas.SetActive(true);

        // Update the team name in the UI
        uiManager.UpdateTeamNameText(teamManager.teamsData[currentTeamIndex].teamName);

        // Initialize the game
        InitializeGame();
    }

    private void InitializeGame()
    {
        // Create team instances
        if (teamInstanceManager != null)
        {
            teamInstanceManager.CreateTeamInstances();
        }
        else
        {
            Debug.LogError("TeamInstanceManager is not assigned in the inspector.");
        }

        // Initialize the timer and rounds
        InitializeTimer();

        // Log the player selection
        if (playerSelectionLogger != null)
        {
            playerSelectionLogger.PrintPlayerSelection();
        }
        else
        {
            Debug.LogError("PlayerSelectionLogger is not assigned in the inspector.");
        }

        // Start the first round
        StartRound();
    }

    // Method to initialize the timer
    private void InitializeTimer()
    {
        if (timer != null)
        {
            timer.StartTimer(selectedRoundTimer);
        }
        else
        {
            Debug.LogError("Timer is not assigned in the inspector.");
        }
    }

    public void StartRound()
    {
        isRoundActive = true;

        // Play a new sound based on the selected mode
        int modeIndex = modeDropdown.value; // 0 = Chords, 1 = Instruments
        audioManager.PlaySound(modeIndex);

        // If in Instruments mode, generate random instrument options
        if (modeIndex == 1)
        {
            correctInstrument = audioManager.GetCurrentInstrument(); // Store the correct instrument
            Debug.Log($"Correct instrument set to: {correctInstrument}");
            GenerateRandomInstrumentOptions(correctInstrument);
        }

        // Show the answer buttons
        uiManager.SetAnswerButtonsActive(true);

        // Show the round number and "GO!"
        if (roundStartCoroutine != null)
        {
            StopCoroutine(roundStartCoroutine);
        }
        roundStartCoroutine = StartCoroutine(uiManager.ShowRoundStartText(currentRound));
    }

    public void OnPlaySoundButtonClicked()
    {
        // Get the correct instrument
        string correctInstrument = audioManager.GetCurrentInstrument();

        // Play the sound of the correct instrument
        audioManager.PlaySound(1); // 1 = Instruments mode

        // Generate random options
        GenerateRandomInstrumentOptions(correctInstrument);

        // Reveal the answer buttons
        uiManager.SetAnswerButtonsActive(true);
    }

    public void OnAnswerClicked(int buttonIndex)
    {
        if (!isRoundActive) return;

        bool isCorrect = false;
        int modeIndex = modeDropdown.value; // 0 = Chords, 1 = Instruments

        if (modeIndex == 0) // Chords mode
        {
            if (buttonIndex == 0) // Assume button 0 is "Major"
            {
                isCorrect = audioManager.IsMajorChordPlayed();
                Debug.Log($"User selected: Major | Correct answer: {(isCorrect ? "Major" : "Minor")}");
            }
            else if (buttonIndex == 1) // Assume button 1 is "Minor"
            {
                isCorrect = !audioManager.IsMajorChordPlayed();
                Debug.Log($"User selected: Minor | Correct answer: {(isCorrect ? "Minor" : "Major")}");
            }
        }
        else if (modeIndex == 1) // Instruments mode
        {
            // Compare the button index with the correct instrument index
            isCorrect = (buttonIndex == correctInstrumentIndex);

            // Log the result of the comparison
            Debug.Log($"User selected: {currentInstrumentOptions[buttonIndex]} | Correct instrument: {currentInstrumentOptions[correctInstrumentIndex]}");
            Debug.Log($"Comparison result: {isCorrect}");
        }

        // Provide feedback
        if (isCorrect)
        {
            Debug.Log("Correct!");
            feedbackText.text = "Correct!";
            feedbackText.color = Color.green; // Set text color to green
            teamManager.teamsData[currentTeamIndex].score++;
            teamInstanceManager.UpdateTeamScore(currentTeamIndex, teamManager.teamsData[currentTeamIndex].score);
        }
        else
        {
            Debug.Log("Incorrect!");
            feedbackText.text = "Incorrect!";
            feedbackText.color = Color.red; // Set text color to red
        }

        // Show feedback for 2 seconds
        StartCoroutine(ShowFeedback());

        // Disable the answer buttons
        uiManager.SetAnswerButtonsActive(false);
        uiManager.SetPlaySoundButtonActive(false);

        // Enable the "Next Team" button for the current mode
        uiManager.SetNextTeamButtonActive(true);
        isRoundActive = false;
    }

    // Coroutine to show feedback for 2 seconds
    private IEnumerator ShowFeedback()
    {
        feedbackText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f); // Show feedback for 2 seconds
        feedbackText.gameObject.SetActive(false);
    }

    public void NextTeam()
    {
        correctInstrument = null;
        Debug.Log("Correct instrument cleared.");

        // Increment the round number
        currentRound++;
        Debug.Log("Current round: " + currentRound);

        // Check if the game should end
        if (currentRound > selectedNumberOfRounds)
        {
            GameOver();
            return; // Exit the method to prevent starting a new round
        }

        // Move to the next team
        currentTeamIndex = (currentTeamIndex + 1) % teamManager.teamsData.Count;
        uiManager.UpdateTeamNameText(teamManager.teamsData[currentTeamIndex].teamName);

        // Enable the "Play Sound" button
        uiManager.SetPlaySoundButtonActive(true);

        // Disable the "Next Team" button
        uiManager.SetNextTeamButtonActive(false);

        // Reset the timer
        timer.ResetTimer(selectedRoundTimer);

        // Start the new round
        StartRound();
    }

    public void OnNextTeamButtonClicked()
    {
        // Move to the next team
        currentTeamIndex = (currentTeamIndex + 1) % teamManager.teamsData.Count;
        playerSelectionLogger.PrintPlayerSelection(); // Log the current team

        // Restart the question for the next team
        StartRound();
    }

    // Existing methods for initial configuration
    void OnTeamCountChanged(int selectedIndex)
    {
        int numberOfTeams = selectedIndex + 1;
        teamManager.ClearTeams();
        teamManager.CreateTeams(numberOfTeams);
    }

    void OnRoundTimerChanged(int selectedIndex)
    {
        selectedRoundTimer = int.Parse(roundTimerDropdown.options[selectedIndex].text);
        uiManager.UpdateTimerText(selectedRoundTimer); // Update the timer text
    }

    private void GameOver()
    {
        Debug.Log("Game Over triggered!");
        restartButton.gameObject.SetActive(true);

        // Show the "Game Over" message
        gameOverText.gameObject.SetActive(true);

        // Determine the winning teams (teams with the highest score)
        List<GameObject> winningShips = GetWinningTeamShips();

        if (winningShips != null && winningShips.Count > 0)
        {
            // Build the game over message with all winning teams
            string winningTeamsMessage = "Game Over\n";
            foreach (GameObject ship in winningShips)
            {
                string teamName = ship.transform.parent.Find("NameTeam").GetComponent<TMP_Text>().text;
                winningTeamsMessage += $"{teamName} Wins!\n";
            }

            // Show the names of the winning teams
            gameOverText.text = winningTeamsMessage;

            // Move all winning ships upward
            foreach (GameObject ship in winningShips)
            {
                StartCoroutine(MoveUp(ship));
            }
        }
        else
        {
            Debug.LogError("Could not determine the winning ships.");
        }
    }

    private List<GameObject> GetWinningTeamShips()
    {
        int maxScore = -1;
        List<GameObject> winningShips = new List<GameObject>();

        // Find the highest score
        for (int i = 0; i < teamManager.teamsData.Count; i++)
        {
            if (teamManager.teamsData[i].score > maxScore)
            {
                maxScore = teamManager.teamsData[i].score;
            }
        }

        // Find all teams with the highest score
        for (int i = 0; i < teamManager.teamsData.Count; i++)
        {
            if (teamManager.teamsData[i].score == maxScore)
            {
                GameObject ship = teamInstanceManager.GetTeamShip(i);
                if (ship != null)
                {
                    winningShips.Add(ship);
                }
                else
                {
                    Debug.LogError($"Could not find ship for team {i}.");
                }
            }
        }

        return winningShips;
    }

    // Coroutine to move the winning ship upward
    private IEnumerator MoveUp(GameObject ship)
    {
        while (true)
        {
            ship.transform.Translate(Vector3.up * moveUpSpeed * Time.deltaTime * 50);
            yield return null;
        }
    }

    private void OnModeChanged(int selectedIndex)
    {
        // 0 = Chords, 1 = Instruments
        uiManager.SetMode(selectedIndex); // Update the UI based on the selected mode
    }

    public void GenerateRandomInstrumentOptions(string correctInstrument)
    {
        // Create a temporary list to store the options
        currentInstrumentOptions = new List<string>();

        // Add the correct option
        currentInstrumentOptions.Add(correctInstrument);

        // Select 3 random incorrect options
        while (currentInstrumentOptions.Count < 4)
        {
            string randomInstrument = instrumentList[Random.Range(0, instrumentList.Count)];
            if (!currentInstrumentOptions.Contains(randomInstrument)) // Avoid duplicates
            {
                currentInstrumentOptions.Add(randomInstrument);
            }
        }

        // Shuffle the options so the correct one isn't always in the same position
        Shuffle(currentInstrumentOptions);

        // Save the index of the correct instrument
        correctInstrumentIndex = currentInstrumentOptions.IndexOf(correctInstrument);

        // Update the button texts with the options
        for (int i = 0; i < instrumentButtonTexts.Length; i++)
        {
            if (instrumentButtonTexts[i] != null)
            {
                instrumentButtonTexts[i].text = currentInstrumentOptions[i];
            }
            else
            {
                Debug.LogError($"The TextMeshProUGUI for button {i} is not assigned in the Inspector.");
            }
        }

        // Log the correct instrument and its index
        Debug.Log($"Correct instrument: {correctInstrument} | Index: {correctInstrumentIndex}");
    }

    // Method to shuffle a list (Fisher-Yates shuffle)
    private void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }

    // Method to restart the scene
    public void RestartGame()
    {
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}