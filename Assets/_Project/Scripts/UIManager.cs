using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Main Panels")]
    public GameObject mainLineupPanel;
    public GameObject inspectionPanel;

    [Header("Inspection UI Elements")]
    public Image detailImageBox;      // The black box for eyes/hands
    public TextMeshProUGUI infoText;  // The white box for descriptions
    public Image suspectPortrait;    // The small picture of the person being inspected

    private Suspect currentSuspect;   // Tracks who we are looking at right now

    // Called by the SuspectController when you click the Black Box
    public void OpenInspection(Suspect data)
    {
        currentSuspect = data;

        // Show the panel and hide the main lineup
        mainLineupPanel.SetActive(false);
        inspectionPanel.SetActive(true);

        // Reset UI to a clean state
        suspectPortrait.sprite = data.mainPortrait;
        infoText.text = "Select an area to inspect...";
        detailImageBox.gameObject.SetActive(false); // Hide detail until a button is pressed
    }

    // Button Functions for the Inspection Screen
    public void OnInspectBody()
    {
        detailImageBox.gameObject.SetActive(true);
        detailImageBox.sprite = currentSuspect.bodyDetailImage;
        infoText.text = currentSuspect.bodyDescription;
    }

    public void OnInspectStuffs()
    {
        detailImageBox.gameObject.SetActive(true);
        detailImageBox.sprite = currentSuspect.stuffsDetailImage;
        infoText.text = currentSuspect.stuffsDescription;
    }

    public void OnInspectRumor()
    {
        detailImageBox.gameObject.SetActive(false); // Rumors are text only
        infoText.text = currentSuspect.rumorDescription;
    }

    public void OnUseUrineTest()
    {
        infoText.text = currentSuspect.GetUrineTestResult();
        // We will add logic later to disable the button after one use!
    }

    public void CloseInspection()
    {
        inspectionPanel.SetActive(false);
        mainLineupPanel.SetActive(true);
    }
}