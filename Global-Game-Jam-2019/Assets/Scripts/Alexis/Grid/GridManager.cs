﻿using System.Collections;
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
    private List<Cell> cells = new List<Cell>();
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

    private void OnDrawGizmos()
    {
        if (cells.Count == 0) return; 
        Gizmos.color = Color.red;
        for (int i = 0; i < cells.Count; i++)
        {
            if (cells[i].LinkedPosition.Count == 0) return; 
            for (int j = 0; j < cells[i].LinkedPosition.Count; j++)
            {
                Gizmos.DrawLine(cells[i].TilePosition, cells[i].LinkedPosition[j]); 
            }    
        }
    }
    #endregion
}
