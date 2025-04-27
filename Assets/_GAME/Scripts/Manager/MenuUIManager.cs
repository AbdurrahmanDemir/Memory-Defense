using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUIManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private RectTransform scrollableRect;
    public RectTransform ScrollableRect => scrollableRect;
    [SerializeField] private RectTransform pagesParent;

    [Header("Settings")]
    [SerializeField] private float pageSwapDuration;
    [SerializeField] private LeanTweenType pageSwapEasing;
    private float pageWidth;
    private int currentPageIndex = -1;

    private bool isControllable;
    public bool IsControllable=> isControllable;


    [Header("Actions")]
    public Action<int> pageChanged;
    public static Action staticPageChanged;

    IEnumerator Start()
    {
        isControllable = true;
        yield return null;

        pageWidth = pagesParent.rect.width;

        for(int i=0; i<pagesParent.childCount; i++)
        {
            RectTransform page = pagesParent.GetChild(i) as RectTransform;
            page.anchoredPosition = Vector2.right * i * pageWidth;
        }

        ShowPage(2);
    }

    public void ShowPage(int pageIndex)
    {
        if (pageIndex == currentPageIndex)
            return;

        Vector2 targetParentAnchoredPosition= Vector2.left*pageIndex*pageWidth;

        LeanTween.cancel(scrollableRect);
        LeanTween.move(scrollableRect, targetParentAnchoredPosition, pageSwapDuration).setEase(pageSwapEasing);

        //if(pagesParent.GetChild(pageIndex).TryGetComponent(out UIPage uiPage))
        //{
        //    iuPage.Show();
        //}

        pageChanged?.Invoke(pageIndex);
        staticPageChanged?.Invoke();

        currentPageIndex = pageIndex;
    }
    public void ShowNextPage()
    {
        int targetPageIndex = Mathf.Min(currentPageIndex + 1, 4);
        currentPageIndex = -1;
        ShowPage(targetPageIndex);
    }
    public void ShowPreviousPage()
    {
        int targetPageIndex = Mathf.Min(currentPageIndex + 1);
        currentPageIndex = -1;
        ShowPage(targetPageIndex);
    }
    public void BackToCurrentPage()
    {
        int currentPage = currentPageIndex;
        currentPageIndex = -1;

        ShowPage(currentPage);
    }

}
