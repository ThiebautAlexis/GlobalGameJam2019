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

    [SerializeField] Sprite dirtyTile;
    [SerializeField] Sprite[] decorTiles;
    [SerializeField] Sprite house;
    [SerializeField] BlackMatter blackMatterPrefab; 
    #endregion

    #region Methods
    #region Grid
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

                    Cell _cell = new Cell(_tilePosition, _linkedTiles);

                    //
                    Sprite _tileSprite = _tile.sprite; 
                    if(dirtyTile == _tileSprite)
                    {
                        _cell.SetState(CellState.BlackWater); 
                    }
                    else if(decorTiles.Any(s => s == _tileSprite))
                    {
                        _cell.SetState(CellState.NonNavigable);
                    }
                    else if(_tileSprite == house)
                    {
                        _cell.SetState(CellState.House); 
                    }
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

    #region SpawnGarbages
    void SpawnGarbage()
    {
        Cell _makeDirtyCell;
        if (!cells.Any(c => c.State == CellState.Dirty))
        {
            List<Cell> _freeCells = cells.Where(c => c.State == CellState.Free).ToList();
            int _index = Random.Range(0, _freeCells.Count - 1);
            _makeDirtyCell = _freeCells[_index];
            _makeDirtyCell.SetState(CellState.Dirty);
            BlackMatter _d = Instantiate(blackMatterPrefab, _makeDirtyCell.TilePosition, Quaternion.identity);
            _d.LinkedCell = _makeDirtyCell;
        }
        else
        {
            List<Cell> _dirtyCells = cells.Where(c => c.State == CellState.Dirty).ToList();
            List<Cell> _linkedFreeCells;
            Cell _cell; 
            foreach (Cell c in _dirtyCells)
            {
                _linkedFreeCells = new List<Cell>(); 
                for (int i = 0; i < c.LinkedPosition.Count; i++)
                {
                    _cell = GetCellFromPosition(c.LinkedPosition[i]); 
                    if(_cell.State == CellState.Free)
                        _linkedFreeCells.Add(_cell); 
                }
                if (_linkedFreeCells.Count == 0) continue; 
                int _index = Random.Range(0, _linkedFreeCells.Count - 1);
                _makeDirtyCell = _linkedFreeCells[_index]; 
                _makeDirtyCell.SetState(CellState.Dirty);
                BlackMatter _d = Instantiate(blackMatterPrefab, _makeDirtyCell.TilePosition, Quaternion.identity);
                _d.LinkedCell = _makeDirtyCell; 
            }
        }
    }
    #endregion
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
        InvokeRepeating("SpawnGarbage", 0, 5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        /*
        for (int i = 0; i < cells.Count; i++)
        {
            switch (cells[i].State)
            {
                case CellState.Free:
                    Gizmos.color = Color.green; 
                    break;
                case CellState.NonNavigable:
                    Gizmos.color = Color.red;
                    break;
                case CellState.Dirty:
                    Gizmos.color = Color.blue;
                    break;
                case CellState.BlackWater:
                    Gizmos.color = Color.black;
                    break;
                case CellState.House:
                    Gizmos.color = Color.cyan; 
                    break;
                default:
                    break;
            }
            Gizmos.DrawSphere(cells[i].TilePosition, .2f); 
            }
            */
        Gizmos.color = Color.red; 
        for (int i = 0; i < cells.Count; i++)
        {
            for (int j = 0; j < cells[i].LinkedPosition.Count; j++)
            {
                Gizmos.DrawLine(cells[i].TilePosition, cells[i].LinkedPosition[j]); 
            }
        }
        
    }
    #endregion
}
