using UnityEngine;

[CreateAssetMenu(fileName = "MemoryCard", menuName = "New Memory Card")]

public class MemoryCardSO : ScriptableObject
{
    public string cardName;
    public Sprite cardImage;
    public int damage;

    public CardEffect cardEffect;
    public int effectValue;

    [TextArea] public string cardEffectDescription;


    public int baseUpgradeCost = 10;

}
public enum CardEffect
{
    None = 0,
    Hero = 1,
    PowerUp = 2,
    Health = 3,
    Coin = 4,
    Trap = 5
}