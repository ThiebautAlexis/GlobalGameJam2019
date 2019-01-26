using System; 
using System.Collections;
using System.Collections.Generic;
using System.Linq; 
using UnityEngine;
using UnityEngine.Tilemaps; 

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

    #endregion

    #region Constructor
    public Cell(Vector2 _tilePosition, List<Vector2> _linkedPositions)
    {
        TilePosition = _tilePosition;
        LinkedPosition = _linkedPositions; 
    }
    #endregion

    #region Methods

    #endregion
}
