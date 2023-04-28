using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultsMenuHandler : MonoBehaviour
{
    // Start is called before the first frame update

   public GameManager gameManager;
   public Text dTaken;
   public Text dDealt;
   public Text turns;

   void Start()
   {
        gameManager = GameManager.Instance;
        setDtaken();
        setDDealt();
        setTurns();
   }

   public void EndSession()
   {
        gameManager.damageTaken = 0;
        gameManager.damageDealt = 0;
        gameManager.turnsTaken = 0;
        SceneManager.LoadScene("MenuScene");
   }
   
   public void setDtaken()
   {
        dTaken.text = ""+gameManager.damageTaken;
   }

   public void setDDealt()
   {
        dDealt.text = ""+gameManager.damageDealt;
   }

   public void setTurns()
   {
        turns.text = ""+gameManager.turnsTaken;
   }
}
