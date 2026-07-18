using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UISKillToolTip : UIToolTip
{
    [SerializeField] private TextMeshProUGUI skillDes;
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillCost;


    public void ShowToolTip(string _skillDescription, string _skillName, int _prize)
    {
        skillName.text = _skillName;
        skillDes.text = _skillDescription;
        skillCost.text = "Cost: " + _prize;

        AdjustPosition();

        gameObject.SetActive(true);
    }
    public void HideToolTip()
    {
        gameObject.SetActive(false);
    }
}
