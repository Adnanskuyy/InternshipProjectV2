using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    // 1. SINGLETON PATTERN
    public static GameManager Instance { get; private set; }

    [Header("Suspect Management")]
    public List<SuspectController> suspectCards;

    // 2. GAME STATE OWNERSHIP
    public bool IsUrineTestUsed { get; private set; } = false;
    private int _targetUserIndex;

    [Header("FTUX / Briefing References")]
    public CanvasGroup briefingCanvasGroup;
    public GameObject briefingOverlayObject;

    private const string FirstTimeKey = "HasSeenBriefing";

    private void Awake()
    {
        // Initialize Singleton
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    void Start()
    {
        InitializeGame();
        HandleBriefing();
    }

    // 3. CENTRALIZED LOGIC
    public void InitializeGame()
    {
        IsUrineTestUsed = false;

        // Randomly pick a target suspect for replayability
        _targetUserIndex = Random.Range(0, suspectCards.Count);

        for (int i = 0; i < suspectCards.Count; i++)
        {
            if (suspectCards[i] != null)
            {
                bool isTarget = (i == _targetUserIndex);
                suspectCards[i].SetTargetStatus(isTarget);
            }
        }
    }

    public void UseUrineTest()
    {
        IsUrineTestUsed = true;
        // The UIManager can now check GameManager.Instance.IsUrineTestUsed
    }

    private void HandleBriefing()
    {
        // Check if the player has played before using PlayerPrefs
        if (PlayerPrefs.GetInt(FirstTimeKey, 0) == 0)
        {
            briefingOverlayObject.SetActive(true);
            briefingCanvasGroup.alpha = 1f;
        }
        else
        {
            briefingOverlayObject.SetActive(false);
        }
    }

    public void StartMission()
    {
        StartCoroutine(FadeOutBriefing());
        PlayerPrefs.SetInt(FirstTimeKey, 1);
        PlayerPrefs.Save();
    }

    private IEnumerator FadeOutBriefing()
    {
        float duration = 0.5f;
        float currentTime = 0f;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            briefingCanvasGroup.alpha = Mathf.Lerp(1f, 0f, currentTime / duration);
            yield return null;
        }

        briefingOverlayObject.SetActive(false);
    }
}