using DG.Tweening;
using System.Collections;
using UnityEngine;

public class IceGolemBulletController : MonoBehaviour
{
    public HeroSO heroSO;
    public Vector2 targetPosition;
    public GameObject target;
    bool onDamage;


    BulletParticleManager bulletParticle;
    private bool isReleased = false;

    private void Awake()
    {
        bulletParticle = GameObject.FindGameObjectWithTag("ParticleManager").GetComponent<BulletParticleManager>();
    }
    private void Start()
    {
        DOTween.Sequence()
            .AppendInterval(1)
            .AppendCallback(() => bulletParticle.iceGolemBulletPool.Release(gameObject));
    }
    private void Update()
    {
        if (target == null || isReleased)
        {
            ReleaseBullet();
            return;
        }

        if (targetPosition != null)
            MoveTowardsTarget(targetPosition);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().HeroTakeDamage(heroSO.GetCurrentDamage());
            StartCoroutine(EnemyAttackSpeed(collision.gameObject));
        }
        else if (collision.CompareTag("EnemyTower"))
        {
            collision.GetComponent<EnemyTowerController>().TakeDamage(heroSO.GetCurrentDamage());
            StartCoroutine(EnemyAttackSpeed(collision.gameObject));
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {

            collision.gameObject.GetComponent<Enemy>().HeroTakeDamage(heroSO.GetCurrentDamage());
            StartCoroutine(EnemyAttackSpeed(collision.gameObject));

        }
        else if (collision.gameObject.CompareTag("EnemyTower"))
        {
            collision.gameObject.GetComponent<EnemyTowerController>().TakeDamage(heroSO.GetCurrentDamage());
            StartCoroutine(EnemyAttackSpeed(collision.gameObject));
        }
    }
    IEnumerator EnemyAttackSpeed(GameObject enemy)
    {
        if (!onDamage)
        {
            onDamage = true;

            float cooldown = enemy.GetComponent<Enemy>().enemySO.cooldown;
            float moveSpeed = enemy.GetComponent<Enemy>().enemySO.moveSpeed;
            enemy.GetComponent<Enemy>().cooldown = cooldown + 2;
            enemy.GetComponent<Enemy>().moveSpeed = moveSpeed - (moveSpeed * (100 / 100));

            yield return new WaitForSeconds(0.1f);

            ReleaseBullet();
            onDamage = false;
        }
    }
    //IEnumerator DamageOn(GameObject enemy)
    //{
    //    onDamage = true;
    //    float cooldown = enemy.GetComponent<Enemy>().enemySO.cooldown;
    //    float moveSpeed = enemy.GetComponent<Enemy>().enemySO.moveSpeed;
    //    enemy.GetComponent<Enemy>().cooldown = cooldown + 2;
    //    enemy.GetComponent<Enemy>().moveSpeed = moveSpeed - (moveSpeed*(100/100));

    //    Debug.Log("Enemy cooldown:" + enemy.GetComponent<Enemy>().cooldown + "move speed: " + enemy.GetComponent<Enemy>().moveSpeed);

    //    yield return new WaitForSeconds(3f);
    //    onDamage = false;
    //    enemy.GetComponent<Enemy>().cooldown = cooldown;
    //    enemy.GetComponent<Enemy>().moveSpeed = moveSpeed;

    //    Debug.Log("Enemy cooldown:" + enemy.GetComponent<Enemy>().cooldown + "move speed: " + enemy.GetComponent<Enemy>().moveSpeed);


    //}
    private void ReleaseBullet()
    {
        if (isReleased) return;
        isReleased = true;
        Debug.Log("Bullet released: " + gameObject.name);
        bulletParticle.iceGolemBulletPool.Release(gameObject);
    }
    public void ResetBullet()
    {
        isReleased = false;
        target = null;
        targetPosition = Vector2.zero;
    }


    private void MoveTowardsTarget(Vector2 targetPosition)
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, 5 * Time.deltaTime);
    }
}
