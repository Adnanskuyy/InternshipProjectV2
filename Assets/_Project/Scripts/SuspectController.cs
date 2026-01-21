using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SuspectController : MonoBehaviour
{
    [Header("Data Reference")]
    public Suspect suspectData; // We will fill this via the Generator later

    [Header("UI Components")]
    public Image portraitImage;
    public TextMeshProUGUI nameText;
    public Button inspectButton; // The Black Box
    public Button deduceButton;  // The Purple Button

    // Add this so the card sets itself up as soon as you press Play
    private void Start()
    {
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

        // Hook up the buttons in code for cleanliness
        inspectButton.onClick.AddListener(OnInspectClicked);
        deduceButton.onClick.AddListener(OnDeduceClicked);
    }

    private void OnInspectClicked()
    {
        // Tell the UI Manager to show the Inspection Panel for this person
        FindObjectOfType<UIManager>().OpenInspection(suspectData);
    }

    private void OnDeduceClicked()
    {
        if (suspectData.isUser)
        {
            Debug.Log("YOU WIN! You found the user.");
        }
        else
        {
            Debug.Log("GAME OVER. That person was innocent.");
        }
    }
}