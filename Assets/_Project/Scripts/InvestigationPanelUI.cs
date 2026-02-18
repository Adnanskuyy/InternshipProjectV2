using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InvestigationPanelUI : MonoBehaviour
{
    [Header("Data References")]
    [SerializeField] private GameSessionSO sessionData;

    [Header("UI Display Components")]
    [SerializeField] private Image portraitImage;        // Left side
    [SerializeField] private Image investigationImage;   // Right side (The "Man Image" placeholder)
    [SerializeField] private TextMeshProUGUI infoText;   // The white text box

    [Header("Buttons")]
    [SerializeField] private Button urineTestButton;

    // Called by UINavigationManager when opening this panel
    public void RefreshDisplay()
    {
        SuspectSO current = sessionData.currentlySelectedSuspect;

        // 1. Setup Initial View
        portraitImage.sprite = current.portrait;
        investigationImage.gameObject.SetActive(false); // Hide right image by default
        infoText.text = $"Investigating {current.suspectName}... Select a category below.";

        // 2. Handle Urine Test Button State
        // If the test was already used this session, lock it
        urineTestButton.interactable = !sessionData.urineTestUsed;
    }

    // --- Button Actions ---

    public void OnCheckBodyClicked()
    {
        SuspectSO current = sessionData.currentlySelectedSuspect;
        investigationImage.sprite = current.bodyImage;
        investigationImage.gameObject.SetActive(true); // Show image
        infoText.text = current.bodyDescription;
    }

    public void OnCheckStuffClicked()
    {
        SuspectSO current = sessionData.currentlySelectedSuspect;
        investigationImage.sprite = current.stuffImage;
        investigationImage.gameObject.SetActive(true); // Show image
        infoText.text = current.stuffDescription;
    }

    public void OnCheckRumorClicked()
    {
        SuspectSO current = sessionData.currentlySelectedSuspect;
        investigationImage.gameObject.SetActive(false); // Hide image
        infoText.text = current.rumorText;
    }

    public void OnUrineTestClicked()
    {
        if (sessionData.urineTestUsed) return;

        SuspectSO current = sessionData.currentlySelectedSuspect;
        sessionData.urineTestUsed = true; // Mark as used for the whole session

        investigationImage.gameObject.SetActive(false); // Hide image

        // Reveal the truth based on the SO data
        infoText.text = current.isActuallyUsingDrugs
            ? current.urineTestResultPositive
            : current.urineTestResultNegative;

        // Disable button immediately
        urineTestButton.interactable = false;
    }
}