using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private string sceneName = "MainScene";
    [SerializeField] private GameObject continueButton;
    [SerializeField] private UIFadeScreen fadeScreen;

    private void Start()
    {
        if(!SaveManager.instance.HasSaveData())
            continueButton.SetActive(false);
    }

    public void ContinueGame()
    {
        StartCoroutine(LoadSceneWithFadeEffect(1.5f));
    }

    public void NewGame()
    {
        SaveManager.instance.DeleteSaveData();
        StartCoroutine(LoadSceneWithFadeEffect(1.5f));
    }

    public void ExitGame()
    {
        Debug.Log("Exiting game...");
        //Application.Quit();
    }

    IEnumerator LoadSceneWithFadeEffect(float _dalay) //这个_delay需要考虑到淡入淡出动画的时间长度
    {
        fadeScreen.FadeOut();

        yield return new WaitForSeconds(_dalay);

        SceneManager.LoadScene(sceneName);
    }
}
