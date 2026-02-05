using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    // 1. SINGLETON PATTERN
    public static UIManager Instance { get; private set; }

    [Header("Main Panels")]
    public GameObject mainLineupPanel;
    public GameObject inspectionPanel;
    public GameObject resultPanel;

    [Header("Inspection UI Elements")]
    public Image detailImageBox;
    public TextMeshProUGUI infoText;
    public Image suspectPortrait;
    public Button urineTestButton;

    [Header("Result UI")]
    public TextMeshProUGUI resultTitleText;
    public TextMeshProUGUI resultMessageText;

    [Header("Fader Settings")]
    public CanvasGroup faderCanvasGroup;
    private bool isTransitioning = false;

    private Suspect currentSuspect;
    private SuspectController currentController;

    private void Awake()
    {
        // Initialize Singleton
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    // 2. MODULAR PANEL SWAPPER
    public void OpenInspection(Suspect data, SuspectController controller)
    {
        if (isTransitioning) return;
        StartCoroutine(TransitionToPanel(data, controller));
    }

    private IEnumerator TransitionToPanel(Suspect data, SuspectController controller)
    {
        isTransitioning = true;
        faderCanvasGroup.blocksRaycasts = true;

        // Fade Out
        yield return StartCoroutine(Fade(1, 0.4f));

        // Data Setup
        currentSuspect = data;
        currentController = controller;

        mainLineupPanel.SetActive(false);
        inspectionPanel.SetActive(true);

        // Update Visuals
        suspectPortrait.sprite = data.mainPortrait;
        infoText.text = "Select an area to inspect...";
        detailImageBox.gameObject.SetActive(false);

        // Sync Urine Button with GameState (We'll move this logic to GameManager later)
        urineTestButton.interactable = !GameManager.Instance.IsUrineTestUsed;

        // Fade In
        yield return StartCoroutine(Fade(0, 0.4f));

        faderCanvasGroup.blocksRaycasts = false;
        isTransitioning = false;
    }

    public void CloseInspection()
    {
        if (isTransitioning) return;
        StartCoroutine(TransitionToMain());
    }

    private IEnumerator TransitionToMain()
    {
        isTransitioning = true;
        faderCanvasGroup.blocksRaycasts = true;

        yield return StartCoroutine(Fade(1, 0.4f));

        inspectionPanel.SetActive(false);
        mainLineupPanel.SetActive(true);

        yield return StartCoroutine(Fade(0, 0.4f));

        faderCanvasGroup.blocksRaycasts = false;
        isTransitioning = false;
    }

    // 3. GENERIC FADER
    private IEnumerator Fade(float targetAlpha, float duration)
    {
        float startAlpha = faderCanvasGroup.alpha;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            faderCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / duration);
            yield return null;
        }
        faderCanvasGroup.alpha = targetAlpha;
    }

    // UI BUTTON HOOKS
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

    public void ShowResult(bool isWin)
    {
        mainLineupPanel.SetActive(false);
        inspectionPanel.SetActive(false);
        resultPanel.SetActive(true);

        resultTitleText.text = isWin ? "CASE CLOSED" : "WRONG TARGET";
        resultTitleText.color = isWin ? Color.green : Color.red;
        resultMessageText.text = isWin ? "You found the user!" : "Investigation failed.";
    }

    public void OnUseUrineTest()
    {
        if (GameManager.Instance.IsUrineTestUsed) return;

        bool isPositive = currentController.isTheRealUser;
        infoText.text = isPositive ? "The test result is POSITIVE." : "The test result is NEGATIVE.";

        GameManager.Instance.UseUrineTest(); // Tell the Manager it's used
        urineTestButton.interactable = false; // Disable UI button
    }
}