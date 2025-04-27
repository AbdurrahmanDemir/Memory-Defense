using TMPro;
using UnityEngine;
using System;

public class RangeEnemy : Enemy
{
    [Header("Settings")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletTransform;

    [Header("Action")]
    public static Action<Vector2,GameObject,EnemySO,Transform> onEnemyBulletInstante;

    protected override void PerformAreaAttack()
    {

    }

    protected override void PerformSingleTargetAttack(GameObject target)
    {

        animator.Play("attack");
        onEnemyBulletInstante?.Invoke(bulletTransform.position,target,enemySO,bulletTransform);
        //GameObject bulletInstance = Instantiate(bullet, bulletTransform);
        //bulletInstance.transform.position = bulletTransform.position;
        //bulletInstance.GetComponent<SkeletonBulletController>().targetPosition = target.transform.position;
        //bulletInstance.GetComponent<SkeletonBulletController>().enemySO = enemySO;
    }
}
