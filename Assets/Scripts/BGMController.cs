using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMController : MonoBehaviour
{
    public AudioSource bgmSource;
    public AudioClip normalBGM;
    public AudioClip invincibleBGM;
    public AudioClip speedBoostBGM;


    public void SwitchToInvincibleBGM()
    {
        if (bgmSource.clip == invincibleBGM) return; // 이미 무적 BGM이면 패스

        bgmSource.Stop();
        bgmSource.clip = invincibleBGM;
        bgmSource.Play();
    }

    public void SwitchToSpeedBoostBGM()
    {
        if (bgmSource.clip == speedBoostBGM) return; // 이미 빠른 BGM이면 패스

        bgmSource.Stop();
        bgmSource.clip = speedBoostBGM;
        bgmSource.Play();
    }

    public void SwitchToNormalBGM()
    {
        if (bgmSource.clip == normalBGM) return; // 이미 기본 BGM이면 패스

        bgmSource.Stop();
        bgmSource.clip = normalBGM;
        bgmSource.Play();
    }

}
