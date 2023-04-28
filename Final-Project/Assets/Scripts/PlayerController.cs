using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    public OverlayTile standingOnTile;
    public OverlayTile prevTile;
    public int HP;
    public int maxHP;
    public int ATK;
    public int DEF;
    public int atkRange;
    public int MOV;
    public int deathPred;
    public bool available;
    public SpriteRenderer sprite; 
    public static event Action onAnyAttack;
    public static event Action onDeath;
    public static event Action onFade;
    public static event Action onNoDamage;
    public GameManager gameManager;
    public string type;
    public bool turnEnd;
    public Color defColor;

    void Start()
    {
        HP = 15 + (GameManager.Instance.Level * 5);
        ATK = 5 + (GameManager.Instance.Level * 2);
        DEF = 1 + (GameManager.Instance.Level * 2);
        MOV = 3;
        atkRange = 1;
        sprite = GetComponent<SpriteRenderer>();
        defColor = sprite.color;
        gameManager = GameManager.Instance;
        gameManager.playerUnits += 1;
        gameManager.playerTurns += 1;
        gameManager.allies.Add(GetComponent<PlayerController>());
        turnEnd = false;

        if(type == "Tank")
        {
            HP += 5;
            DEF += 2;
        }

        if(type == "Flier")
        {
            MOV += 2;
            ATK +=3;
            DEF -= 1;
        }

        if(type == "Archer")
        {
            HP -= 3;
            MOV -= 1;
            ATK +=2;
            DEF -= 1;
            atkRange += 1;
        }

        if(type == "Soldier")
        {
            HP +=2;
            ATK +=1;
            DEF += 1;
        }

        if(type == "Lord")
        {
            HP +=10;
            ATK +=2;
            DEF += 2;
            MOV += 1;
            atkRange +=1;
        }

        if(type == "Mage")
        {
            HP +=0;
            ATK +=4;
            DEF -=2;
            atkRange +=4;
            MOV -= 1;
        }

        maxHP = HP;
    }

    public void takeDamage(int damage)
    {
        if(damage<=0)
        {
            damage = 0;
        }

        gameManager.damageTaken += damage;
        Debug.Log(type + "has taken " + damage + " points of damage");

        if((HP-damage)<=0)
        {
            HP = 0;
        }
        else
        {
            HP = HP - damage;
        }


        if(damage<=0)
        {
            StartCoroutine("noDamage");
        }
        else if (HP <= 0)
        {
            StartCoroutine("Death");
        }
        else
        {
           StartCoroutine("Blink"); 
        }
    }

    public void recoverHP(int heal)
    {
        HP = HP + heal;


        if (HP>=maxHP)
        {
            HP = maxHP;
        }
    }

    public int getAttack()
    {
        return ATK;
    }

    public int getDefense()
    {
        return DEF;
    }

    public int getHP()
    {
        return HP;
    }

    public int getmaxHP()
    {
        return maxHP;
    }

    public string getType()
    {
        return type;
    }

    private IEnumerator Blink() {
 
         Color defaultColor = sprite.color;
 
         sprite.color = new Color(1, 1, 1,0);

         onAnyAttack.Invoke();
 
         yield return new WaitForSeconds(0.5f);

         if(turnEnd && !gameManager.playerPhase)
         {
            sprite.color = defaultColor ;
         }
         else
          {  
            sprite.color = new Color(0.2f,0.2f,0.2f,1.0f);
          }
     }

    private IEnumerator noDamage() {

         Color defaultColor = sprite.color;
         
         onNoDamage.Invoke();
 
         yield return new WaitForSeconds(0.5f);

         if(turnEnd && !gameManager.playerPhase)
         {
            sprite.color = defaultColor ;
         }
         else
          {  
            sprite.color = new Color(0.2f,0.2f,0.2f,1.0f);
          }
     }
    private IEnumerator Death() {
 
         Color defaultColor = sprite.color;
 
         sprite.color = new Color(1, 1, 1,0);

         onDeath.Invoke();
 
         yield return new WaitForSeconds(0.4f);

         onFade.Invoke();
        
         gameManager.playerUnits -= 1;
         if(gameManager.playerPhase)
         {
            gameManager.playerTurns -= 1;
         }
         gameManager.allies.Remove(GetComponent<PlayerController>());
         standingOnTile.isPlayer = false;
         Destroy(gameObject);
 
         
     }

     public void colorChange(Color c)
     {
        sprite.color = c;
     }
}



