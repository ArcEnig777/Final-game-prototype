using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class flierWalk : MonoBehaviour
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
        MouseController.onFlierWalk += (PlayHitSound);
    }

    void OnDisable()
    {
        MouseController.onFlierWalk -= (PlayHitSound);
    }

    void PlayHitSound()
    {
        source.Play();
    }
}
