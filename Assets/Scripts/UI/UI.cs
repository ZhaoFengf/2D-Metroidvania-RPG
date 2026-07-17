using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject skillTreeUI;
    [SerializeField] private GameObject craftUI;
    [SerializeField] private GameObject optionUI;

    public UISKillToolTip skillToolTip;
    
    public UIItemToolTip itemToolTip;
    public UIStatToolTip statToolTip;
    public UICraftWindow craftWindow;

    private void Awake()
    {
        SwitchTo(skillTreeUI);//实现在技能脚本声明事件之前在技能树槽声明事件
    }

    void Start()
    {
        //itemToolTip = GetComponentInChildren<UIItemToolTip>(true);//加true表示允许检查非活跃对象
        SwitchTo(null);

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
        for(int i =0; i < transform.childCount; i++)
        {
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
            return;
        }
        SwitchTo(_menu);
    }
}
