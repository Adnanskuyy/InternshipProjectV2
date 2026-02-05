using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SuspectController : MonoBehaviour
{
    [Header("Data Reference")]
    public Suspect suspectData;

    [Header("UI Components")]
    public Image portraitImage;
    public TextMeshProUGUI nameText;
    public Button inspectButton;
    public Button deduceButton;

    [Header("Game State")]
    public bool isTheRealUser;

    private void Start()
    {
        // Automatically setup the UI if data was assigned in the Inspector
        if (suspectData != null)
        {
            Setup(suspectData);
        }
    }

    public void Setup(Suspect data)
    {
        suspectData = data;

        // Update the visuals based on the ScriptableObject data
        if (portraitImage != null) portraitImage.sprite = data.mainPortrait;
        if (nameText != null) nameText.text = data.personName;

        // 1. REFACTORED BUTTON HOOKS
        // We clear listeners to avoid memory leaks or double-triggers
        inspectButton.onClick.RemoveAllListeners();
        inspectButton.onClick.AddListener(OnInspectClicked);

        deduceButton.onClick.RemoveAllListeners();
        deduceButton.onClick.AddListener(OnDeduceClicked);
    }

    public void SetTargetStatus(bool status)
    {
        isTheRealUser = status;
    }

    private void OnInspectClicked()
    {
        // 2. USING THE SINGLETON
        // No more FindObjectOfType! Direct, fast access.
        if (UIManager.Instance != null)
        {
            UIManager.Instance.OpenInspection(suspectData, this);
        }
    }

    private void OnDeduceClicked()
    {
        // Direct call to show the result panel
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowResult(isTheRealUser);
        }
    }
}