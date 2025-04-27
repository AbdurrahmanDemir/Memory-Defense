using UnityEngine;

public class MeleeEnemy : Enemy
{    
    protected override void PerformSingleTargetAttack(GameObject target)
    {
        Debug.Log($"{gameObject.name} is attacking {target.name} with single target attack for {enemySO.damage} damage!");

        animator.Play("attack");
        if (target.CompareTag("Hero"))
            target.GetComponent<Hero>().HeroTakeDamage(enemySO.damage);
        else if (target.CompareTag("Tower"))
            target.GetComponent<TowerController>().TakeDamage(enemySO.damage);
    }

    protected override void PerformAreaAttack()
    {
        // Alan hasarý uygulanmaz
    }
}
