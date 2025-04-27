//using Demir.MissionSystem;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OldMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject sceneLoadingPanel;
    [SerializeField] private Slider loadingSlider;
    [SerializeField] private MenuUIManager uiManager;

    [Header("Tutorial")]
    [SerializeField] private GameObject tutorialPanel1;
    [SerializeField] private GameObject tutorialPanel2;
    [SerializeField] private GameObject tutorialPanel3;
    [SerializeField] private GameObject tutorialPanel4;
    [SerializeField] private GameObject tutorialPanel5;


    private void Start()
    {
        //MissionManager.Increment(EMissionType.Dailylogin, 1);

        if (!PlayerPrefs.HasKey("MenuTutorial"))
            TogglePanel(tutorialPanel1);
    }
    public void RunGameStart()
    {
        StartCoroutine(loadingScene("RunnerGame"));
    }
    public void BoatGameStart()
    {
        StartCoroutine(loadingScene("BoatGame"));
    }
    public void GameSceneOpen(string name)
    {
        //if(name=="RunnerGame")
        //    MissionManager.Increment(EMissionType.PlayRunnerMode, 1);
        //else if(name=="BoatGame")
        //    MissionManager.Increment(EMissionType.PlayBoatMode, 1);
        //else if (name == "BasketballGame")
        //    MissionManager.Increment(EMissionType.PlayBasketballMode, 1);

        StartCoroutine(loadingScene(name));
    }
    IEnumerator loadingScene(string arenaNumber)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(arenaNumber);

        sceneLoadingPanel.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 10f);
            loadingSlider.value = progress;
            yield return null;

        }

    }
    public void TogglePanel(GameObject panel)
    {
        if (panel.activeSelf)
        {
            panel.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack).OnComplete(() => panel.SetActive(false));
        }
        else
        {
            panel.SetActive(true);
            panel.transform.localScale = Vector3.zero;
            panel.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
        }
    }

    //TUTORIAL

    public void TutorialPanel1Off()
    {
        TogglePanel(tutorialPanel1);
        TogglePanel(tutorialPanel2);
        uiManager.ShowPage(3);
    }
    public void TutorialPanel2Off()
    {
        TogglePanel(tutorialPanel2);
        TogglePanel(tutorialPanel3);
    }
    public void TutorialPanel3Off()
    {
        TogglePanel(tutorialPanel3);
        TogglePanel(tutorialPanel4);
    }
    public void TutorialPanel4Off()
    {
        TogglePanel(tutorialPanel4);
        uiManager.ShowPage(2);

        StartCoroutine(TutorialPanel5());
        
    }
    IEnumerator TutorialPanel5()
    {
        TogglePanel(tutorialPanel5); 
        yield return new WaitForSeconds(3f);
        PlayerPrefs.SetInt("MenuTutorial", 1);
        TogglePanel(tutorialPanel5);
    }
}
