using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject characterUI;//不清楚啥时候添加的

    public UIItemToolTip itemToolTip;
    
    void Start()
    {
        itemToolTip = GetComponentInChildren<UIItemToolTip>(true);//加true表示允许检查非活跃对象
    }

    void Update()
    {
        
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
}
