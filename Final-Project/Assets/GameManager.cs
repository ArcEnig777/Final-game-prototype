using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int playerUnits;
    public int enemyUnits;
    public int Level;
    public int nextLevel;
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        Level = 1;
        nextLevel = 2;
        DontDestroyOnLoad(gameObject);
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void changeLevel()
    {
        Level += 1;
        playerUnits = 0;
        enemyUnits = 0;

        if(Level == 2)
        {
            SceneManager.LoadScene("StageTwoScene");
        }
        else
        {
            SceneManager.LoadScene("MenuScene");
        }
    }

    public void returnMenu()
    {
  
        SceneManager.LoadScene("MenuScene");
        
    }
}
