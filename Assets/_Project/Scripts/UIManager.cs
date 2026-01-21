using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Main Panels")]
    public GameObject mainLineupPanel;
    public GameObject inspectionPanel;

    [Header("Inspection UI Elements")]
    public Image detailImageBox;
    public TextMeshProUGUI infoText;
    public Image suspectPortrait;

    [Header("Interactive Elements")]
    // ADD THIS LINE: This allows the script to "talk" to the button
    public Button urineTestButton;

    [Header("Result UI")]
    public GameObject resultPanel;
    public TextMeshProUGUI resultTitleText;
    public TextMeshProUGUI resultMessageText;

    private Suspect currentSuspect;
    private bool globalUrineTestUsed = false;
    // 1. Add this variable at the top to track the ACTIVE controller
    private SuspectController currentController;

    public void OpenInspection(Suspect data, SuspectController controller)
    {
        currentSuspect = data;
        currentController = controller; // Remember which physical card this is

        mainLineupPanel.SetActive(false);
        inspectionPanel.SetActive(true);

        suspectPortrait.sprite = data.mainPortrait;
        infoText.text = "Select an area to inspect...";
        detailImageBox.gameObject.SetActive(false);
    }

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
        detailImageBox.gameObject.SetActive(false);
        infoText.text = currentSuspect.rumorDescription;
    }

    public void OnUseUrineTest()
    {
        if (globalUrineTestUsed) return;

        // Direct check: Is the person we are currently looking at the real user?
        bool isPositive = currentController.isTheRealUser;

        infoText.text = isPositive ? "The test result is POSITIVE." : "The test result is NEGATIVE.";

        globalUrineTestUsed = true;
        urineTestButton.interactable = false;
    }

    public void CloseInspection()
    {
        inspectionPanel.SetActive(false);
        mainLineupPanel.SetActive(true);
    }

    public void ShowResult(bool isWin)
    {
        // 1. Turn OFF everything else first
        mainLineupPanel.SetActive(false);
        inspectionPanel.SetActive(false);

        // 2. Turn ON the result panel
        resultPanel.SetActive(true);

        if (isWin)
        {
            resultTitleText.text = "CASE CLOSED";
            resultTitleText.color = Color.green;
            resultMessageText.text = "Great job! You identified the individual showing drug-use symptoms.";
        }
        else
        {
            resultTitleText.text = "WRONG TARGET";
            resultTitleText.color = Color.red;
            resultMessageText.text = "An innocent person was accused. The investigation has failed.";
        }
    }

    public void RestartGame()
    {
        // Gets the name of the current scene and loads it fresh
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}