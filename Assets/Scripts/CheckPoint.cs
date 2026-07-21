using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private Animator anim;
    public string checkPointId;
    public bool activated;

    private void Awake() //原本在start
    {
        anim = GetComponent<Animator>();
    }

    [ContextMenu("Generate CheckPoint ID")]
    private void GenerateId()
    {
        checkPointId = System.Guid.NewGuid().ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>() != null)
        {
            ActivateCheckPiont();
        }
    }

    public void ActivateCheckPiont()
    {
        //AudioManager.instance.PlaySFX(0, transform);//可在这里设置音频
        activated = true;
        anim.SetBool("active", true);
    }
}
