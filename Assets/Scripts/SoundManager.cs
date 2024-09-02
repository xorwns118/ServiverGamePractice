using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name; // 곡의 이름
    public AudioClip clip; // 곡 
}

public class SoundManager : MonoBehaviour
{
    static public SoundManager instance;
    #region singleton
    // 싱글턴(singleton), 프로젝트 내에서 객체를 파괴하지 않고 유지시킴 (씬 이동시 기존에 있던 모든 하이라키의 객체가 파괴됨)
    private void Awake() // 객체 생성시 최초 시행 
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(this.gameObject);
    }
    #endregion singleton

    /* private void OnEnable() // 매번 활성화 되면 실행 (코루틴 실행 x)
    {
        
    }

    */

    public AudioSource[] audioSourceEffects;
    public AudioSource audioSourceBgm;

    public string[] PlaySoundName;

    public Sound[] effectSounds;
    public Sound[] bgmSounds;

    // Start is called before the first frame update
    void Start() // 매번 활성화 되면 실행 (코루틴 실행 o)
    {
        PlaySoundName = new string[audioSourceEffects.Length];
    }
    
    public void PlaySE(string _name)
    {
        for (int i = 0; i < effectSounds.Length; i++)
        {
            if(_name == effectSounds[i].name)
            {
                for (int j = 0; j < audioSourceEffects.Length; j++)
                {
                    if (!audioSourceEffects[j].isPlaying)
                    {
                        PlaySoundName[j] = effectSounds[i].name;
                        audioSourceEffects[j].clip = effectSounds[i].clip;
                        audioSourceEffects[j].Play();
                        return;
                    }
                }
                Debug.Log("모든 가용 AudioSorce가 사용중입니다");
                return;
            }
        }
        Debug.Log(_name + " 가 SoundManager에 등록되지 않았습니다");
    }

    public void StopAllSE()
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            audioSourceEffects[i].Stop();
        }
    }

    public void StopSE(string _name)
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            if (PlaySoundName[i] == _name)
            {
                audioSourceEffects[i].Stop();
                return;
            }
        }
        Debug.Log("재생중인 " + _name + " 사운드가 없습니다");
    }
}
