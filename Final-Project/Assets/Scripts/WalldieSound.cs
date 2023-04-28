using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WalldieSound : MonoBehaviour
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
        EnemyController.onWallDeath += (PlayHitSound);
    }

    void OnDisable()
    {
        EnemyController.onWallDeath -= (PlayHitSound);
    }

    void PlayHitSound()
    {
        source.Play();
    }
}
