using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public OverlayTile standingOnTile;
    public int HP;
    public int ATK;
    public int DEF;
    public bool available; 

    void Start()
    {
        HP = 15;
        ATK = 5;
        DEF = 1;
    }

    public void takeDamage(int damage)
    {
        HP = HP - damage;

        if (HP <= 0)
        {
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

/*public float mSpeed = 5f;
    public Transform movePoint;
    public LayerMask noMoveMask;
    // Start is called before the first frame update
    void Start()
    {
        movePoint.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position,movePoint.position, mSpeed*Time.deltaTime);

        if(Vector3.Distance(transform.position, movePoint.position) <= .05f)
        {
            if(Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
            {
                if(!Physics2D.OverlapCircle(movePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f,0f), .2f, noMoveMask))
                {
                    movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f,0f);
                }
            }
            else if(Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {
                if(!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, Input.GetAxisRaw("Vertical"),0f), .2f, noMoveMask))
                {
                    movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"),0f);
                }
            }
        }

    }*/

