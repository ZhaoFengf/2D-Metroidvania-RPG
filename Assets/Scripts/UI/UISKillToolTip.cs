using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UISKillToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI skillDes;
    [SerializeField] private TextMeshProUGUI skillName;


    public void ShowToolTip(string _skillDescription, string _skillName)
    {
        skillName.text = _skillName;
        skillDes.text = _skillDescription;
        gameObject.SetActive(true);
    }
    public void HideToolTip()
    {
        gameObject.SetActive(false);
    }
}
