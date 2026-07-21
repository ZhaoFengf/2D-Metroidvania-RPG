using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_FX : EntityFX
{
    [Header("After image fx")]
    [SerializeField] private GameObject afterImagePrefab;
    [SerializeField] private float colorLooseRate;
    [SerializeField] private float afterImageCooldown;
    private float afterImageCooldownTimer;

    [Header("Screen shake FX")]
    [SerializeField] private float shakeMultiplier;
    public Vector3 shakeSwordImpact;
    public Vector3 shakeHighDamage;
    private CinemachineImpulseSource screenShake;

    [Header("Dust FX")]
    [SerializeField] private ParticleSystem dustFX;

    protected override void Start()
    {
        base.Start();
        screenShake = GetComponent<CinemachineImpulseSource>();
    }

    private void Update()
    {
        afterImageCooldownTimer -= Time.deltaTime;
    }

    public void CreateAfterImage()
    {
        if (afterImageCooldownTimer > 0)
            return;

        afterImageCooldownTimer = afterImageCooldown;
        GameObject newAfterImage = Instantiate(afterImagePrefab, transform.position, transform.rotation);
        newAfterImage.GetComponent<AfterImageFX>().SetupAfterImage(colorLooseRate, sr.sprite);
    }

    public void ScreenShake(Vector3 _shakePower)
    {
        screenShake.m_DefaultVelocity = new Vector3(_shakePower.x * player.facingDirection, _shakePower.y) * shakeMultiplier;
        screenShake.GenerateImpulse();
    }

    //后续可以优化，即，对于尘埃的方向进行调整，现在的话主要就是朝着左上的
    public void PlayDustFX()
    {
        if (dustFX != null)
            dustFX.Play();
    }
}
