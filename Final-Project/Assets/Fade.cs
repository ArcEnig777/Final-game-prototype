using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
     public float time = 1;
    void Start(){
        StartCoroutine(FadeInMusicRoutine());
    }

    IEnumerator FadeInMusicRoutine(){
        AudioSource bgm = GetComponent<AudioSource>();
        float maxV = bgm.volume;
        float t = 0;
        bgm.volume = 0;
        bgm.Play();
        while(t<time){
            t+=Time.deltaTime;
            yield return null;
            bgm.volume = Mathf.Lerp(0,maxV,t/time);
        }
        bgm.volume = maxV;
        yield return null;
    }
}
