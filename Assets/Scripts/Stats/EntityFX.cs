using System.Collections;
using TMPro;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    protected Player player;
    protected SpriteRenderer sr;

    [Header("Pop up Text")]
    [SerializeField] private GameObject popUpTextPrefab;


    [Header("Flash FX")]
    [SerializeField] private Material hitMat;
    [SerializeField] private float flashDuration = 0.1f;
    private Material originMat;

    [Header("Ailment colors")]
    [SerializeField] private Color[] chillColor;
    [SerializeField] private Color[] igniteColor;
    [SerializeField] private Color[] shockColor;

    [Header("Ailment particles")]
    [SerializeField] private ParticleSystem igniteFX;
    [SerializeField] private ParticleSystem chillFX;
    [SerializeField] private ParticleSystem shockFX;

    [Header("Hit FX")]
    [SerializeField] private GameObject hitFX;
    [SerializeField] private GameObject criticalHitFX;

    private GameObject myHealthBar;
    
     
    protected virtual void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        player = PlayerManager.instance.player;
        
        originMat = sr.material;

        myHealthBar = GetComponentInChildren<UIHealthBar>().gameObject;
    }

    private void Update()
    {
        
    }

    public void CreatePopupText(string _text)
    {
        Vector3 positionOffset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(3, 5), 0);
        GameObject newText = Instantiate(popUpTextPrefab, transform.position + positionOffset, Quaternion.identity);
        newText.GetComponent<TextMeshPro>().text = _text;
    }


    public void MakeTransprent(bool _transprent)
    {
        if (_transprent)
        {
            myHealthBar.SetActive(false);
            sr.color = Color.clear;
        }
        else
        {
            myHealthBar.SetActive(true);
            sr.color = Color.white;
        }
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

        igniteFX.Stop();
        chillFX.Stop();
        shockFX.Stop();
    }

    public void IgniteFxFor(float _seconds)
    {
        igniteFX.Play();

        InvokeRepeating("IgniteColorFX", 0, 0.3f);
        Invoke("CancelColorChange", _seconds);
    }

    public void ChillFxFor(float _seconds)
    {
        chillFX.Play();

        InvokeRepeating("ChillColorFX", 0, 0.3f);
        Invoke("CancelColorChange", _seconds);
    }

    public void ShockFxFor(float _seconds)
    {
        shockFX.Play();

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

    public void CreateHitFX(Transform _target, bool _isCritical)
    {
        float zRotation = Random.Range(-90f, 90f); //旋转特效的z角度，实现不同角度的特效方向
        float xPosition = Random.Range(-0.5f, 0.5f); 
        float yPosition = Random.Range(-0.5f, 0.5f);

        Vector3 hitFXRotation = new Vector3(0, 0, zRotation);

        GameObject hitFXPrefab = hitFX;

        if (_isCritical)
        {
            hitFXPrefab = criticalHitFX;

            float yOffset = 0;
            float zOffset = Random.Range(-45f, 45f);

            if (GetComponent<Entity>().facingDirection == -1)
                yOffset = 180;
            hitFXRotation = new Vector3(0, yOffset, zOffset);

        }

        GameObject newHitFX = Instantiate(hitFXPrefab, _target.position + new Vector3(xPosition, yPosition), Quaternion.identity);
        
        //if(!_isCritical)
        //    newHitFX.transform.Rotate(0, 0, zRotation);
        //else
        //    newHitFX.transform.localScale = new Vector3(GetComponent<Entity>().facingDirection, 1, 1);
        newHitFX.transform.Rotate(hitFXRotation);

        Destroy(newHitFX, .5f);
    }

    
}
