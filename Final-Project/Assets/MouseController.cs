
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using static ArrowTranslator;

public class MouseController : MonoBehaviour
{
        public GameObject cursor;
        public float speed;
        public GameObject characterPrefab;
        public PlayerController character;
        public EnemyController target;

        private PathFinder pathFinder;
        private RangeFinder rangeFinder;
        private ArrowTranslator arrowTranslator;
        public List<OverlayTile> path;
        public List<OverlayTile> rangeFinderTiles;
        public List<OverlayTile> attackRangeFinderTiles;
        public List<OverlayTile> enemyAttackRangeFinderTiles;
        public bool isMoving;
        public bool isAttacking;
        public bool routine;
        public bool ShowingErange;
        public SpriteRenderer characterSprite;
        public GameManager gameManager;

        private void Start()
        {
            pathFinder = new PathFinder();
            rangeFinder = new RangeFinder();
            arrowTranslator = new ArrowTranslator();

            path = new List<OverlayTile>();
            isMoving = false;
            isAttacking = false;
            routine =false;
            rangeFinderTiles = new List<OverlayTile>();
            attackRangeFinderTiles = new List<OverlayTile>();
            enemyAttackRangeFinderTiles = new List<OverlayTile>();
            gameManager = GameManager.Instance;
        }

        void LateUpdate()
        {
            if(gameManager.playerPhase)
            {
                if(GetComponent<SpriteRenderer>().enabled == false)
                {
                    GetComponent<SpriteRenderer>().enabled = true;
                }
                RaycastHit2D? hit = GetFocusedOnTile();
                if (hit.HasValue)
                {
                    OverlayTile tile = hit.Value.collider.gameObject.GetComponent<OverlayTile>();
                    cursor.transform.position = tile.transform.position;
                    cursor.gameObject.GetComponent<SpriteRenderer>().sortingOrder = tile.transform.GetComponent<SpriteRenderer>().sortingOrder;

                    if (rangeFinderTiles.Contains(tile) && !isMoving && !isAttacking && character != null && !character.turnEnd)
                    {
                        path = pathFinder.FindPath(character.standingOnTile, tile, rangeFinderTiles);

                        foreach (var item in rangeFinderTiles)
                        {
                            MapManager.Instance.map[item.grid2DLocation].SetSprite(ArrowDirection.None);
                        }

                        for (int i = 0; i < path.Count; i++)
                        {
                            var previousTile = i > 0 ? path[i - 1] : character.standingOnTile;
                            var futureTile = i < path.Count - 1 ? path[i + 1] : null;

                            var arrow = arrowTranslator.TranslateDirection(previousTile, path[i], futureTile);
                            path[i].SetSprite(arrow);
                        }
                    }

                    if (Input.GetMouseButtonDown(0) && !isAttacking && !isMoving)
                    {

                        RaycastHit2D? unit = GetFocusedOnCharacter();
                        RaycastHit2D? enRange = GetFocusedOnEnemy();

                        if(unit.HasValue)
                        {

                            Debug.Log(unit.Value);

                            if (character != null)
                            {
                                if(unit.Value.collider.gameObject.GetComponent<PlayerController>().standingOnTile == character.standingOnTile)
                                {
                                    GetInRangeAtkTiles();
                                    isAttacking = true;
                                }
                                else
                                {
                                    if(unit.Value.collider.gameObject.GetComponent<PlayerController>().turnEnd)
                                    {
                                        foreach (var item in rangeFinderTiles)
                                        {
                                            MapManager.Instance.map[item.grid2DLocation].SetSprite(ArrowDirection.None);
                                        }
                                        Debug.Log("Unit has already taken their turn");
                                        character = null;
                                    }
                                    else
                                    {
                                        character = unit.Value.collider.gameObject.GetComponent<PlayerController>();
                                        characterSprite = unit.Value.collider.gameObject.GetComponent<SpriteRenderer>();
                                        RaycastHit2D? player = GetFocusedOnPlayer(character);
                                        OverlayTile ptile = player.Value.collider.gameObject.GetComponent<OverlayTile>();
                                        PositionCharacterOnLine(ptile);
                                        character.standingOnTile = ptile;
                                        character.prevTile = ptile;
                                        GetInRangeTiles();
                                    }

                                }
                            }
                            else if(!unit.Value.collider.gameObject.GetComponent<PlayerController>().turnEnd)
                            {
                                character = unit.Value.collider.gameObject.GetComponent<PlayerController>();
                                characterSprite = unit.Value.collider.gameObject.GetComponent<SpriteRenderer>();

                                Debug.Log(character);

                                RaycastHit2D? player = GetFocusedOnPlayer(character);
                                OverlayTile ptile = player.Value.collider.gameObject.GetComponent<OverlayTile>();
                                PositionCharacterOnLine(ptile);
                                character.standingOnTile = ptile;
                                character.prevTile = ptile;
                                GetInRangeTiles();
                            }

                        }

                        else if(character != null)
                        {
                            if (character.standingOnTile == null)
                            {
                                RaycastHit2D? player = GetFocusedOnPlayer(character);
                                OverlayTile ptile = player.Value.collider.gameObject.GetComponent<OverlayTile>();
                                PositionCharacterOnLine(ptile);
                                character.prevTile = ptile;
                                character.standingOnTile = ptile;
                            } 
                            else if (path.Count > 0)
                            {
                                character.prevTile = character.standingOnTile;
                                isMoving = true;
                                foreach (var item in rangeFinderTiles)
                                {
                                    MapManager.Instance.map[item.grid2DLocation].SetSprite(ArrowDirection.None);
                                }
                                tile.gameObject.GetComponent<OverlayTile>().HideTile();
                            }
                            else
                            {
                                character = null;
                                characterSprite = null;
                            }
                        }
                        else if (enRange.HasValue && !isAttacking && !isMoving)
                        {
                            target = enRange.Value.collider.gameObject.GetComponent<EnemyController>();
                            enemyGetInRangeAtkTiles(true);

                        }

                    }
                    else if (Input.GetMouseButtonDown(1) && character != null && !isMoving)
                    {

                        foreach (var item in rangeFinderTiles)
                        {
                            MapManager.Instance.map[item.grid2DLocation].SetSprite(ArrowDirection.None);
                        }
                        PositionCharacterOnLine(character.prevTile);
                        character = null;
                        characterSprite = null;
                        isAttacking = false;

                    }
                    else if (Input.GetMouseButtonDown(0) && isAttacking && !character.turnEnd)
                    {
                        RaycastHit2D? ehit = GetFocusedOnTile();

                        if (ehit.HasValue)
                        {
                            OverlayTile etile = hit.Value.collider.gameObject.GetComponent<OverlayTile>();

                            if(attackRangeFinderTiles.Contains(etile) && tile.isPlayerBlocked)
                            {
                                Debug.Log("I found an enemy!");
                                RaycastHit2D? enemy = GetFocusedOnEnemy();

                                if (enemy.HasValue)
                                {
                                    target = enemy.Value.collider.gameObject.GetComponent<EnemyController>();
                                    enemyGetInRangeAtkTiles(false);

                                    routine = true;
                                    StartCoroutine(AttackEnemy());
                                    
                                }

                            }
                            else if (!routine)
                            {
                                isAttacking = false;
                                character.standingOnTile.isPlayer = true;
                                character.turnEnd = true;
                                character.colorChange(new Color(0.2f,0.2f,0.2f,1.0f));
                                gameManager.playerTurns -=1;
                                character = null;
                                characterSprite = null;
                                target = null;
                                GetComponent<SpriteRenderer>().enabled = false;
                                gameManager.enemyPhase();
                            }
                        }
                    }
                }

                if(isMoving && character != null)
                {
                    character.standingOnTile.isPlayer = false;
                    MoveAlongPath();
                }
            }
            

        }

