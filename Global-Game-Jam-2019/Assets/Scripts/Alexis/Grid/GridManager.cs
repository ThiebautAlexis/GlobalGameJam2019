using System.Collections;
using System.Collections.Generic;
using System.Linq; 
using UnityEngine;
using UnityEngine.Tilemaps; 

/*
 * GridManager:
 * Created by: Thiebaut Alexis
 * Date: 26/01/2019
 * Description: 
 * 
 * [CHANGES]
 * 26/01/2019 by Thiebaut Alexis
 * Initialisation of the GridManager Script
 */
public class GridManager : MonoBehaviour
{
    #region Field and properties
    public static GridManager Instance; 

    [SerializeField] Tilemap tilemap; 
    public Tilemap Tilemap { get { return tilemap; } }
    [SerializeField] private List<Cell> cells = new List<Cell>();
    public List<Cell> Cells { get { return cells; } }
    #endregion

    #region Methods
    /// <summary>
    /// Generate the grid with a radius for the X and Y Axis
    /// </summary>
    public void GenerateGrid()
    {
        if (cells.Count > 0)
        {
            Debug.Log("Clearing Grid");
            cells.Clear();
        }
        Vector2 _tilePosition;
        Vector3Int _coordinate = new Vector3Int(0, 0, 0);
        for (int i = -tilemap.size.x; i < tilemap.size.x; i++)
        {
            for (int j = -tilemap.size.y; j < tilemap.size.y; j++)
            {
                _coordinate.x = i;
                _coordinate.y = j;
                if(tilemap.HasTile(_coordinate))
                {
                    _tilePosition = tilemap.GetCellCenterWorld(_coordinate);
                    Tile _tile = (Tile)tilemap.GetTile(_coordinate);
                    _tile.colliderType = Tile.ColliderType.Grid;
                    List<Vector2> _linkedTiles = new List<Vector2>();
                    //
                    _coordinate = new Vector3Int(i + 1, j, 0);
                    if (tilemap.HasTile(_coordinate))
                        _linkedTiles.Add(tilemap.GetCellCenterWorld(_coordinate));
                    _coordinate = new Vector3Int(i - 1, j, 0);
                    if (tilemap.HasTile(_coordinate))
                        _linkedTiles.Add(tilemap.GetCellCenterWorld(_coordinate));
                    _coordinate = new Vector3Int(i , j +1, 0);
                    if (tilemap.HasTile(_coordinate))
                        _linkedTiles.Add(tilemap.GetCellCenterWorld(_coordinate));
                    _coordinate = new Vector3Int(i , j -1, 0);
                    if (tilemap.HasTile(_coordinate))
                        _linkedTiles.Add(tilemap.GetCellCenterWorld(_coordinate));

                    //
                    Cell _cell = new Cell(_tilePosition, _linkedTiles); 
                    cells.Add(_cell);
                }
            }
        }
    }

    /// <summary>
    /// Clear the grid list and destroy all cells
    /// </summary>
    public void ClearGrid()
    {
        if (cells.Count > 0)
        {
           cells.Clear();
        }
    }

    /// <summary>
    /// Check the path to reach a particulary tile
    /// </summary>
    /// <returns></returns>
    public List<Cell> ComputePath(Cell _start, Cell _end)
    {

        return null; 
    }

    public Cell GetClosestCell(Vector2 _position)
    {
        return cells.OrderBy(c => Vector2.Distance(_position, c.TilePosition)).FirstOrDefault(); 
    }

    public Vector3 GetStraightLine(Cell _start, Cell _end)
    {
        Vector3 _dir = (_end.TilePosition - _start.TilePosition).normalized;
        Vector3 _orthoCellSize = new Vector3(tilemap.cellSize.x, -tilemap.cellSize.y, 0).normalized;
        if (_dir == tilemap.cellSize.normalized || _dir == -tilemap.cellSize.normalized || _dir == _orthoCellSize || _dir == -_orthoCellSize)
            return _dir ;
        else return Vector3.zero; 
    }

    public Cell GetCellFromPosition(Vector2 _position)
    {
        Cell _c = cells.Where(c => c.TilePosition == _position).FirstOrDefault();
        if (_c == null) return GetClosestCell(_position);
        return _c; 
    }

    #endregion

    #region UnityMethods
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return; 
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion
}
