using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class PredictionUIController : MonoBehaviour
{
    [Header("Data References")]
    [SerializeField] private GameSessionSO gameSession;
    [SerializeField] private UIDocument uiDocument;

    private VisualElement root;
    private Button predictButton;

    private void OnEnable()
    {
        if (uiDocument == null) uiDocument = GetComponent<UIDocument>();
        if (uiDocument == null)
        {
            Debug.LogError("PredictionUIController: No UIDocument found!");
            return;
        }

        root = uiDocument.rootVisualElement;
        if (root == null) return;

        InitializeUI();
    }

    private void InitializeUI()
    {
        // Setup Predict Button
        predictButton = root.Q<Button>("predict-button");
        if (predictButton != null)
        {
            predictButton.clicked += OnPredictClicked;
            UpdatePredictButtonState();
        }

        // Setup Cards
        if (gameSession != null && gameSession.suspectsInSession != null)
        {
            for (int i = 0; i < 4; i++) // Assuming 4 cards fixed as per layout
            {
                if (i >= gameSession.suspectsInSession.Count) break;

                var suspect = gameSession.suspectsInSession[i];
                var status = (i < gameSession.playerAssignedStatuses?.Count) 
                             ? gameSession.playerAssignedStatuses[i] 
                             : SuspectStatus.Unknown;

                // Update Name
                var nameLabel = root.Q<Label>($"name-label-{i}");
                if (nameLabel != null) nameLabel.text = suspect != null ? suspect.suspectName : "Empty";

                // Update Status
                var statusLabel = root.Q<Label>($"status-label-{i}");
                if (statusLabel != null) statusLabel.text = status.ToString().ToUpper();
                
                // Optional: Set visual feedback based on status (e.g., color)
                // For now, we stick to the text update.
            }
        }
    }

    private void UpdatePredictButtonState()
    {
        if (predictButton == null) return;
        
        bool isReady = gameSession != null && gameSession.IsReadyToPredict();
        predictButton.SetEnabled(isReady);
        predictButton.style.opacity = isReady ? 1.0f : 0.5f;
    }

    private void OnPredictClicked()
    {
        Debug.Log("PredictionUIController: Predict button clicked! Logic to be implemented.");
        // Implement prediction logic or transition here
    }

    private void OnValidate()
    {
        if (uiDocument == null) uiDocument = GetComponent<UIDocument>();
    }
}
