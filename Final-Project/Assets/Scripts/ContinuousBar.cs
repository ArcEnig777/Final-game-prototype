using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinuousBar : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform barTransform;

    [Range(0f,1f)]
    public float progress = 0f;
    float HP;
    float maxHP;

    public PlayerController player;
    public Image barColor;

    void Start(){
      maxHP = 0.0f;
      SetBar();
    }

    void SetBar(){
        barTransform.localScale = new Vector3(progress,1,1);
        if (progress <= 0.25f)
        {
            barColor.color = new Color(1.0f, 0.0f, 0.0f);
        }
        else if (progress <= 0.50f)
        {
            barColor.color = new Color(0.70f, 0.30f, 0.10f);
        }
        else if (progress <= 0.75f)
        {
            barColor.color = new Color(0.0f, 1.0f, 0.0f);
        }


    }
    
    void Update(){
        if (maxHP == 0.0f)
        {
            maxHP = player.getHP();
        }
        HP = player.getHP();
        progress = ((HP*100.0f)/maxHP)/100.0f;
        SetBar();
    }
}
