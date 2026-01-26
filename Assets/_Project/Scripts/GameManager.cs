using System.Collections.Generic; // This fixes the List error!
using UnityEngine;
using System.Collections; // Required for Coroutines

public class GameManager : MonoBehaviour
{
    [Header("Manual Setup")]
    public List<SuspectController> cards;
    public int targetUserIndex;

    [Header("FTUX References")]
    public CanvasGroup briefingCanvasGroup; // Drag BriefingOverlay here
    public GameObject briefingOverlayObject;

    private const string FirstTimeKey = "HasSeenBriefing";

    void Start()
    {
        InitializeGame();
        HandleBriefing();
    }

    private void HandleBriefing()
    {
        // Check if the player has played before
        if (PlayerPrefs.GetInt(FirstTimeKey, 0) == 0)
        {
            briefingOverlayObject.SetActive(true);
            briefingCanvasGroup.alpha = 1f; // Ensure it's visible
        }
        else
        {
            briefingOverlayObject.SetActive(false);
        }
    }

    // Called by the "Begin Investigation" Button
    public void StartMission()
    {
        StartCoroutine(FadeOutBriefing());

        // Save the breadcrumb
        PlayerPrefs.SetInt(FirstTimeKey, 1);
        PlayerPrefs.Save();
    }

    private IEnumerator FadeOutBriefing()
    {
        float duration = 0.5f; // Half a second fade
        float currentTime = 0f;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            briefingCanvasGroup.alpha = Mathf.Lerp(1f, 0f, currentTime / duration);
            yield return null; // Wait for next frame
        }

        briefingOverlayObject.SetActive(false);
    }

    public void InitializeGame()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            bool isTarget = (i == targetUserIndex);
            if (cards[i] != null) cards[i].SetTargetStatus(isTarget);
        }
    }
}