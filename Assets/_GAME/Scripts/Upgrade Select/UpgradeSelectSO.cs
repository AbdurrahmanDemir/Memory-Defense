using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade Select", menuName = "New Item")]

public class UpgradeSelectSO : ScriptableObject
{
    [Header("Upgrade Info")]
    public UpgradeType upgradeType;
    public Sprite upgradeIcon;
    public string upgradeName;
    [TextArea] public string upgradeDescription;
    public int amount;
}

public enum UpgradeType
{
    TowerHealth=0,
    HookLenght=1,
    HookStrenght=2,
    EpicHeroCount=3,
    LegendaryHeroCount=4,
    DamageUpgrade=5,
    HealthUpgrade=6,
    TokenAdd=7
}
