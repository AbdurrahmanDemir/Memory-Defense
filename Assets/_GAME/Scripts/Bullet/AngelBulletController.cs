using DG.Tweening;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AngelBulletController : MonoBehaviour
{
    public HeroSO heroSO;
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
            .AppendCallback(() => bulletParticle.angelBulletPool.Release(gameObject));
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
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().HeroTakeDamage(heroSO.GetCurrentDamage());
            ReleaseBullet();
        }
        else if (collision.CompareTag("EnemyTower"))
        {
            collision.GetComponent<EnemyTowerController>().TakeDamage(heroSO.GetCurrentDamage());
            ReleaseBullet();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().HeroTakeDamage(heroSO.GetCurrentDamage());
            ReleaseBullet();
        }
        else if (collision.gameObject.CompareTag("EnemyTower"))
        {
            collision.gameObject.GetComponent<EnemyTowerController>().TakeDamage(heroSO.GetCurrentDamage());
            ReleaseBullet();
        }
    }

    private void ReleaseBullet()
    {
        if (isReleased) return;
        isReleased = true;
        Debug.Log("Bullet released: " + gameObject.name);
        bulletParticle.angelBulletPool.Release(gameObject);
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
