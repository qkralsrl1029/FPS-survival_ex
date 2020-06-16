using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource[] audioSourceEffects;    //효과음
    public AudioSource audioSourceBGM;          //배경음악

    public Sound[] effectSounds;                //전체 효과음
    public Sound[] bgmSounds;                   //전체 배경음악

    public string[] PlaySoundName;

    

    void Awake()
    {
        if (instance == null)   //최초실행시 자기자신 넣어줌
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);     //씬이동시에도 파괴되지 않음
        }
        else
            Destroy(this.gameObject);   //씬이동후 다시 실행되면, static으로 선언된 instance에는 이미 객체가 생성되었기 때문에 새로 만들어진 변수는 파괴됨
    } //기본적인 싱글톤 구조

    private void Start()
    {
        PlaySoundName = new string[audioSourceEffects.Length];
    }


    public void PlaySE(string _name)        //플레이 할 곡 이름을 파라매터로 받아와서
    {
        for (int i = 0; i < effectSounds.Length; i++)   //기존 저장된 사운드들과 비교
        {
            if(_name==effectSounds[i].name)
            {
                for (int j = 0; j < audioSourceEffects.Length; j++) //이름이 같은 사운드가 있다면 
                {
                    if(!audioSourceEffects[j].isPlaying)            //현재 재생중이지않은 남은 오디오소스가 있는지 찾고
                    {
                        PlaySoundName[j] = effectSounds[i].name;    //재생할 사운드를 '재생중인 사운드' 배열에 넣기
                        audioSourceEffects[j].clip = effectSounds[i].clip;  //있으면 재생후 함수 종료
                        audioSourceEffects[j].Play();
                        return;
                    }
                }
                Debug.Log("오디오소스가 꽉 찼습니다");                     //없으면 에러문구 출력

            }
        }
        Debug.Log(_name+" 등록되지 않은 곡입니다");                       //같은이름이 없으면 에러문구 출력
    }

    public void StopAllSE()     //재생중인 곡 전부 멈추기
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
            if(PlaySoundName[i]==_name)
            {
                audioSourceEffects[i].Stop();
                return;
            }
        }
    }
}

[System.Serializable]       //데이터 직렬화(클래스를 가시화)
public class Sound
{
    public string name;     //사운드이름
    public AudioClip clip;  //사운드클립
}
