using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DeathFade : MonoBehaviour
{
    AudioSource source;
    // Start is called before the first frame update
    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void OnEnable()
    {
        PlayerController.onFade += (PlayHitSound);
        EnemyController.onFade += (PlayHitSound);
    }

    void OnDisable()
    {
        PlayerController.onFade -= (PlayHitSound);
        EnemyController.onFade -= (PlayHitSound);
    }

    void PlayHitSound()
    {
        source.Play();
    }
}
