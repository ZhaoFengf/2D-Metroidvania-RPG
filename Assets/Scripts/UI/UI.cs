using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class UI : MonoBehaviour
{
    [Header("End Screen")]
    [SerializeField] private  UIFadeScreen fadeScreen;
    [SerializeField] private GameObject endText;
    [SerializeField] private GameObject restartButton;
    [Space]

    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject skillTreeUI;
    [SerializeField] private GameObject craftUI;
    [SerializeField] private GameObject optionUI;
    [SerializeField] private GameObject inGameUI;

    public UISKillToolTip skillToolTip;
    
    public UIItemToolTip itemToolTip;
    public UIStatToolTip statToolTip;
    public UICraftWindow craftWindow;

    private void Awake()
    {
        SwitchTo(skillTreeUI);//实现在技能脚本声明事件之前在技能树槽声明事件
        fadeScreen.gameObject.SetActive(true); //添加后能够确保游戏开始时候的淡入动画
    }

    void Start()
    {
        //itemToolTip = GetComponentInChildren<UIItemToolTip>(true);//加true表示允许检查非活跃对象,现在直接在unity中指定对象

        //SwitchTo(null);
        SwitchTo(inGameUI);

        itemToolTip.gameObject.SetActive(false);
        statToolTip.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            SwitchWithKeyTo(characterUI);
        if (Input.GetKeyDown(KeyCode.B))
            SwitchWithKeyTo(craftUI);
        if (Input.GetKeyDown(KeyCode.K))
            SwitchWithKeyTo(skillTreeUI);
        if (Input.GetKeyDown(KeyCode.O))
            SwitchWithKeyTo(optionUI);
    }

    public void SwitchTo(GameObject _menu)
    {

        for (int i =0; i < transform.childCount; i++)
        {
            bool fadeScreen = transform.GetChild(i).GetComponent<UIFadeScreen>() != null;
            if(!fadeScreen)
                transform.GetChild(i).gameObject.SetActive(false);
        }

        if (_menu != null)
            _menu.SetActive(true);

    }

    public void SwitchWithKeyTo(GameObject _menu)
    {
        if(_menu != null && _menu.activeSelf)
        {
            _menu.SetActive(false);
            CheckForInGameUI();
            return;
        }
        SwitchTo(_menu);
    }

    private void CheckForInGameUI()
    {
        for(int i=0; i< transform.childCount; i++)
        {
            bool fadeScreen = transform.GetChild(i).GetComponent<UIFadeScreen>() != null; //确保将淡入淡出排除在外

            if (transform.GetChild(i).gameObject.activeSelf && !fadeScreen)
                return;
        }

        SwitchTo(inGameUI);
    }

    public void SwitchOnEndScreen()
    {
        //SwitchTo(null);
        fadeScreen.FadeOut();
        StartCoroutine(EndScreenCorutione());
    }

    IEnumerator EndScreenCorutione()
    {
        yield return new WaitForSeconds(1f);
        endText.SetActive(true);
        yield return new WaitForSeconds(1f);
        restartButton.SetActive(true);
    }

    public void RestartGameButton() => GameManager.instance.RestartScene();
}
