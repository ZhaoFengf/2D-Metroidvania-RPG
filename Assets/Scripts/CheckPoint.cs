using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private Animator anim;
    public string checkPointId;
    public bool activated;

    private void Awake() //Ô­±¾ÔÚstart
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
        activated = true;
        anim.SetBool("active", true);
    }
}
