using System.Collections;
using System.Collections.Generic;
using System.Linq; 
using UnityEngine;

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
    private Vector2 tileExtend = new Vector2(2, 1); 
    [SerializeField, Range(10, 25)] private int xSize = 10;
    [SerializeField, Range(10, 25)] private int ySize = 10;
    [SerializeField] GameObject cellPrefab; 

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
            cells.ForEach(c => DestroyImmediate(c.gameObject));
            cells.Clear(); 
        }
        if (!cellPrefab)
        {
            Debug.LogError("Prefab Not Found"); 
            return; 
        }
        Cell _cell; 
        Vector2 _cellPosition;
        for (int i = -xSize; i < xSize; i++)
        {
            for (int j = -ySize; j < ySize; j++)
            {
                _cellPosition = i % 2 != 0 ?  new Vector3(i, j) : new Vector3(i, j + .5f); 
                _cell = Instantiate(cellPrefab, _cellPosition, Quaternion.identity, this.transform).GetComponent<Cell>();
                _cell.gameObject.name = $"Cell: {i},{j}";
                cells.Add(_cell); 
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
            cells.ForEach(c => DestroyImmediate(c.gameObject));
            cells.Clear();
        }
    }
    #endregion

    #region UnityMethods
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
