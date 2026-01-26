using System.Collections;
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

    [Header("Fader Settings")]
    public CanvasGroup faderCanvasGroup;
    public Animator faderAnimator;
    private bool isTransitioning = false; // The Guard

    public void CloseInspection()
    {
        if (isTransitioning) return; // Ignore clicks if already fading
        StartCoroutine(TransitionToLineup());
    }

    private IEnumerator TransitionToLineup()
    {
        isTransitioning = true; // Lock

        faderCanvasGroup.blocksRaycasts = true;

        // 1. Fade to Black
        yield return StartCoroutine(FadeCanvas(0, 1, 0.4f));

        // 2. The Switch (Done while screen is 100% black)
        inspectionPanel.SetActive(false);
        mainLineupPanel.SetActive(true);

        // Safety check: Ensure the other panel is definitely off
        if (resultPanel != null) resultPanel.SetActive(false);

        // 3. Fade to Clear
        yield return StartCoroutine(FadeCanvas(1, 0, 0.4f));

        faderCanvasGroup.blocksRaycasts = false;
        isTransitioning = false; // Unlock
    }

    private IEnumerator TransitionToInspect(Suspect data, SuspectController controller)
    {
        // 1. BLOCK CLICKS & FADE TO BLACK
        faderCanvasGroup.blocksRaycasts = true;
        yield return StartCoroutine(FadeCanvas(0, 1, 0.4f)); // Fade from 0 to 1 over 0.4s

        // 2. THE HIDDEN SWITCH (Happens while screen is 100% black)
        currentSuspect = data;
        currentController = controller;

        mainLineupPanel.SetActive(false);
        inspectionPanel.SetActive(true);

        suspectPortrait.sprite = data.mainPortrait;
        infoText.text = "Select an area to inspect...";
        detailImageBox.gameObject.SetActive(false);

        // 3. FADE BACK TO CLEAR & UNLOCK
        yield return StartCoroutine(FadeCanvas(1, 0, 0.4f));
        faderCanvasGroup.blocksRaycasts = false;
    }

    // THE WORKHORSE FUNCTION
    private IEnumerator FadeCanvas(float start, float end, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            faderCanvasGroup.alpha = Mathf.Lerp(start, end, elapsed / duration);
            yield return null;
        }
        faderCanvasGroup.alpha = end;
    }

    public void OpenInspection(Suspect data, SuspectController controller)
    {
        StartCoroutine(TransitionToInspect(data, controller));
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