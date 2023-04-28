using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class bigHeal : MonoBehaviour
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
        EnemyController.onBigHeal += (PlayHitSound);
    }

    void OnDisable()
    {
        EnemyController.onBigHeal -= (PlayHitSound);
    }

    void PlayHitSound()
    {
        source.Play();
    }
}
