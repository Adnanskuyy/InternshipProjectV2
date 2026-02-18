using UnityEngine;

[CreateAssetMenu(fileName = "Suspect_", menuName = "DetectiveGame/SuspectData")]
public class SuspectSO : ScriptableObject
{
    [Header("Basic Information")]
    public string suspectName;
    public Sprite portrait;

    [Header("Visual Evidence (Images Required)")]
    public Sprite bodyImage;
    [TextArea(3, 10)] public string bodyDescription;

    public Sprite stuffImage;
    [TextArea(3, 10)] public string stuffDescription;

    [Header("Intangible Evidence (Text Only)")]
    [TextArea(3, 10)] public string rumorText;

    [Header("The Secret Truth")]
    public bool isActuallyUsingDrugs;

    [Header("Urine Test Feedback")]
    [TextArea(2, 5)] public string urineTestResultPositive = "The test results are positive for prohibited substances.";
    [TextArea(2, 5)] public string urineTestResultNegative = "The test results are negative.";
}