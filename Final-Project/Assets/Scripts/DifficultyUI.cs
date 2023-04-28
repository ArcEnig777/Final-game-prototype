using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DifficultyUI : MonoBehaviour
{
    public DiffModifier diff;
    public GameManager gameManager;

    private void Start()
    {
        diff = DiffModifier.Instance;
    }

    public void setEasy()
    {
        diff.HPmodifier = -5;
        diff.ATKmodifier = -1;
        diff.DEFmodifier = -2;

        gameManager = GameManager.Instance;

        if (gameManager !=null)
        {
            gameManager.inLevel = true;
            gameManager.enableHUD();
        }

        SceneManager.LoadScene("StageOneScene");

    }
    public void setNormal()
    {
        diff.HPmodifier = 0;
        diff.ATKmodifier = 0;
        diff.DEFmodifier = 0;

        gameManager = GameManager.Instance;

        if (gameManager !=null)
        {
            gameManager.inLevel = true;
            gameManager.enableHUD();
        }

        SceneManager.LoadScene("StageOneScene");
    }
    public void setHard()
    {
        diff.HPmodifier = 5;
        diff.ATKmodifier = 2;
        diff.DEFmodifier = 1;
        diff.RNGmodifier = 1;

        gameManager = GameManager.Instance;

        if (gameManager !=null)
        {
            gameManager.inLevel = true;
            gameManager.enableHUD();
        }
        
        SceneManager.LoadScene("StageOneScene");
    }
}
