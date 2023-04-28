using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class heavyWalk : MonoBehaviour
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
        MouseController.onHeavyWalk += (PlayHitSound);
    }

    void OnDisable()
    {
        MouseController.onHeavyWalk -= (PlayHitSound);
    }

    void PlayHitSound()
    {
        source.Play();
    }
}
