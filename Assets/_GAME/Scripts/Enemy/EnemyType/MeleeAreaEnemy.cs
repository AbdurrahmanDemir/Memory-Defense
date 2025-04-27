using UnityEngine;

public class MeleeAreaEnemy : Enemy
{
    protected override void PerformSingleTargetAttack(GameObject target)
    {
        
    }

    protected override void PerformAreaAttack()
    {
        Collider2D[] targetsInRange = Physics2D.OverlapCircleAll(transform.position, enemySO.range, targetLayerMask);
        foreach (var targetx in targetsInRange)
        {
            Debug.Log($"{gameObject.name} is attacking {targetx.gameObject.name} with area of effect attack for {enemySO.damage} damage!");


            animator.Play("attack");
            if (targetx.CompareTag("Hero"))
                targetx.GetComponent<Hero>().HeroTakeDamage(enemySO.damage);
            else if (targetx.CompareTag("Tower"))
                targetx.GetComponent<TowerController>().TakeDamage(enemySO.damage);
            // Alan içindeki her düþmana hasar verin
        }

    }
}
