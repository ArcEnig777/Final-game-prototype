using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
        private static MapManager _instance;
        public static MapManager Instance { get { return _instance; } }

        public LayerMask layerB;

        public GameObject overlayPrefab;
        public GameObject overlayContainer;

        public Dictionary<Vector2Int, OverlayTile> map;
        public bool ignoreBottomTiles;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            } else
            {
                _instance = this;
            }
        }

        private static RaycastHit2D? GetFocusedOnTile()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

             LayerMask layerT = LayerMask.GetMask("Walltitles");

            RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero, 20.0f, layerT);

            if (hits.Length > 0)
            {
                return hits.OrderByDescending(i => i.collider.transform.position.z).First();
            }

            return null;
        }

        void Start()
        {
            var tileMaps = gameObject.transform.GetComponentsInChildren<Tilemap>().OrderByDescending(x => x.GetComponent<TilemapRenderer>().sortingOrder);
            map = new Dictionary<Vector2Int, OverlayTile>();

            foreach (var tm in tileMaps)
            {
                BoundsInt bounds = tm.cellBounds;

                for (int z = bounds.max.z; z >= bounds.min.z; z--)
                {
                    for (int y = bounds.min.y; y < bounds.max.y; y++)
                    {
                        for (int x = bounds.min.x; x < bounds.max.x; x++)
                        {
                            if (z == 0 && ignoreBottomTiles)
                                return;

                            if (tm.HasTile(new Vector3Int(x, y, z)))
                            {
                                if (!map.ContainsKey(new Vector2Int(x, y)))
                                {
                                    var overlayTile = Instantiate(overlayPrefab, overlayContainer.transform);
                                    var cellWorldPosition = tm.GetCellCenterWorld(new Vector3Int(x, y, z));
                                    overlayTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z + 1);
                                    overlayTile.GetComponent<SpriteRenderer>().sortingOrder = tm.GetComponent<TilemapRenderer>().sortingOrder;
                                    overlayTile.gameObject.GetComponent<OverlayTile>().gridLocation = new Vector3Int(x, y, z);
                                    if (tm.gameObject.tag == "Wall")
                                    {
                                        //Debug.Log(Physics2D.OverlapCircleAll(new Vector2(x,y), 0.1f, layerB));
                                        overlayTile.gameObject.GetComponent<OverlayTile>().isBlocked = true;
                                    }
                                    else
                                    {
                                        overlayTile.gameObject.GetComponent<OverlayTile>().isBlocked = false;
                                    }
                                    
                                    map.Add(new Vector2Int(x, y), overlayTile.gameObject.GetComponent<OverlayTile>());
                                }
                            }
                        }
                    }
                }
            }
        }
        public List<OverlayTile> GetSurroundingTiles(Vector2Int originTile)
        {
            var surroundingTiles = new List<OverlayTile>();

            Vector2Int TileToCheck = new Vector2Int(originTile.x + 1, originTile.y);
            if (map.ContainsKey(TileToCheck))
            {
                if (Mathf.Abs(map[TileToCheck].transform.position.z - map[originTile].transform.position.z) <= 1 && !map[TileToCheck].isBlocked)
                    surroundingTiles.Add(map[TileToCheck]);
            }

            TileToCheck = new Vector2Int(originTile.x - 1, originTile.y);
            if (map.ContainsKey(TileToCheck))
            {
                if (Mathf.Abs(map[TileToCheck].transform.position.z - map[originTile].transform.position.z) <= 1 && !map[TileToCheck].isBlocked)
                    surroundingTiles.Add(map[TileToCheck]);
            }

            TileToCheck = new Vector2Int(originTile.x, originTile.y + 1);
            if (map.ContainsKey(TileToCheck))
            {
                if (Mathf.Abs(map[TileToCheck].transform.position.z - map[originTile].transform.position.z) <= 1 && !map[TileToCheck].isBlocked)
                    surroundingTiles.Add(map[TileToCheck]);
            }

            TileToCheck = new Vector2Int(originTile.x, originTile.y - 1);
            if (map.ContainsKey(TileToCheck))
            {
                if (Mathf.Abs(map[TileToCheck].transform.position.z - map[originTile].transform.position.z) <= 1 && !map[TileToCheck].isBlocked)
                    surroundingTiles.Add(map[TileToCheck]);
            }

            return surroundingTiles;
        }
}
