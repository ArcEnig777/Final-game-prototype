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
    public List<OverlayTile> enemyAttackRangeFinderTiles;
    public List<OverlayTile> targetAttackRangeFinderTiles;
    private RangeFinder rangeFinder;
    public PlayerController target;
    public bool isAttacking = false;
    public bool turn = false;
    
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
        gameManager.enemies.Add(GetComponent<EnemyController>());
        enemyAttackRangeFinderTiles = new List<OverlayTile>();
        targetAttackRangeFinderTiles = new List<OverlayTile>();
        rangeFinder = new RangeFinder();

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
    public int getHP()
    {
        return HP;
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
         if(gameManager.playerPhase)
         {
            gameManager.enemies.Remove(GetComponent<EnemyController>());
            standingOnTile.isPlayerBlocked = false;
            Destroy(gameObject);
            if(GameManager.Instance.enemyUnits == 0)
            {
                GameManager.Instance.changeLevel();
            }
         }

 
         
     }
    private static RaycastHit2D? GetFocusedOnTarget(Vector2 tile)
    {
        Vector2 mousePos2D = tile;

        LayerMask layer = LayerMask.GetMask("Units");

        RaycastHit2D[] pUnit = Physics2D.RaycastAll(mousePos2D, Vector2.zero, 1.0f, layer);

        if (pUnit.Length > 0)
        {
            Debug.Log("I found something interesting");
            return pUnit.OrderByDescending(i => i.collider.transform.position.z).First();
        }

        return null;
    }
    private void enemyGetInRangeAtkTiles()
    {
        enemyAttackRangeFinderTiles = rangeFinder.GetTilesInAtkRange(new Vector2Int(standingOnTile.gridLocation.x, standingOnTile.gridLocation.y), atkRange);

        foreach (var item in enemyAttackRangeFinderTiles)
        {   
            item.ShowTileEnemy();   
        }
 
    }
    private void targetGetInRangeAtkTiles(bool showable)
    {
        targetAttackRangeFinderTiles = rangeFinder.GetTilesInAtkRange(new Vector2Int(target.standingOnTile.gridLocation.x, target.standingOnTile.gridLocation.y), target.atkRange);

        if(showable)
        {
            foreach (var item in targetAttackRangeFinderTiles)
            {   
                item.ShowTileEnemy();   
            }
        }
 
    }
    private void hideAtkRange()
    {
        foreach (var item in enemyAttackRangeFinderTiles)
        {   
            item.HideTile();   
        }
    }
     public void attack()
     {
        StartCoroutine("hitTarget"); 
     }
     
     public IEnumerator hitTarget() 
     {
        //yield return new WaitForSeconds(0.4f);
        enemyGetInRangeAtkTiles();
        Debug.Log(atkRange);
        foreach (var item in enemyAttackRangeFinderTiles)
        {
            Debug.Log(item);

            if(item.isPlayer)
            {
                yield return new WaitForSeconds(0.5f);
                foreach (PlayerController ally in gameManager.allies)
                {
                    if(ally.standingOnTile == item)
                    {
                        if(target == null)
                        {
                            target = ally;
                        }
                        else
                        {
                            if(ally.getDefense()>=ATK)
                            {
                                continue;
                            }
                            else if((ally.getDefense()-ATK)>=ally.getHP())
                            {
                                target = ally;
                            }
                            else if(target.getDefense()>ally.getDefense())
                            {
                                target = ally;
                            }
                        }
                    }
                }
            }
            else
            {
                continue;
            }
        }
        hideAtkRange();
        if (target != null)
        {
            targetGetInRangeAtkTiles(false);

            target.takeDamage(ATK - target.getDefense());
            
            Debug.Log(target);

            yield return new WaitForSeconds(0.5f);

            Debug.Log(target);

            if (target != null && targetAttackRangeFinderTiles.Contains(standingOnTile))
            {
                takeDamage(target.getAttack() - DEF);

            }
            else if (target == null && GameManager.Instance.playerUnits == 0)
            {
                GameManager.Instance.returnMenu();
            }

            yield return new WaitForSeconds(0.4f);
        }

        isAttacking = false;
        target = null;
     }


}


                /*yield return new WaitForSeconds(0.2f);
                RaycastHit2D? hit = GetFocusedOnTarget(new Vector2(item.gridLocation.x, item.gridLocation.y));
                if(hit.HasValue)
                {
                     Debug.Log(hit.Value.collider.gameObject.GetComponent<PlayerController>().type);
                    if(target == null)
                    {
                        target = hit.Value.collider.gameObject.GetComponent<PlayerController>();
                    }
                    else
                    {
                        if(hit.Value.collider.gameObject.GetComponent<PlayerController>().getDefense()>=ATK)
                        {
                            continue;
                        }
                        else if ((hit.Value.collider.gameObject.GetComponent<PlayerController>().getDefense() - ATK) >= hit.Value.collider.gameObject.GetComponent<PlayerController>().getHP())
                        {
                            target = hit.Value.collider.gameObject.GetComponent<PlayerController>();
                        }
                        else if(target.getDefense()>hit.Value.collider.gameObject.GetComponent<PlayerController>().getDefense())
                        {
                            target = hit.Value.collider.gameObject.GetComponent<PlayerController>();
                        }
                    }
                }*/