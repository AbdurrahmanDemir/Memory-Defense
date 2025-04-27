using UnityEngine;

public class PageButton : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private RectTransform iconRT;
    [SerializeField] private GameObject label;

    [Header(" Settings ")]
    [SerializeField] private float iconMoveDistance;
    [SerializeField] private float animationDuration;

    private float iconInitialY;

    private void Awake()
    {
        iconInitialY = iconRT.anchoredPosition.y;
        label.SetActive(false);
    }

    public void Select()
    {
        label.SetActive(true);

        LeanTween.cancel(iconRT);
        LeanTween.moveY(iconRT, iconInitialY + iconMoveDistance, animationDuration);
    }

    public void Deselect()
    {
        label.SetActive(false);

        LeanTween.cancel(iconRT);
        LeanTween.moveY(iconRT, iconInitialY, animationDuration);
    }
}
