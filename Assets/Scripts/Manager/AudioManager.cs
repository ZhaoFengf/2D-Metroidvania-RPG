using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private float sfxMinmunDistance = 10;
    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;

    public bool playBGM;
    private int bgmIndex;

    private bool canPlaySFX;

    private void Awake()
    {
        if(instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;

        Invoke("AllowSFX", 1f);
    }

    private void Update()
    {
        if(!playBGM)
            StopAllBGM();
        else
        {
            if(!bgm[bgmIndex].isPlaying)
                PlayBGM(bgmIndex);
        }
    }

    //或许参照弹幕，后续在每个对象上添加audio，然后根据距离进行衰减也是一种方法。
    public void PlaySFX(int _sfxIndex, Transform _source = null)
    {
        //if (sfx[_sfxIndex].isPlaying)
        //    return;
        if (canPlaySFX == false)
            return;

        if (_source != null && Vector2.Distance(_source.position, PlayerManager.instance.player.transform.position) > sfxMinmunDistance)
            return;

        if (_sfxIndex < sfx.Length)
        {
            sfx[_sfxIndex].pitch = Random.Range(0.9f, 1.1f);
            sfx[_sfxIndex].Play();
        }
    }

    public void StopSFX(int _index) => sfx[_index].Stop();

    public void StopSFXWithTime(int _index) => StartCoroutine(DecreaseVolume(sfx[_index]));

    private IEnumerator DecreaseVolume(AudioSource _audio)
    {
        float defaultVolume = _audio.volume;

        while(_audio.volume > .1f)
        {
            _audio.volume -= _audio.volume * .2f;
            yield return new WaitForSeconds(.1f);
        }
        _audio.Stop();
        _audio.volume = defaultVolume;
    } 

    public void PlayBGM(int _bgmIndex)
    {
        bgmIndex = _bgmIndex;

        StopAllBGM();

        bgm[bgmIndex].Play();
    }

    public void PlayRandomBGM()
    {
        bgmIndex = Random.Range(0, bgm.Length);
        PlayBGM(bgmIndex);
    }

    public void StopAllBGM()
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
    }

    private void AllowSFX() => canPlaySFX = true;
}
