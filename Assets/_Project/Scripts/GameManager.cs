using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [Header("Manual Setup")]
    [Tooltip("Drag your 4 SuspectCard objects from the Hierarchy here.")]
    public List<SuspectController> cards;

    [Tooltip("Which card is the user? (0 to 3)")]
    public int targetUserIndex;

    void Start()
    {
        // Safety Check: Ensure the list is not empty
        if (cards == null || cards.Count == 0)
        {
            Debug.LogError("GameManager: The 'cards' list is empty! Drag your SuspectCards into the Inspector.");
            return;
        }

        InitializeGame();
    }

    public void InitializeGame()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            // If the current index matches our target, set to true
            bool isTarget = (i == targetUserIndex);

            if (cards[i] != null)
            {
                cards[i].SetTargetStatus(isTarget);
            }
        }

        Debug.Log($"Game Started. Target is Suspect at index: {targetUserIndex}");
    }
}