using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Accessibility;

public class BlackHole_HotKey_Controller : MonoBehaviour
{
    private SpriteRenderer sr;
    private KeyCode myHotKey;
    private TextMeshProUGUI myText;

    private Transform myEnemy;
    private BlackHole_Skill_Controller blackHole;

    public void SetUpHotKey(KeyCode _myNewHotKey, Transform _myEnemy, BlackHole_Skill_Controller _myBlackHole)
    {
        sr = GetComponent<SpriteRenderer>();
        myText = GetComponentInChildren<TextMeshProUGUI>();

        myEnemy = _myEnemy;
        blackHole = _myBlackHole;

        myHotKey = _myNewHotKey;
        myText.text = _myNewHotKey.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(myHotKey))
        {
            //Debug.Log("Hotkey pressed: " + myHotKey);
            blackHole.AddEnemyToList(myEnemy);

            myText.color = Color.clear;
            sr.color = Color.clear;
        }
    }

}
