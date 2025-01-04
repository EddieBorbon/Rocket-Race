using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class TeamManager : MonoBehaviour
{
    public GameObject teamPanelPrefab; // Prefab for the team panel
    public Transform teamsContainer; // Container for team panels
    public List<TeamData> teamsData = new List<TeamData>(); // List of team data

    // Clear all teams and their UI panels
    public void ClearTeams()
    {
        foreach (Transform child in teamsContainer)
        {
            Destroy(child.gameObject);
        }
        teamsData.Clear();
    }

    // Create UI panels for the specified number of teams
    public void CreateTeams(int numberOfTeams)
    {
        for (int i = 0; i < numberOfTeams; i++)
        {
            GameObject teamPanel = Instantiate(teamPanelPrefab, teamsContainer);

            // Set team number
            TMP_Text numberTeamText = teamPanel.transform.Find("NumberTeam")?.GetComponent<TMP_Text>();
            if (numberTeamText != null)
            {
                numberTeamText.text = "Team " + (i + 1);
            }

            // Set team name input
            TMP_InputField teamNameInput = teamPanel.transform.Find("TeamNameInput")?.GetComponent<TMP_InputField>();
            if (teamNameInput != null)
            {
                teamNameInput.placeholder.GetComponent<TMP_Text>().text = "Enter team name";
                teamNameInput.text = i < teamsData.Count ? teamsData[i].teamName : "Team " + (i + 1);
            }

            // Set rocket design dropdown
            TMP_Dropdown rocketDesignDropdown = teamPanel.transform.Find("RocketDesignDropdown")?.GetComponent<TMP_Dropdown>();
            if (rocketDesignDropdown != null)
            {
                rocketDesignDropdown.ClearOptions();
                rocketDesignDropdown.AddOptions(new List<string> { "Classic", "Futuristic", "Cartoon" });
                if (i < teamsData.Count)
                {
                    int designIndex = rocketDesignDropdown.options.FindIndex(option => option.text == teamsData[i].rocketDesign);
                    if (designIndex >= 0) rocketDesignDropdown.value = designIndex;
                }
                else
                {
                    rocketDesignDropdown.value = Random.Range(0, rocketDesignDropdown.options.Count);
                }
            }

            // Set rocket color dropdown
            TMP_Dropdown rocketColorDropdown = teamPanel.transform.Find("RocketColorDropdown")?.GetComponent<TMP_Dropdown>();
            if (rocketColorDropdown != null)
            {
                rocketColorDropdown.ClearOptions();
                rocketColorDropdown.AddOptions(new List<string> { "Red", "Blue", "Green", "Yellow" });
                if (i < teamsData.Count)
                {
                    int colorIndex = rocketColorDropdown.options.FindIndex(option => option.text == teamsData[i].rocketColor);
                    if (colorIndex >= 0) rocketColorDropdown.value = colorIndex;
                }
                else
                {
                    rocketColorDropdown.value = Random.Range(0, rocketColorDropdown.options.Count);
                }
            }

            // Add listeners to save changes
            if (teamNameInput != null) teamNameInput.onEndEdit.AddListener((value) => SaveTeams());
            if (rocketDesignDropdown != null) rocketDesignDropdown.onValueChanged.AddListener((value) => SaveTeams());
            if (rocketColorDropdown != null) rocketColorDropdown.onValueChanged.AddListener((value) => SaveTeams());
        }
    }

    // Save team data to PlayerPrefs
    public void SaveTeams()
    {
        teamsData.Clear();
        foreach (Transform teamPanel in teamsContainer)
        {
            TeamData data = new TeamData();

            TMP_InputField teamNameInput = teamPanel.transform.Find("TeamNameInput")?.GetComponent<TMP_InputField>();
            if (teamNameInput != null) data.teamName = teamNameInput.text;

            TMP_Dropdown rocketDesignDropdown = teamPanel.transform.Find("RocketDesignDropdown")?.GetComponent<TMP_Dropdown>();
            if (rocketDesignDropdown != null) data.rocketDesign = rocketDesignDropdown.options[rocketDesignDropdown.value].text;

            TMP_Dropdown rocketColorDropdown = teamPanel.transform.Find("RocketColorDropdown")?.GetComponent<TMP_Dropdown>();
            if (rocketColorDropdown != null) data.rocketColor = rocketColorDropdown.options[rocketColorDropdown.value].text;

            teamsData.Add(data);
        }

        // Save to PlayerPrefs
        string jsonData = JsonUtility.ToJson(new TeamDataWrapper { teams = teamsData });
        PlayerPrefs.SetString("TeamsData", jsonData);
        PlayerPrefs.Save();
    }

    // Load team data from PlayerPrefs
    public void LoadTeams()
    {
        if (PlayerPrefs.HasKey("TeamsData"))
        {
            string jsonData = PlayerPrefs.GetString("TeamsData");
            TeamDataWrapper wrapper = JsonUtility.FromJson<TeamDataWrapper>(jsonData);
            teamsData = wrapper.teams;
        }
    }
}