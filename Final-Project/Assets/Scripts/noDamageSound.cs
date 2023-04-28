using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class noDamageSound : MonoBehaviour
{
    // Start is called before the first frame update
    AudioSource source;
    // Start is called before the first frame update
    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void OnEnable()
    {
        PlayerController.onNoDamage += (PlayHitSound);
        EnemyController.onNoDamage += (PlayHitSound);
    }

    void OnDisable()
    {
        PlayerController.onNoDamage -= (PlayHitSound);
        EnemyController.onNoDamage -= (PlayHitSound);
    }

    void PlayHitSound()
    {
        source.Play();
    }
}
