using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DiffModifier : MonoBehaviour
{
    public int HPmodifier;
    public int ATKmodifier;
    public int DEFmodifier;
    public int RNGmodifier;
    public static DiffModifier Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
    }
}
