using UnityEngine;

[CreateAssetMenu(fileName = "MenuHero", menuName = "HeroCard")]

public class MenuHeroCardSO : ScriptableObject
{
    [Header("Hero")]
    public string cardName;
    public Sprite heroIcon;
    public CardType cardType;
    public int baseUpgradeCost = 10;
    public float range;
    public int damage;
    public int health;
    public float cooldown;
    public float moveSpeed;
    public float undergroundRange;
    [Header("Other Stat")]
    public string specialStatName;
    public float specialStat;
    [TextArea] public string cardDescription;


    public int GetCurrentHealth()
    {
        return PlayerPrefs.GetInt($"{cardName}_Health", health);
    }
    public int GetCurrentDamage()
    {
        return PlayerPrefs.GetInt($"{cardName}_Damage", damage);
    }
    public int GetUpgradeCost()
    {
        return PlayerPrefs.GetInt($"{cardName}_UpgradeCost", baseUpgradeCost);
    }
    public void UpgradeHero()
    {
        int currentDamage = GetCurrentDamage();
        int upgradeCost = GetUpgradeCost();
        int currentHealth = GetCurrentHealth();

        if (DataManager.instance.TryPurchaseGold(upgradeCost))
        {
            int newDamage = currentDamage + 5;
            int newUpgradeCost = upgradeCost * 5;
            int newHealth = currentHealth + 50;

            PlayerPrefs.SetInt($"{cardName}_Damage", newDamage);
            PlayerPrefs.SetInt($"{cardName}_Health", newHealth);
            PlayerPrefs.SetInt($"{cardName}_UpgradeCost", newUpgradeCost);
            PlayerPrefs.Save();
        }
    }
}
public enum CardType
{
    Cammon=0,
    Rare=1,
    Epic=2,
    Legendary=3
}


