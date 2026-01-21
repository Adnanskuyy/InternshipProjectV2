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
    public bool isTheRealUser; // Set manually by GameManager at Start

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
        portraitImage.sprite = data.mainPortrait;
        nameText.text = data.personName;

        // Hook up the buttons in code
        // We use RemoveAllListeners first to prevent double-triggering if Setup is called twice
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
        // Pass 'this' (the controller itself) to the UI Manager
        FindObjectOfType<UIManager>().OpenInspection(suspectData, this);
    }

    private void OnDeduceClicked()
    {
        // Professional Win/Loss Handling
        if (isTheRealUser)
        {
            Debug.Log("<color=green>CORRECT!</color> You found the user.");
            // Next Step: Trigger the Win UI Panel
        }
        else
        {
            Debug.Log("<color=red>WRONG!</color> This person was innocent.");
            // Next Step: Trigger the Game Over UI Panel
        }
    }
}