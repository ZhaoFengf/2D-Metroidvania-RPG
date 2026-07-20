using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFadeScreen : MonoBehaviour
{
    private Animator anim;

    void Awake() //原本是start
    {
        anim = GetComponent<Animator>();
    }

    public void FadeOut()
    {
        gameObject.SetActive(true); //目前添加了这一句，使得默认将gameobject隐藏是也能够激活
        anim.SetTrigger("fadeOut");
    }
    public void FadeIn()
    {
        gameObject.SetActive(true);
        anim.SetTrigger("fadeIn");
    }
}
