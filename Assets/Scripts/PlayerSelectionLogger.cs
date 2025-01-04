using UnityEngine;

public class PlayerSelectionLogger : MonoBehaviour
{
    public TeamManager teamManager;

    public void PrintPlayerSelection()
    {
        if (teamManager.teamsData.Count == 0)
        {
            //Debug.Log("No hay equipos configurados.");
            return;
        }

        //Debug.Log("Selección del jugador:");
        for (int i = 0; i < teamManager.teamsData.Count; i++)
        {
            TeamData team = teamManager.teamsData[i];
          /*  Debug.Log($"Equipo {i + 1}:");
            Debug.Log($"- Nombre: {team.teamName}");
            Debug.Log($"- Diseño del cohete: {team.rocketDesign}");
            Debug.Log($"- Color del cohete: {team.rocketColor}");
            Debug.Log("-----------------------------");*/
        }
    }
}