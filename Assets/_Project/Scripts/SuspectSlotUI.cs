using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SuspectSlotUI : MonoBehaviour
{
    [Header("Data References")]
    [SerializeField] private GameSessionSO sessionData;
    private int mySuspectIndex; // Set during initialization

    [Header("UI Components")]
    [SerializeField] private Image portraitImg;
    [SerializeField] private TextMeshProUGUI nameText;

    [Header("Status Button UI")]
    [SerializeField] private Button statusButton;
    [SerializeField] private Image statusButtonBg;
    [SerializeField] private TextMeshProUGUI statusText;

    // We'll call this from a "MainUIController" later
    public void Initialize(int index)
    {
        mySuspectIndex = index;
        SuspectSO data = sessionData.suspectsInSession[mySuspectIndex];

        // Set basic info
        portraitImg.sprite = data.portrait;
        nameText.text = data.suspectName;

        UpdateStatusVisuals();
    }

    public void OnStatusButtonClicked()
    {
        int nextValue = ((int)sessionData.playerAssignedStatuses[mySuspectIndex] + 1) % 3;
        sessionData.playerAssignedStatuses[mySuspectIndex] = (SuspectStatus)nextValue;
        UpdateStatusVisuals();

        // ADD THIS LINE: Tells the manager to refresh the Predict button's interactable state
        Object.FindAnyObjectByType<UINavigationManager>().RefreshPredictButton();
    }

    public void UpdateStatusVisuals()
    {
        SuspectStatus current = sessionData.playerAssignedStatuses[mySuspectIndex];

        switch (current)
        {
            case SuspectStatus.Unknown:
                statusButtonBg.color = Color.white;
                statusText.text = "Unknown";
                break;
            case SuspectStatus.Negative:
                statusButtonBg.color = Color.red; // As per your requirement
                statusText.text = "Negative";
                break;
            case SuspectStatus.Positive:
                statusButtonBg.color = Color.green; // As per your requirement
                statusText.text = "Positive";
                break;
        }
    }

    public void OnSlotClicked()
    {
        // Tells the manager to open the investigation for this specific index
        Object.FindAnyObjectByType<UINavigationManager>().OpenInvestigation(mySuspectIndex);
    }
}