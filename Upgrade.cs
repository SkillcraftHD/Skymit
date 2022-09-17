using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade", menuName = "Upgrade")]
public class Upgrade : ScriptableObject
{
    [TextArea(1, 3)]
    public string description;
    public int cost;
    public int secondCost;
    public int thirdCost;
    public int costGain;
    public int maxUpgrades;

    [HideInInspector]
    public int upgradesDone = 0;

    public Sprite sprite;
}