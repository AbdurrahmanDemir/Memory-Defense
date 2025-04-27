using System;
using System.Collections;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class IceGolemHero : Hero
{
    [Header("Settings")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletTransform;

    [Header("Action")]
    public static Action<Vector2, GameObject, HeroSO, Transform> onIceGolemBulletInstante;
    public static Action<Vector2> onIceParticle;


    bool onDamage;
    protected override void PerformSingleTargetAttack(GameObject target)
    {        
        onIceGolemBulletInstante?.Invoke(bulletTransform.position, target, heroSO, bulletTransform);
        onIceParticle?.Invoke(target.transform.position);

        StartCoroutine(DamageOn(target));
    }
    IEnumerator DamageOn(GameObject enemy)
    {
        IceMageSO iceMageSO = heroSO as IceMageSO;

        float cooldown = enemy.GetComponent<Enemy>().enemySO.cooldown;
        float moveSpeed = enemy.GetComponent<Enemy>().enemySO.moveSpeed;
        yield return new WaitForSeconds(iceMageSO.freezeDuration);
        onDamage = false;
        enemy.GetComponent<Enemy>().cooldown = cooldown;
        enemy.GetComponent<Enemy>().moveSpeed = moveSpeed;


        Debug.Log("Enemy cooldown:" + enemy.GetComponent<Enemy>().cooldown + "move speed: " + enemy.GetComponent<Enemy>().moveSpeed);


    }
    protected override void PerformAreaAttack()
    {
        // Alan hasarý uygulanmaz
    }
}
