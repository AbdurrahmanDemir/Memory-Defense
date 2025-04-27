using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;

public class DeadParticleManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private GameObject deadParticlePrefabs;

    [Header("Pooling")]
    private ObjectPool<GameObject> deadParticlePool;

    private void Awake()
    {
        Enemy.onDead += BloodParticleCallBack;
    }
    private void OnDestroy()
    {
        Enemy.onDead -= BloodParticleCallBack;
    }


    private void Start()
    {
        deadParticlePool = new ObjectPool<GameObject>(CreateFunction,
                                                      ActionOnGet,
                                                      ActionOnRelease,
                                                      ActionOnDestroy);
    }

    private GameObject CreateFunction()
    {
        return Instantiate(deadParticlePrefabs) as GameObject;
    }
    private void ActionOnGet(GameObject particle)
    {
        particle.SetActive(true);
    }
    private void ActionOnRelease(GameObject particle)
    {
        particle.SetActive(false);
    }
    private void ActionOnDestroy(GameObject particle)
    {
        Destroy(particle);
    }

    private void BloodParticleCallBack(Vector2 createPosition)
    {
        GameObject bloodPaticleInstance = deadParticlePool.Get();

        bloodPaticleInstance.transform.position = createPosition;

        DOTween.Sequence()
            .AppendInterval(3)
            .AppendCallback(() => deadParticlePool.Release(bloodPaticleInstance));
    }
}
