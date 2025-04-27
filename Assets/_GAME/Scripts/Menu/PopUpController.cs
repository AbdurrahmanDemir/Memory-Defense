using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class PopUpController : MonoBehaviour
{
    public static PopUpController instance;

    [Header("PopUp")]
    [SerializeField] private GameObject popUpPrefabs;
    [SerializeField] private TextMeshProUGUI popUpPrefabsText;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    public void OpenPopUp(string text)
    {
        if (popUpPrefabs.activeSelf)
        {
            popUpPrefabs.transform.DOScale(Vector3.zero, 0.2f)
                .SetEase(Ease.InBack)
                .OnComplete(() => popUpPrefabs.SetActive(false));
        }
        else
        {
            popUpPrefabs.SetActive(true);
            popUpPrefabs.transform.localScale = Vector3.zero;
            popUpPrefabsText.text = text;
            popUpPrefabs.transform.DOScale(Vector3.one, 0.2f)
                .SetEase(Ease.OutBack)
                .OnComplete(() => StartCoroutine(ClosePanelAfterDelay(1.8f)));
        }

    }

    private IEnumerator ClosePanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (popUpPrefabs.activeSelf)
        {
            popUpPrefabs.transform.DOScale(Vector3.zero, 0.2f)
                .SetEase(Ease.InBack)
                .OnComplete(() => popUpPrefabs.SetActive(false));
        }
    }
}

