using System;
using System.Collections.Generic;

[Serializable]
public class TeamData
{
    public string teamName; // Name of the team
    public string rocketDesign; // Design of the team's rocket
    public string rocketColor; // Color of the team's rocket
    public int score; // Score of the team
}

[Serializable]
public class TeamDataWrapper
{
    public List<TeamData> teams; // List of teams
}