using UnityEngine;

[System.Serializable]
public class Suspect
{
    public string personName;
    public bool isUser;

    [Header("Visuals")]
    public Sprite mainPortrait;       // The full person picture
    public Sprite bodyDetailImage;    // e.g., Picture of red eyes
    public Sprite stuffsDetailImage;  // e.g., Picture of a suspicious bag

    [Header("Clue Text")]
    [TextArea] public string bodyDescription;
    [TextArea] public string stuffsDescription;
    [TextArea] public string rumorDescription;

    public bool hasBeenTested = false;

    public string GetUrineTestResult()
    {
        return isUser ? "The test result is POSITIVE." : "The test result is NEGATIVE.";
    }
}