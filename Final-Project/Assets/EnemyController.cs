using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class EnemyController : MonoBehaviour
{
    public OverlayTile standingOnTile;
    public int HP;
    public int ATK;
    public int DEF;
    public int atkRange;
    public int MOV;
    public bool available;
    public SpriteRenderer sprite; 
    public static event Action onAnyAttack;
    public static event Action onDeath;
    public GameManager gameManager;
    public string type;
    
    // Start is called before the first frame update
    void Start()
    {
        HP = 10 + (GameManager.Instance.Level * 5);
        ATK = 3 + (GameManager.Instance.Level * 2);
        DEF = 1 + (GameManager.Instance.Level * 2);
        MOV = 3;
        atkRange = 1;
        sprite = GetComponent<SpriteRenderer>();
        gameManager = GameManager.Instance;
        gameManager.enemyUnits += 1;

        if(type == "Tank")
        {
            HP += 5;
            DEF += 2;
            gameManager.enemyUnits -= 1;
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

        if(type == "Commander")
        {
            HP +=10;
            ATK +=2;
            DEF += 2;
            atkRange +=1;
        }
    }

    void Update()
    {
        if (standingOnTile == null)
        {
            Debug.Log(transform.position.y);
            RaycastHit2D? enemy = GetFocusedOnEnemy(new Vector2(transform.position.x, transform.position.y));
            if (enemy.HasValue)
            {
                OverlayTile ptile = enemy.Value.collider.gameObject.GetComponent<OverlayTile>();
                Debug.Log(ptile);
                transform.position = new Vector3(ptile.transform.position.x, ptile.transform.position.y, ptile.transform.position.z);
                gameObject.GetComponent<SpriteRenderer>().sortingOrder = ptile.GetComponent<SpriteRenderer>().sortingOrder;
                standingOnTile = ptile;
                ptile.isPlayerBlocked = true;
            }
        }
    }

    private static RaycastHit2D? GetFocusedOnEnemy(Vector2 c)
    {
        Vector2 char2D = c;
        LayerMask layerT = LayerMask.GetMask("Solidtitles");

        RaycastHit2D[] hits = Physics2D.RaycastAll(char2D, Vector2.zero, 20.0f, layerT);

        if (hits.Length > 0)
        {
            return hits.OrderByDescending(i => i.collider.transform.position.z).First();
        }

        return null;
    }

    public void takeDamage(int damage)
    {
        
        HP = HP - damage;


        if (HP <= 0)
        {
            StartCoroutine("Death");
        }
        else
        {
           StartCoroutine("Blink"); 
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

    private IEnumerator Blink() {
 
         Color defaultColor = sprite.color;
 
         sprite.color = new Color(1, 1, 1,0);

         onAnyAttack.Invoke();
 
         yield return new WaitForSeconds(0.5f);
 
         sprite.color = defaultColor ;
     }

    private IEnumerator Death() {
 
         Color defaultColor = sprite.color;
 
         sprite.color = new Color(1, 1, 1,0);

         onDeath.Invoke();
 
         yield return new WaitForSeconds(0.4f);

         standingOnTile.isPlayerBlocked = false;
         if(type != "Tank")
         {
            gameManager.enemyUnits -= 1;
         }
         
         Destroy(gameObject);
 
         
     }

}
