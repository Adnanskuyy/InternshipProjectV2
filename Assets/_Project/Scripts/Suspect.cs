using UnityEngine;

// This line allows you to right-click in Unity to create new Suspect data files!
[CreateAssetMenu(fileName = "NewSuspect", menuName = "DeductionGame/Suspect")]
public class Suspect : ScriptableObject // Change from 'class' to 'ScriptableObject'
{
    public string personName;

    [Header("Visuals")]
    public Sprite mainPortrait;
    public Sprite bodyDetailImage;
    public Sprite stuffsDetailImage;

    [Header("Clue Text")]
    [TextArea(3, 10)] public string bodyDescription;
    [TextArea(3, 10)] public string stuffsDescription;
    [TextArea(3, 10)] public string rumorDescription;

}