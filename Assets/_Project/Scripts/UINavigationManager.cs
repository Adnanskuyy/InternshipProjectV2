using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UINavigationManager : MonoBehaviour
{
    [Header("Panel References")]
    [SerializeField] private GameObject gameUIPanel;        // The 4-person list
    [SerializeField] private GameObject investigationPanel; // The detail view

    [Header("Global UI Elements")]
    [SerializeField] private Button predictButton;
    [SerializeField] private GameSessionSO sessionData;

    [Header("Suspect Slots")]
    [SerializeField] private List<SuspectSlotUI> suspectSlots;

    [SerializeField] private InvestigationPanelUI investigationUI;

    private void Start()
    {
        // 1. Reset data for a fresh start
        sessionData.ResetSession();

        // 2. Initialize the slots with data
        for (int i = 0; i < suspectSlots.Count; i++)
        {
            suspectSlots[i].Initialize(i);
        }

        // 3. Set initial view
        ShowMainPanel();
        RefreshPredictButton();
    }

    // --- Navigation Logic ---

    public void OpenInvestigation(int index)
    {
        sessionData.currentlySelectedSuspect = sessionData.suspectsInSession[index];
        gameUIPanel.SetActive(false);
        investigationPanel.SetActive(true);

        // Refresh the visuals with the new suspect's data
        investigationUI.RefreshDisplay();
    }

    public void ShowMainPanel()
    {
        gameUIPanel.SetActive(true);
        investigationPanel.SetActive(false);
        RefreshPredictButton();
    }

    // --- Validation Logic ---

    public void RefreshPredictButton()
    {
        // Uses the logic we wrote in GameSessionSO
        predictButton.interactable = sessionData.IsReadyToPredict();
    }

    // Add this to your UINavigationManager.cs
    public void CloseInvestigation()
    {
        investigationPanel.SetActive(false);
        gameUIPanel.SetActive(true);

        // CRITICAL: Refresh the Main UI slots in case statuses changed
        for (int i = 0; i < suspectSlots.Count; i++)
        {
            suspectSlots[i].UpdateStatusVisuals();
        }

        // Check if the Predict button should be enabled now
        RefreshPredictButton();
    }
}