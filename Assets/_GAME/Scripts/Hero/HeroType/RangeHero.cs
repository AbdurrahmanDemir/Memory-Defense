using System;
using UnityEngine;

public class RangeHero : Hero
{
    [Header("Settings")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletTransform;

    [Header("Action")]
    public static Action<Vector2, GameObject, HeroSO, Transform> onAngelBulletInstante;


    protected override void PerformAreaAttack()
    {
       
    }

    protected override void PerformSingleTargetAttack(GameObject target)
    {
        onAngelBulletInstante?.Invoke(bulletTransform.position, target, heroSO, bulletTransform);


        //GameObject bulletInstance = Instantiate(bullet, bulletTransform);
        //bulletInstance.transform.position = bulletTransform.position;
        //bulletInstance.GetComponent<AngelBulletController>().targetPosition=target.transform.position;
        //bulletInstance.GetComponent<AngelBulletController>().heroSO = heroSO;
    }
}
