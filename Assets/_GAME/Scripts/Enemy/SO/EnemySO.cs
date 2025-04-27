using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Creat Enemy")]

public class EnemySO : ScriptableObject
{
    [Header("Enemy")]
    public string enemyName;
    public Sprite enemyImage;
    [Header("Enemy Level")]
    public int enemyLevel;
    [Header("Enemy Settings")]
    public string attackType;
    public bool isAreaOfEffect;
    public int maxHealth;
    public int damage;
    public float range;
    public float moveSpeed;
    public float cooldown;

    public int GetEnemyHealth()
    {
        return maxHealth + (enemyLevel * 50);
    }
    public int GetEnemyDamage()
    {
        return damage + (enemyLevel * 5);

    }
}
