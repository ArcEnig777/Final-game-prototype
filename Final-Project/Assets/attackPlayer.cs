using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class attackPlayer : MonoBehaviour
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
        PlayerController.onAnyAttack += (PlayHitSound);
        EnemyController.onAnyAttack += (PlayHitSound);
    }

    void OnDisable()
    {
        PlayerController.onAnyAttack -= (PlayHitSound);
        EnemyController.onAnyAttack -= (PlayHitSound);
    }

    void PlayHitSound()
    {
        source.Play();
    }
}
