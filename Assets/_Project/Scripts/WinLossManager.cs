using UnityEngine;
using TMPro;

public class WinLossManager : MonoBehaviour
{
    [Header("Data References")]
    [SerializeField] private GameSessionSO sessionData;

    [Header("UI Result Components")]
    [SerializeField] private GameObject resultPanel; // A new UI panel for the end screen
    [SerializeField] private TextMeshProUGUI resultTitleText; // "CORRECT" or "WRONG"
    [SerializeField] private TextMeshProUGUI resultDescriptionText;

    public void EvaluatePrediction()
    {
        bool foundGuilty = false;
        int guiltyIndex = -1;

        // 1. Find who the player marked as "Positive"
        for (int i = 0; i < sessionData.playerAssignedStatuses.Count; i++)
        {
            if (sessionData.playerAssignedStatuses[i] == SuspectStatus.Positive)
            {
                foundGuilty = true;
                guiltyIndex = i;
                break;
            }
        }

        // 2. Check against the "Truth"
        if (foundGuilty && sessionData.suspectsInSession[guiltyIndex].isActuallyUsingDrugs)
        {
            ShowResult(true, $"Correct! {sessionData.suspectsInSession[guiltyIndex].suspectName} was indeed using drugs.");
        }
        else
        {
            // Find the actual culprit to tell the player they failed
            string actualCulprit = "";
            foreach (var suspect in sessionData.suspectsInSession)
            {
                if (suspect.isActuallyUsingDrugs) actualCulprit = suspect.suspectName;
            }

            ShowResult(false, $"Incorrect. The actual user was {actualCulprit}. Better luck next time, Detective.");
        }
    }

    private void ShowResult(bool isWin, string message)
    {
        resultPanel.SetActive(true);
        resultTitleText.text = isWin ? "CASE CLOSED" : "CASE FAILED";
        resultTitleText.color = isWin ? Color.green : Color.red;
        resultDescriptionText.text = message;
    }

    public void RestartGame()
    {
        // Simple scene reload for WebGL
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}