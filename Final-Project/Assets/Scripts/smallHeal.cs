using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class smallHeal : MonoBehaviour
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
        EnemyController.onSmallHeal += (PlayHitSound);
    }

    void OnDisable()
    {
        EnemyController.onSmallHeal -= (PlayHitSound);
    }

    void PlayHitSound()
    {
        source.Play();
    }
}
