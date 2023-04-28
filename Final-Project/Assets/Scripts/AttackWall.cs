using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackWall : MonoBehaviour
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
        EnemyController.onWall += (PlayHitSound);
    }

    void OnDisable()
    {
        EnemyController.onWall -= (PlayHitSound);
    }

    void PlayHitSound()
    {
        source.Play();
    }
}
