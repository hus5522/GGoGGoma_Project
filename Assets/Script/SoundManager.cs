using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public static AudioClip bgm_floor2_Sound, bgm_floor3_Sound, bgm_floor4_Sound, shootGunSound, tickSound, drawCardSound;
    static AudioSource audioSrc;

	// Use this for initialization
	void Start () {
        bgm_floor2_Sound = Resources.Load<AudioClip>("BGM_Floor2");
        bgm_floor3_Sound = Resources.Load<AudioClip>("BGM_Floor3");
        bgm_floor4_Sound = Resources.Load<AudioClip>("BGM_Floor4");
        shootGunSound = Resources.Load<AudioClip>("Gun");
        tickSound = Resources.Load<AudioClip>("Tick");
        drawCardSound = Resources.Load<AudioClip>("DrawCard");

        audioSrc = GetComponent<AudioSource>();
        
    }

    public static void PlaySound(string clip)
    {
        switch (clip)
        {
            case "bgm_floor2":
                audioSrc.PlayOneShot(bgm_floor2_Sound);
                break;
            case "bgm_floor3":
                audioSrc.PlayOneShot(bgm_floor3_Sound);
                break;
            case "bgm_floor4":
                audioSrc.PlayOneShot(bgm_floor4_Sound);
                break;
            case "shootGun":
                audioSrc.PlayOneShot(shootGunSound);
                break;
            case "tick":
                audioSrc.PlayOneShot(tickSound);
                break;
            case "drawCard":
                audioSrc.PlayOneShot(drawCardSound);
                break;
        }
    }

}
