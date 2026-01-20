using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI infoDisplayBox; // Where rumors/clues appear
    public Button urineTestButton;

    // This function runs when you click a Person's Image
    public void OnInspectPerson(int index)
    {
        // Logic: Get data from SuspectGenerator for this index
        // Display physical symptoms or rumors in the infoDisplayBox
        Debug.Log("Inspecting Suspect #" + index);
    }

    // This function runs when you click the Purple "Person X" button
    public void OnDeductChoice(int index)
    {
        // Logic: Check if Suspect[index].isUser == true
        // Trigger Win or Loss screen
    }
}