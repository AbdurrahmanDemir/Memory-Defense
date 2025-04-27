using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;


public class IceGolemParticle : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private GameObject iceParticlePrefabs;

    [Header("Pooling")]
    private ObjectPool<GameObject> iceParticlePool;

    private void Awake()
    {
        IceGolemHero.onIceParticle += IceParticleCallBack;
    }
    private void OnDestroy()
    {
        IceGolemHero.onIceParticle -= IceParticleCallBack;
    }


    private void Start()
    {
        iceParticlePool = new ObjectPool<GameObject>(CreateFunction,
                                                      ActionOnGet,
                                                      ActionOnRelease,
                                                      ActionOnDestroy);
    }

    private GameObject CreateFunction()
    {
        return Instantiate(iceParticlePrefabs) as GameObject;
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

    private void IceParticleCallBack(Vector2 createPosition)
    {
        GameObject iceParticleInstance = iceParticlePool.Get();

        iceParticleInstance.transform.position = createPosition;

        DOTween.Sequence()
            .AppendInterval(3)
            .AppendCallback(() => iceParticlePool.Release(iceParticleInstance));
    }
}
