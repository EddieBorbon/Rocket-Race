using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class TeamInstanceManager : MonoBehaviour
{
    public GameObject teamPrefab; // Prefab for the team instance
    public Transform teamsArea; // Area where team instances will be created
    public TeamManager teamManager; // Reference to the TeamManager
    public SpriteManager spriteManager; // Reference to the SpriteManager

    private List<TMP_Text> teamScoreTexts = new List<TMP_Text>(); // List of score texts
    private List<GameObject> teamInstances = new List<GameObject>(); // List of team instances

    public void CreateTeamInstances()
    {
        // Validate required references
        if (teamPrefab == null || teamsArea == null)
        {
            Debug.LogError("TeamPrefab or TeamsArea is not assigned in the inspector.");
            return;
        }

        if (teamManager == null || spriteManager == null)
        {
            Debug.LogError("TeamManager or SpriteManager is not assigned in the inspector.");
            return;
        }

        // Clear existing data
        teamScoreTexts.Clear();
        teamInstances.Clear();

        // Create an instance for each team
        foreach (TeamData teamData in teamManager.teamsData)
        {
            GameObject teamInstance = Instantiate(teamPrefab, teamsArea);
            teamInstances.Add(teamInstance);

            // Set team name
            TMP_Text nameTeamText = teamInstance.transform.Find("NameTeam")?.GetComponent<TMP_Text>();
            if (nameTeamText != null)
            {
                nameTeamText.text = string.IsNullOrEmpty(teamData.teamName)
                    ? "Team " + (teamManager.teamsData.IndexOf(teamData) + 1)
                    : teamData.teamName;
            }
            else
            {
                Debug.LogError("TMP_Text component for the team name not found.");
            }

            // Set rocket image
            Image rocketImage = teamInstance.transform.Find("RocketImage")?.GetComponent<Image>();
            if (rocketImage != null)
            {
                rocketImage.sprite = string.IsNullOrEmpty(teamData.rocketDesign) || string.IsNullOrEmpty(teamData.rocketColor)
                    ? spriteManager.GetRandomSprite() // Random sprite if design or color is missing
                    : spriteManager.GetRocketSprite(teamData.rocketDesign, teamData.rocketColor); // Specific sprite
            }
            else
            {
                Debug.LogError("Image component for the rocket image not found.");
            }

            // Set initial score
            TMP_Text scoreText = teamInstance.transform.Find("Score")?.GetComponent<TMP_Text>();
            if (scoreText != null)
            {
                scoreText.text = "Score : 0";
                teamScoreTexts.Add(scoreText);
            }
            else
            {
                Debug.LogError("TMP_Text component for the score not found.");
            }
        }
    }

    // Update the score of a specific team
    public void UpdateTeamScore(int teamIndex, int newScore)
    {
        if (teamIndex >= 0 && teamIndex < teamScoreTexts.Count)
        {
            teamScoreTexts[teamIndex].text = "Score : " + newScore.ToString();
        }
        else
        {
            Debug.LogError("Invalid team index.");
        }
    }

    // Get the rocket GameObject of a specific team
    public GameObject GetTeamShip(int teamIndex)
    {
        if (teamIndex >= 0 && teamIndex < teamInstances.Count)
        {
            Transform rocketImageTransform = teamInstances[teamIndex].transform.Find("RocketImage");
            if (rocketImageTransform != null)
            {
                return rocketImageTransform.gameObject;
            }
            else
            {
                Debug.LogError("RocketImage object not found in the team instance.");
                return null;
            }
        }
        else
        {
            Debug.LogError("Invalid team index.");
            return null;
        }
    }
}