using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathSFX : MonoBehaviour
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
        PlayerController.onDeath += (PlayDeathSound);
        EnemyController.onDeath += (PlayDeathSound);
    }

    void OnDisable()
    {
        PlayerController.onDeath -= (PlayDeathSound);
        EnemyController.onDeath -= (PlayDeathSound);
    }

    void PlayDeathSound()
    {
        source.Play();
    }
}