        private void MoveAlongPath()
        {
            var step = speed * Time.deltaTime;
            float zIndex = path[0].transform.position.z;
            character.transform.position = Vector2.MoveTowards(character.transform.position, path[0].transform.position, step);
            character.transform.position = new Vector3(character.transform.position.x, character.transform.position.y, zIndex);

            if(Vector2.Distance(character.transform.position, path[0].transform.position) < 0.00001f)
            {
                PositionCharacterOnLine(path[0]);
                path.RemoveAt(0);
            }
            if(path.Count <= 0)
            {
                isMoving = false;
                isAttacking = true;
                GetInRangeAtkTiles();
            }

        }

        private void PositionCharacterOnLine(OverlayTile tile)
        {
            character.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y, tile.transform.position.z);
            character.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder;
            character.standingOnTile = tile;
        }

        private static RaycastHit2D? GetFocusedOnTile()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

             LayerMask layerT = LayerMask.GetMask("Solidtitles");

            RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero, 20.0f, layerT);

            if (hits.Length > 0)
            {
                return hits.OrderByDescending(i => i.collider.transform.position.z).First();
            }

            return null;
        }

        private static RaycastHit2D? GetFocusedOnCharacter()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            LayerMask layer = LayerMask.GetMask("Units");

            RaycastHit2D[] pUnit = Physics2D.RaycastAll(mousePos2D, Vector2.zero, 20.0f, layer);

            if (pUnit.Length > 0)
            {
                Debug.Log("I found something interesting");
                return pUnit.OrderByDescending(i => i.collider.transform.position.z).First();
            }

            return null;
        }

        private static RaycastHit2D? GetFocusedOnEnemy()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            LayerMask layer = LayerMask.GetMask("Enemy");

            RaycastHit2D[] pUnit = Physics2D.RaycastAll(mousePos2D, Vector2.zero, 20.0f, layer);

            if (pUnit.Length > 0)
            {
                Debug.Log("I found something interesting");
                return pUnit.OrderByDescending(i => i.collider.transform.position.z).First();
            }

            return null;
        }

        private static RaycastHit2D? GetFocusedOnPlayer(PlayerController c)
        {
            Vector2 char2D = new Vector2(c.transform.position.x, c.transform.position.y);

            LayerMask layerT = LayerMask.GetMask("Solidtitles");

            RaycastHit2D[] hits = Physics2D.RaycastAll(char2D, Vector2.zero, 20.0f, layerT);

            if (hits.Length > 0)
            {
                return hits.OrderByDescending(i => i.collider.transform.position.z).First();
            }

            return null;
        }
        private void GetInRangeTiles()
        {
            rangeFinderTiles = rangeFinder.GetTilesInRange(new Vector2Int(character.standingOnTile.gridLocation.x, character.standingOnTile.gridLocation.y), character.MOV);

            foreach (var item in rangeFinderTiles)
            {   
                    item.ShowTile();   
            }
        }
        private void GetInRangeAtkTiles()
        {
            attackRangeFinderTiles = rangeFinder.GetTilesInAtkRange(new Vector2Int(character.standingOnTile.gridLocation.x, character.standingOnTile.gridLocation.y), character.atkRange);

            foreach (var item in attackRangeFinderTiles)
            {   
                    item.ShowTileEnemy();   
            }
        }
        private void enemyGetInRangeAtkTiles(bool showable)
        {
            enemyAttackRangeFinderTiles = rangeFinder.GetTilesInAtkRange(new Vector2Int(target.standingOnTile.gridLocation.x, target.standingOnTile.gridLocation.y), target.atkRange);

            if(showable)
            {
                foreach (var item in enemyAttackRangeFinderTiles)
                {   
                    item.ShowTileEnemy();   
                }
            }
 
        }
        private IEnumerator AttackEnemy() {

            target.takeDamage(character.getAttack() - target.getDefense());
            
            Debug.Log(target);

            yield return new WaitForSeconds(0.5f);

            Debug.Log(target);

            if (target != null && enemyAttackRangeFinderTiles.Contains(character.standingOnTile))
            {
                character.takeDamage(target.getAttack() - character.getDefense());

                if(character == null && GameManager.Instance.playerUnits == 0)
                {
                    isAttacking = false;
                    character = null;
                    characterSprite = null;
                    target = null;
                    routine = false;
                    GameManager.Instance.returnMenu();
                }
            }
            else if (target == null && GameManager.Instance.enemyUnits == 0)
            {
                isAttacking = false;
                character = null;
                characterSprite = null;
                target = null;
                routine = false;

                GameManager.Instance.changeLevel();

            }

            yield return new WaitForSeconds(0.5f);
            isAttacking = false;
            character.standingOnTile.isPlayer = true;
            character.turnEnd = true;
            character.colorChange(new Color(0.2f,0.2f,0.2f,1.0f));
            gameManager.playerTurns -=1;
            character = null;
            characterSprite = null;
            target = null;
            routine = false;
            GetComponent<SpriteRenderer>().enabled = false;
            gameManager.enemyPhase();

        }
        private IEnumerator AttackCharacter() {

            character.takeDamage(target.getAttack() - character.getDefense());

            yield return new WaitForSeconds(0.5f);

        }

   
}
