using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer sr;

    [Header("Flash FX")]
    [SerializeField] private Material hitMat;
    [SerializeField] private float flashDuration = 0.1f;
    private Material originMat;

    [Header("Ailment colors")]
    [SerializeField] private Color[] chillColor;
    [SerializeField] private Color[] igniteColor;
    [SerializeField] private Color[] shockColor;
     
    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originMat = sr.material;
    }

    public void MakeTransprent(bool _transprent)
    {
        if (_transprent)
            sr.color = Color.clear;
        else
            sr.color = Color.white;
    }

    private IEnumerator FlashFX()
    {
        sr.material = hitMat;
        Color currentColor = sr.color;
        sr.color = Color.white;

        yield return new WaitForSeconds(flashDuration);
        
        sr.color = currentColor;
        sr.material = originMat;
    }

    private void ActiveRedBlink()
    {
        if(sr.color != Color.white)
            sr.color = Color.white;
        else
            sr.color = Color.red;
    }

    private void CancelColorChange()
    {
        //CancelInvoke("ActiveRedBlink");
        CancelInvoke();
        sr.color = Color.white;
    }

    public void IgniteFxFor(float _seconds)
    {
        InvokeRepeating("IgniteColorFX", 0, 0.3f);
        Invoke("CancelColorChange", _seconds);
    }

    public void ChillFxFor(float _seconds)
    {
        InvokeRepeating("ChillColorFX", 0, 0.3f);
        Invoke("CancelColorChange", _seconds);
    }

    public void ShockFxFor(float _seconds)
    {
        InvokeRepeating("ShockColorFX", 0, 0.3f);
        Invoke("CancelColorChange", _seconds);
    }

    private void IgniteColorFX()
    {
        if(sr.color != igniteColor[0])
            sr.color = igniteColor[0];
        else
            sr.color = igniteColor[1];
    }

    private void ChillColorFX()
    {
        if (sr.color != chillColor[0])
            sr.color = chillColor[0];
        else
            sr.color = chillColor[1];
    }

    private void ShockColorFX()
    {
        if(sr.color != shockColor[0])
            sr.color = shockColor[0];
        else
            sr.color = shockColor[1];
    }
}
