using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

    [RequireComponent(typeof(MenuUIManager))]
public class MenuBarButtons : MonoBehaviour
{

        [Header(" Elements ")]
        [SerializeField] private Transform pageButtonsParent;
        private MenuUIManager uiManager;

        [Header(" Settings ")]
        [SerializeField] private Vector2 minMaxWidth;
        [SerializeField] private float animationDuration;
        int currentPageIndex = -1;

        private void Awake()
        {
            uiManager = GetComponent<MenuUIManager>();

            // Add Callbacks to the page buttons
            for (int i = 0; i < pageButtonsParent.childCount; i++)
            {
                int _i = i;
                pageButtonsParent.GetChild(i).GetComponent<Button>().onClick.AddListener(() => uiManager.ShowPage(_i));
            }

            uiManager.pageChanged += OnPageChanged;
        }

        private void OnDestroy()
        {
            uiManager.pageChanged -= OnPageChanged;
        }

        private void OnPageChanged(int pageIndex)
        {
            // We clicked on the same page
            if (pageIndex == currentPageIndex)
                return;

            for (int i = 0; i < pageButtonsParent.childCount; i++)
            {
                LayoutElement element = pageButtonsParent.GetChild(i).GetComponent<LayoutElement>();
                LeanTween.cancel(element.gameObject);

                //float from = minMaxWidth.x;
                float to = minMaxWidth.y;

                if (pageIndex != i)
                {
                    //from = minMaxWidth.y;
                    to = minMaxWidth.x;
                }

                LeanTween.value(element.preferredWidth, to, animationDuration).setOnUpdate((value) =>
                UpdatePageButtonWidth(element, value));
            }

            UpdatePageButtonState(pageIndex);

            currentPageIndex = pageIndex;
        }

        private void UpdatePageButtonWidth(LayoutElement element, float width)
        {
            element.preferredWidth = width;
        }

        private void UpdatePageButtonState(int pageIndex)
        {
        pageButtonsParent.GetChild(pageIndex).GetComponent<PageButton>().Select();

        if (currentPageIndex >= 0)
            pageButtonsParent.GetChild(currentPageIndex).GetComponent<PageButton>().Deselect();
    }
    
}
