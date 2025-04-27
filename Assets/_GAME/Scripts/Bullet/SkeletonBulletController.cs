using DG.Tweening;
using UnityEngine;

public class SkeletonBulletController : MonoBehaviour
{
    public EnemySO enemySO;
    public Vector2 targetPosition;
    public GameObject target;


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
            .AppendCallback(() => bulletParticle.skeletonBulletPool.Release(gameObject));
    }
    private void Update()
    {
        if (target == null || isReleased) // Eðer serbest býrakýlmýþsa hareket ettirme
        {
            ReleaseBullet();
            return;
        }

        if (targetPosition != null)
            MoveTowardsTarget(targetPosition);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hero"))
        {
            collision.GetComponent<Hero>().HeroTakeDamage(enemySO.damage);
            ReleaseBullet();

        }
        else if (collision.CompareTag("Tower"))
        {
            collision.GetComponent<TowerController>().TakeDamage(enemySO.damage);
            ReleaseBullet();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Hero"))
        {
            collision.gameObject.GetComponent<Hero>().HeroTakeDamage(enemySO.damage);
            ReleaseBullet();

        }
        else if (collision.gameObject.CompareTag("Tower"))
        {
            collision.gameObject.GetComponent<TowerController>().TakeDamage(enemySO.damage);
            ReleaseBullet();

        }
    }

    private void ReleaseBullet()
    {
        if (isReleased) return;
        isReleased = true;
        Debug.Log("Bullet released: " + gameObject.name);
        bulletParticle.skeletonBulletPool.Release(gameObject);
    }
    public void ResetBullet()
    {
        isReleased = false;
        target = null;
        targetPosition = Vector2.zero;
    }


    private void MoveTowardsTarget(Vector2 targetPosition)
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, 4 * Time.deltaTime);
    }
}
