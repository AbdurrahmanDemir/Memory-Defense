using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public abstract class Hero : MonoBehaviour
{
    [Header("Settings")]
    public HeroSO heroSO;
    protected float lastAttackTime = 0f;
    public LayerMask targetLayerMask;

    [Header("HeroSO")]
    protected string heroName;
    protected Sprite heroImage;
    protected string attackType;
    protected bool isAreaOfEffect;
    protected int damage;
    protected float range;
    protected float moveSpeed;
    protected float cooldown;
    protected int health;

    [Header("Elements")]
     [SerializeField] private Animator animator;
     private Slider healthSlider;
    SpriteRenderer characterSpriteRenderer;
    private Color originalColor;
    private Vector2 originalScale;
    public Vector2 scaleReduction = new Vector3(0.9f, 0.9f, 1f);

    [Header("Action")]
    private bool onThrow = false;

    private void Awake()
    {
        Hook.onThrowStarting += OnThrowStartingCallBack;
        Hook.onThrowEnding += OnThrowEndingCallBack;

        UpgradeSelectManager.heroDamageItem += PowerUpHeroDamage;
        UpgradeSelectManager.heroHealthItem += PowerUpHeroHealth;

        UpgradeSelectManager.onPowerUpPanelOpened += OnThrowStartingCallBack;
        UpgradeSelectManager.onPowerUpPanelClosed += OnThrowEndingCallBack;

        TowerController.onGameLose += OnThrowStartingCallBack;
        EnemyTowerController.onGameWin += OnThrowStartingCallBack;
    }
    private void OnDestroy()
    {
        Hook.onThrowStarting -= OnThrowStartingCallBack;
        Hook.onThrowEnding -= OnThrowEndingCallBack;

        UpgradeSelectManager.heroDamageItem -= PowerUpHeroDamage;
        UpgradeSelectManager.heroHealthItem -= PowerUpHeroHealth;

        UpgradeSelectManager.onPowerUpPanelOpened -= OnThrowStartingCallBack;
        UpgradeSelectManager.onPowerUpPanelClosed -= OnThrowEndingCallBack;

        TowerController.onGameLose -= OnThrowStartingCallBack;
        EnemyTowerController.onGameWin -= OnThrowStartingCallBack;

    }

    private void Start()
    {
        animator = GetComponent<Animator>();

        characterSpriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = characterSpriteRenderer.color;
        originalScale = transform.localScale;

        heroName = heroSO.heroName;
        heroImage = heroSO.heroImage;
        attackType = heroSO.attackType;
        isAreaOfEffect = heroSO.isAreaOfEffect;
        damage = heroSO.GetCurrentDamage();
        range = heroSO.range;
        moveSpeed = heroSO.moveSpeed;
        cooldown = heroSO.cooldown;


        healthSlider = GetComponentInChildren<Slider>();
        healthSlider.maxValue = heroSO.maxHealth;
        health = heroSO.GetCurrentHealth();
        healthSlider.value = health;
    }

    void Update()
    {
        GameObject target = FindClosestTarget();
        if (target != null)
        {
            float distanceToTarget = Vector2.Distance(transform.position, target.transform.position);

            if (onThrow)
                return;


            if (distanceToTarget <= heroSO.range)
            {
                if (target == null)
                    Debug.Log("Dusman olmustu");
                else
                    Attack(target);

            }
            else
            {
                MoveTowardsTarget(target.transform.position);
                animator.Play("run");

            }
        }
    }
    protected virtual void Attack(GameObject target)
    {

        if(Time.time- lastAttackTime>= heroSO.cooldown)
        {
            lastAttackTime = Time.time;


            if (heroSO.isAreaOfEffect)
            {
                PerformAreaAttack();
                animator.Play("attack");
            }
            else
            {
                PerformSingleTargetAttack(target);
                animator.Play("attack");
            }
        }
    }
    protected abstract void PerformSingleTargetAttack(GameObject target);
    protected abstract void PerformAreaAttack();

    protected GameObject FindClosestTarget()
    {
        GameObject closestTarget = null;
        float closestDistance = Mathf.Infinity;

        Collider2D[] potentialTargets = Physics2D.OverlapCircleAll(transform.position, 100, targetLayerMask);
        foreach (var target in potentialTargets)
        {
            float distance = Vector2.Distance(transform.position, target.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = target.gameObject;
            }
        }

        return closestTarget;
    }
    private void MoveTowardsTarget(Vector2 targetPosition)
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, heroSO.moveSpeed * Time.deltaTime);
    }

    public virtual void HeroTakeDamage(int damage)
    {
        health-=damage;
        healthSlider.value = health;

        characterSpriteRenderer.DOColor(Color.red, 0.1f).OnComplete(() =>
        {
            characterSpriteRenderer.DOColor(originalColor, 0.1f).SetDelay(0.1f);
        });
        transform.DOScale(originalScale * scaleReduction, 0.1f).OnComplete(() =>
        {
            transform.DOScale(originalScale, 0.1f);
        });


        if (health <= 0)
        {
            Debug.Log("hero öldü");
            Destroy(gameObject);

        }
    }
    public void PowerUpHeroHealth(int amount)
    {
        health += amount;
        healthSlider.value = health;
    }
    public void PowerUpHeroDamage(int amount)
    {
        damage += amount;
    }

    public void OnThrowStartingCallBack()
    {
        onThrow = true;
        Debug.Log("Avtipn çalýþtý" + onThrow);
    }
    public void OnThrowEndingCallBack()
    {
        onThrow = false;

        Debug.Log("Avtipn çalýþtý" + onThrow);

    }
}
