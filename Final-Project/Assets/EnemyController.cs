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
    public bool available;
    // Start is called before the first frame update
    void Start()
    {
        HP = 10;
        ATK = 3;
        DEF = 1;
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
            standingOnTile.isPlayerBlocked = false;
            Destroy(gameObject);
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

}
