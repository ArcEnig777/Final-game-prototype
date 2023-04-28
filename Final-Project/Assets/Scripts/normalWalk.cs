using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class normalWalk : MonoBehaviour
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
        MouseController.onNormalWalk += (PlayHitSound);
    }

    void OnDisable()
    {
        MouseController.onNormalWalk -= (PlayHitSound);
    }

    void PlayHitSound()
    {
        source.Play();
    }
}
