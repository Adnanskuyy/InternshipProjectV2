using UnityEngine;
using UnityEngine.InputSystem; // Required for New Input System

public class InputManager : MonoBehaviour
{
    private GameControls controls; // This is the class generated from your Asset

    private void Awake()
    {
        controls = new GameControls(); // Initialize the generated class
    }

    private void OnEnable()
    {
        controls.Player.Enable(); // Start listening for inputs
        controls.Player.Click.performed += OnClickPerformed; // Subscribe to the event
    }

    private void OnDisable()
    {
        controls.Player.Click.performed -= OnClickPerformed; // Clean up to avoid memory leaks
        controls.Player.Disable(); // Stop listening
    }

    private void OnClickPerformed(InputAction.CallbackContext context)
    {
        // This is where the magic happens!
        // For UI buttons, Unity's EventSystem usually handles the "Click"
        // But we use this to detect if the player pressed 'Escape' or 'Back'
        Debug.Log("Pointer Click Detected!");
    }
}