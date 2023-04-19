using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int playerUnits;
    public int playerTurns;
    public int enemyUnits;
    public List<EnemyController> enemies;
    public List<PlayerController> allies;
    public int Level;
    public int nextLevel;
    public static GameManager Instance;
    public bool playerPhase;
    public bool playerPhaseText = false;
    public Text phaseText;
    public Canvas gameCanvas;
    public int tSpeed;
    public Vector3 phaseTextOrigin;

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
        tSpeed = 5;
        phaseTextOrigin = new Vector3(0, 0,0);
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(phaseText);
        DontDestroyOnLoad(gameCanvas);
        
    }

    // Update is called once per frame
    void Update()
    {
        if(gameCanvas.worldCamera == null)
        {
              gameCanvas.renderMode = RenderMode.ScreenSpaceCamera;
              gameCanvas.worldCamera = Camera.main;
        }
    }

    public void changeLevel()
    {
        Level += 1;
        playerUnits = 0;
        playerTurns = 0;
        enemyUnits = 0;
        enemies.Clear();
        allies.Clear();

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

    public void enemyPhase()
    {
        if(playerTurns <=0)
        {
            playerPhase = false;
            playerPhaseText = false;
        }
        if(!playerPhase)
        {
            Debug.Log("enemy phase");
            StartCoroutine("enemyAttack"); 

        }
    }
    private IEnumerator changePhaseText()
    {
        
        phaseText.color = new Color(1, 1, 1,0);
        var step = tSpeed * Time.deltaTime;
        phaseText.transform.position = new Vector3(0,0,0);

        if(!playerPhaseText)
        {
            phaseText.text = "Enemy Phase" ;
            phaseText.color = new Color(0.75f, 0, 0,1); 
        }
        else
        {
            phaseText.text = "Player Phase" ;
            phaseText.color = new Color(0.045f, 0.75f, 0.75f, 1);
        }

        yield return new WaitForSeconds(0.7f);

        phaseText.transform.position = phaseTextOrigin;
        phaseText.color = new Color(1, 1, 1,0);
    }
    private IEnumerator enemyAttack()
    {
            foreach(PlayerController ally in allies)
            {
                ally.sprite.color = ally.defColor;
            }

            yield return StartCoroutine("changePhaseText");

            foreach(EnemyController enemy in enemies)
            {
                yield return StartCoroutine(enemy.hitTarget());
                //enemy.attack();
                //yield return new WaitForSeconds(0.8f);
            }

            for(int i = 0; i < enemies.Count; i++)
            {
                if(enemies[i].getHP()<=0)
                {
                    var temp = enemies[i];
                    enemies.RemoveAt(i);
                    temp.standingOnTile.isPlayerBlocked = false;
                    Destroy(temp);
                }
                //enemy.attack();
                //yield return new WaitForSeconds(0.8f);
            }

            if(enemyUnits <=0)
            {
                changeLevel();
            }

            foreach(PlayerController ally in allies)
            {
                ally.turnEnd = false;
            }

            playerPhaseText = true;

            yield return StartCoroutine("changePhaseText");

            playerPhase = true;
            playerTurns = playerUnits;
    } 
}
