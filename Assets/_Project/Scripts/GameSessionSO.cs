using UnityEngine;
using System.Collections.Generic;

public enum SuspectStatus
{
    Unknown,
    Negative,
    Positive
}

[CreateAssetMenu(fileName = "NewSession", menuName = "DetectiveGame/GameSession")]
public class GameSessionSO : ScriptableObject
{
    [Header("Game Configuration")]
    public List<SuspectSO> suspectsInSession; // Drag your 4 SuspectSOs here

    [Header("Live Session State")]
    public bool urineTestUsed;
    public SuspectSO currentlySelectedSuspect;

    // We store statuses in a list that matches the order of suspectsInSession
    public List<SuspectStatus> playerAssignedStatuses;

    public void ResetSession()
    {
        urineTestUsed = false;
        currentlySelectedSuspect = null;

        // Reset all statuses to Unknown
        playerAssignedStatuses = new List<SuspectStatus>();
        for (int i = 0; i < suspectsInSession.Count; i++)
        {
            playerAssignedStatuses.Add(SuspectStatus.Unknown);
        }
    }
    public bool IsReadyToPredict()
    {
        // Validation logic: Predict button can't be clicked if any are Unknown
        foreach (var status in playerAssignedStatuses)
        {
            if (status == SuspectStatus.Unknown) return false;
        }
        return true;
    }
}