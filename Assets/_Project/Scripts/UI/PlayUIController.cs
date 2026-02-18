using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class PlayUIController : MonoBehaviour
{
    [Header("Data References")]
    [SerializeField] private GameSessionSO sessionData;
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private VisualTreeAsset suspectCardTemplate;

    // UI Elements
    private VisualElement root;
    private VisualElement mainPanel;
    private VisualElement investigationPanel;
    private VisualElement suspectsListContainer;
    private Button predictButton;

    // Investigation Elements
    private VisualElement investigationImageDisplay;
    private Label investigationName;
    private Label evidenceDescription;
    private Button btnBody, btnCloth, btnRumor, btnUrine, btnBack;

    private void OnEnable()
    {
        var uidoc = GetComponent<UIDocument>();
        if (uidoc == null) return;
        root = uidoc.rootVisualElement;

        // Query Main Elements
        mainPanel = root.Q<VisualElement>("main-panel");
        suspectsListContainer = root.Q<VisualElement>("suspects-list");
        predictButton = root.Q<Button>("predict-button");

        // Query Investigation Panel (Instance Container)
        investigationPanel = root.Q<VisualElement>("investigation-overlay");

        if (investigationPanel == null)
        {
            Debug.LogError("PlayUIController: 'investigation-overlay' not found in UXML! Ensure <ui:Instance> has this name.");
            return;
        }

        // Query Investigation Details (Inside the instance)
        investigationImageDisplay = investigationPanel.Q<VisualElement>("evidence-view");
        investigationName = investigationPanel.Q<Label>("investigation-name");
        evidenceDescription = investigationPanel.Q<Label>("evidence-text");
        
        btnBack = investigationPanel.Q<Button>("btn-back");
        btnBody = investigationPanel.Q<Button>("btn-body");
        btnCloth = investigationPanel.Q<Button>("btn-cloth");
        btnRumor = investigationPanel.Q<Button>("btn-rumor");
        btnUrine = investigationPanel.Q<Button>("btn-urine");

        // Register Callbacks
        if (predictButton != null) predictButton.clicked += OnPredictClicked;
        
        if (btnBack != null) btnBack.clicked += CloseInvestigation;
        if (btnBody != null) btnBody.clicked += () => OnInvestigationAction("body");
        if (btnCloth != null) btnCloth.clicked += () => OnInvestigationAction("cloth");
        if (btnRumor != null) btnRumor.clicked += () => OnInvestigationAction("rumor");
        if (btnUrine != null) btnUrine.clicked += OnUrineTestClicked;

        InitializeSession();
    }

    private void Start()
    {
        FitBackgroundToCamera();

        if (sessionData == null) return;
        UpdatePredictButton();
    }

    private void FitBackgroundToCamera()
    {
        // Find the Background Quad and scale it to fit the camera
        GameObject bgObj = GameObject.Find("FluidBackground");
        if (bgObj != null && Camera.main != null)
        {
            float height = 2f * Camera.main.orthographicSize;
            float width = height * Camera.main.aspect;
            // Add a small buffer (1.2x) to ensure full coverage
            bgObj.transform.localScale = new Vector3(width * 1.2f, height * 1.2f, 1);
            bgObj.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0);
            bgObj.transform.rotation = Quaternion.identity;
        }
    }

    private void InitializeSession()
    {
        // Reset Logic
        sessionData.ResetSession();
        
        // Reset UI
        if (investigationPanel != null) investigationPanel.style.display = DisplayStyle.None;
        if (predictButton != null) predictButton.SetEnabled(false);

        GenerateCards();
    }

    private void GenerateCards()
    {
        suspectsListContainer.Clear();
        for (int i = 0; i < sessionData.suspectsInSession.Count; i++)
        {
            int index = i;
            SuspectSO data = sessionData.suspectsInSession[i];
            
            TemplateContainer card = suspectCardTemplate.Instantiate();
            card.AddToClassList("suspect-card");
            
            // Setup visual data
            VisualElement portrait = card.Q<VisualElement>("card-portrait");
            if (data.portrait != null) portrait.style.backgroundImage = new StyleBackground(data.portrait);
            
            Label nameLbl = card.Q<Label>("card-name");
            if (nameLbl != null) nameLbl.text = data.suspectName;
            
            Button statusBtn = card.Q<Button>("status-button");
            if (statusBtn != null)
            {
                statusBtn.RegisterCallback<ClickEvent>(e => e.StopPropagation());
                statusBtn.clicked += () => OnStatusClicked(index, statusBtn);
            }
            
            // Click on card body to investigate
            card.RegisterCallback<ClickEvent>(evt => {
                Debug.Log($"Card {index} clicked. Opening investigation.");
                OpenInvestigation(index);
            });

            suspectsListContainer.Add(card);
        }
    }

    private void OnStatusClicked(int index, Button btn)
    {
        int nextValue = ((int)sessionData.playerAssignedStatuses[index] + 1) % 3;
        sessionData.playerAssignedStatuses[index] = (SuspectStatus)nextValue;
        
        btn.RemoveFromClassList("status-unknown");
        btn.RemoveFromClassList("status-negative");
        btn.RemoveFromClassList("status-positive");

        switch ((SuspectStatus)nextValue)
        {
            case SuspectStatus.Unknown:
                btn.text = "UNKNOWN";
                btn.AddToClassList("status-unknown");
                break;
            case SuspectStatus.Negative:
                btn.text = "NEGATIVE";
                btn.AddToClassList("status-negative");
                break;
            case SuspectStatus.Positive:
                btn.text = "POSITIVE";
                btn.AddToClassList("status-positive");
                break;
        }

        UpdatePredictButton();
    }

    private void UpdatePredictButton()
    {
        if (predictButton == null) return;
        
        bool ready = sessionData.IsReadyToPredict();
        predictButton.SetEnabled(ready);
        if (ready) predictButton.RemoveFromClassList("disabled");
        else predictButton.AddToClassList("disabled");
    }

    private void OpenInvestigation(int index)
    {
        Debug.Log("Opening Investigation Panel for suspect " + index);
        sessionData.currentlySelectedSuspect = sessionData.suspectsInSession[index];
        SuspectSO current = sessionData.currentlySelectedSuspect;

        investigationPanel.style.display = DisplayStyle.Flex; // Show overlay
        investigationPanel.BringToFront(); // Ensure it's on top
        
        if (investigationName != null) investigationName.text = current.suspectName;
        if (evidenceDescription != null) evidenceDescription.text = "Select an action below to investigate.";
        if (investigationImageDisplay != null) investigationImageDisplay.style.backgroundImage = null; 
        
        if (btnUrine != null) btnUrine.SetEnabled(!sessionData.urineTestUsed);
    }

    private void CloseInvestigation()
    {
        investigationPanel.style.display = DisplayStyle.None;
    }

    private void OnInvestigationAction(string type)
    {
        SuspectSO current = sessionData.currentlySelectedSuspect;
        switch (type)
        {
            case "body":
                if (investigationImageDisplay != null && current.bodyImage != null) 
                    investigationImageDisplay.style.backgroundImage = new StyleBackground(current.bodyImage);
                if (evidenceDescription != null) evidenceDescription.text = current.bodyDescription;
                break;
            case "cloth":
                 if (investigationImageDisplay != null && current.stuffImage != null) 
                    investigationImageDisplay.style.backgroundImage = new StyleBackground(current.stuffImage);
                if (evidenceDescription != null) evidenceDescription.text = current.stuffDescription;
                break;
            case "rumor":
                if (investigationImageDisplay != null) investigationImageDisplay.style.backgroundImage = null;
                if (evidenceDescription != null) evidenceDescription.text = current.rumorText;
                break;
        }
    }

    private void OnUrineTestClicked()
    {
        if (sessionData.urineTestUsed) return;
        
        sessionData.urineTestUsed = true;
        SuspectSO current = sessionData.currentlySelectedSuspect;
        
        if (evidenceDescription != null) 
            evidenceDescription.text = current.isActuallyUsingDrugs ? current.urineTestResultPositive : current.urineTestResultNegative;
        
        if (btnUrine != null) btnUrine.SetEnabled(false);
    }

    private void OnPredictClicked()
    {
        SuspectSO culprit = null;
        foreach (var s in sessionData.suspectsInSession)
        {
            if (s.isActuallyUsingDrugs)
            {
                culprit = s;
                break;
            }
        }

        bool won = false;
        // Check player prediction
        for (int i = 0; i < sessionData.suspectsInSession.Count; i++)
        {
            if (sessionData.playerAssignedStatuses[i] == SuspectStatus.Positive)
            {
                if (sessionData.suspectsInSession[i] == culprit) won = true;
                else won = false; // If they picked wrong person as positive
            }
        }

        Debug.Log(won ? "WIN: Case Closed!" : "LOSE: Wrong prediction.");
    }
}
