using UnityEngine;

public class HeroSpawner : MonoBehaviour
{
    [SerializeField] private Transform hookHeroTransform;

    void Awake()
    {
        HeroSpawn();
    }

    void HeroSpawn()
    {
        for (int i = 0; i < heroTypes.Length; i++)
        {
            int num = 0;
            while (num < heroTypes[i].heroCount)
            {
                HookedHero hero = Instantiate(heroPrefabs);
                hero.Type = heroTypes[i];
                hero.ResetHero();
                num++;
            }
        }
    }

    [SerializeField]
    private HookedHero heroPrefabs;

    [SerializeField]
    private HookedHero.HeroType[] heroTypes;
}
