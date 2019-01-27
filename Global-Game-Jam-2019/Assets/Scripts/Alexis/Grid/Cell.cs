using System; 
using System.Collections;
using System.Collections.Generic;
using System.Linq; 
using UnityEngine;

/*
 * Cell:
 * Created by: Thiebaut Alexis
 * Date: 26/01/2019
 * Description: 
 * 
 * [CHANGES]
 * 26/01/2019 by Thiebaut Alexis
 * Initialisation of the Cell Script
 */

[Serializable]
public class Cell
{
    #region Field and properties
    public Vector2 TilePosition;
    public List<Vector2> LinkedPosition = new List<Vector2>();
    private CellState state = CellState.Free; 
    public CellState State { get { return state; } }

    public float HeuristicCostFromStart { get; set; } = 0;
    public float HeuristicCostToDestination { get; set; } = 0;
    public float HeuristicCostTotal { get; set; } = 0; 
    public bool HasBeenSelected { get; set; } = false; 
    #endregion

    #region Constructor
    public Cell(Vector2 _tilePosition, List<Vector2> _linkedPositions)
    {
        TilePosition = _tilePosition;
        LinkedPosition = _linkedPositions; 
    }
    #endregion

    #region Methods
    public void CleanCell()
    {
        if (state == CellState.Dirty) state = CellState.Free;
    }
    public void MakeCellDirty() => state = CellState.Dirty;
    public void MakeCellNonNavigable() => state = CellState.NonNavigable;
    public void SetState(CellState _state) => state = _state; 
    #endregion
}

public enum CellState
{
    Free, 
    NonNavigable, 
    Dirty, 
    BlackWater, 
    House
}
